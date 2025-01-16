using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// 表示使用SM2和SM3算法进行加密和解密的密码类。
/// 该类实现了SM2椭圆曲线公钥密码算法和SM3密码杂凑算法的组合加密方案。
/// </summary>
internal sealed class Cipher
{
    /// <summary>
    /// 密钥生成计数器
    /// </summary>
    private int _ct;

    /// <summary>
    /// 用于加密/解密的椭圆曲线点
    /// </summary>
    private ECPoint _p2;

    /// <summary>
    /// 用于密钥生成的基础SM3摘要
    /// </summary>
    private Sm3Digest _sm3KeyBase;

    /// <summary>
    /// 用于密文的SM3摘要
    /// </summary>
    private Sm3Digest _sm3C3;

    /// <summary>
    /// 用于加密/解密的密钥
    /// </summary>
    private readonly byte[] _key;

    /// <summary>
    /// 密钥偏移量
    /// </summary>
    private byte _keyOff;

    /// <summary>
    /// 初始化Cipher类的新实例。
    /// 设置初始计数器并准备密钥数组。
    /// </summary>
    public Cipher()
    {
        _ct = 1;
        _key = new byte[32];
        _keyOff = 0;
    }

    /// <summary>
    /// 将BigInteger转换为32字节数组。
    /// </summary>
    /// <param name="n">要转换的BigInteger数值。</param>
    /// <returns>返回转换后的32字节数组。如果输入为null，则返回null。</returns>
    public static byte[] ByteConvert32Bytes(BigInteger n)
    {
        if (n == null)
            return null;

        byte[] tmpd;
        if (n.ToByteArray().Length == 33)
        {
            tmpd = new byte[32];
            Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
        }
        else if (n.ToByteArray().Length == 32)
        {
            tmpd = n.ToByteArray();
        }
        else
        {
            tmpd = new byte[32];
            for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
            {
                tmpd[i] = 0;
            }

            Array.Copy(n.ToByteArray(), 0, tmpd, 32 - n.ToByteArray().Length, n.ToByteArray().Length);
        }

        return tmpd;
    }

    /// <summary>
    /// 重置密码状态，初始化SM3摘要并准备进行新的加密/解密。
    /// 该方法会重新初始化SM3摘要对象，并使用椭圆曲线点p2的坐标更新摘要状态。
    /// </summary>
    private void Reset()
    {
        _sm3KeyBase = new Sm3Digest();
        _sm3C3 = new Sm3Digest();

        byte[] p = ByteConvert32Bytes(_p2.Normalize().XCoord.ToBigInteger());
        _sm3KeyBase.BlockUpdate(p, 0, p.Length);
        _sm3C3.BlockUpdate(p, 0, p.Length);

        p = ByteConvert32Bytes(_p2.Normalize().YCoord.ToBigInteger());
        _sm3KeyBase.BlockUpdate(p, 0, p.Length);
        _ct = 1;
        NextKey();
    }

    /// <summary>
    /// 生成下一个密钥并更新密钥偏移量。
    /// 使用SM3摘要算法基于当前状态生成新的密钥。
    /// </summary>
    private void NextKey()
    {
        var sm3KeyCur = new Sm3Digest(this._sm3KeyBase);
        sm3KeyCur.Update((byte)(_ct >> 24 & 0xff));
        sm3KeyCur.Update((byte)(_ct >> 16 & 0xff));
        sm3KeyCur.Update((byte)(_ct >> 8 & 0xff));
        sm3KeyCur.Update((byte)(_ct & 0xff));
        sm3KeyCur.DoFinal(_key, 0);
        _keyOff = 0;
        _ct++;
    }

    /// <summary>
    /// 初始化加密过程，生成密钥对并准备加密。
    /// </summary>
    /// <param name="sm2">SM2算法的实例，用于生成密钥对。</param>
    /// <param name="userKey">用户的公钥，用于加密过程。</param>
    /// <returns>返回生成的临时公钥点c1。</returns>
    public ECPoint Init_enc(Sm2 sm2, ECPoint userKey)
    {
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        ECPrivateKeyParameters ecPrivateKeyParameters = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecPublicKeyParameters = (ECPublicKeyParameters)key.Public;
        BigInteger k = ecPrivateKeyParameters.D;
        ECPoint c1 = ecPublicKeyParameters.Q;
        _p2 = userKey.Multiply(k);
        Reset();
        return c1;
    }

    /// <summary>
    /// 执行加密操作。
    /// 使用生成的密钥对输入数据进行加密，并更新SM3摘要。
    /// </summary>
    /// <param name="data">要加密的数据字节数组。</param>
    public void Encrypt(byte[] data)
    {
        _sm3C3.BlockUpdate(data, 0, data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            if (_keyOff == _key.Length)
            {
                NextKey();
            }

            data[i] ^= _key[_keyOff++];
        }
    }

    /// <summary>
    /// 初始化解密过程，准备解密。
    /// </summary>
    /// <param name="userD">用户的私钥。</param>
    /// <param name="c1">加密时生成的临时公钥点。</param>
    public void Init_dec(BigInteger userD, ECPoint c1)
    {
        _p2 = c1.Multiply(userD);
        Reset();
    }

    /// <summary>
    /// 执行解密操作。
    /// 使用密钥对加密的数据进行解密，并更新SM3摘要。
    /// </summary>
    /// <param name="data">要解密的数据字节数组。</param>
    public void Decrypt(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (_keyOff == _key.Length)
            {
                NextKey();
            }

            data[i] ^= _key[_keyOff++];
        }

        _sm3C3.BlockUpdate(data, 0, data.Length);
    }

    /// <summary>
    /// 完成加密过程，生成最终的密文c3。
    /// 使用SM3摘要生成最终的认证标签。
    /// </summary>
    /// <param name="c3">用于存储生成的密文的字节数组。</param>
    public void Dofinal(byte[] c3)
    {
        byte[] p = ByteConvert32Bytes(_p2.Normalize().YCoord.ToBigInteger());
        _sm3C3.BlockUpdate(p, 0, p.Length);
        _sm3C3.DoFinal(c3, 0);
        Reset();
    }
}