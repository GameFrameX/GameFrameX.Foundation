// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// 表示使用SM2和SM3算法进行加密和解密的密码类。
/// 该类实现了SM2椭圆曲线公钥密码算法和SM3密码杂凑算法的组合加密方案。
/// </summary>
/// <remarks>
/// Represents a cipher class that uses SM2 and SM3 algorithms for encryption and decryption.
/// This class implements a combined encryption scheme of SM2 elliptic curve public key cryptography and SM3 cryptographic hash algorithm.
/// </remarks>
internal sealed class Cipher
{
    /// <summary>
    /// 密钥生成计数器。
    /// </summary>
    /// <remarks>
    /// Key generation counter.
    /// </remarks>
    private int _ct;

    /// <summary>
    /// 用于加密/解密的椭圆曲线点。
    /// </summary>
    /// <remarks>
    /// Elliptic curve point used for encryption/decryption.
    /// </remarks>
    private ECPoint _p2;

    /// <summary>
    /// 用于密钥生成的基础SM3摘要。
    /// </summary>
    /// <remarks>
    /// Base SM3 digest used for key generation.
    /// </remarks>
    private Sm3Digest _sm3KeyBase;

    /// <summary>
    /// 用于密文的SM3摘要。
    /// </summary>
    /// <remarks>
    /// SM3 digest used for ciphertext.
    /// </remarks>
    private Sm3Digest _sm3C3;

    /// <summary>
    /// 用于加密/解密的密钥。
    /// </summary>
    /// <remarks>
    /// Key used for encryption/decryption.
    /// </remarks>
    private readonly byte[] _key;

    /// <summary>
    /// 密钥偏移量（W-05 修复：从 byte 改为 int，避免潜在截断）。
    /// </summary>
    /// <remarks>
    /// Key offset (W-05 fix: changed from byte to int to avoid potential truncation).
    /// </remarks>
    private int _keyOff;

    /// <summary>
    /// 初始化Cipher类的新实例。
    /// 设置初始计数器并准备密钥数组。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the Cipher class.
    /// Sets the initial counter and prepares the key array.
    /// </remarks>
    public Cipher()
    {
        _ct = 1;
        _key = new byte[32];
        _keyOff = 0;
    }

    /// <summary>
    /// 将BigInteger转换为32字节数组。
    /// </summary>
    /// <remarks>
    /// Converts a BigInteger to a 32-byte array.
    /// </remarks>
    /// <param name="n">要转换的BigInteger数值 / BigInteger value to convert</param>
    /// <returns>返回转换后的32字节数组。如果输入为null，则返回null。 / Returns the converted 32-byte array. Returns null if input is null.</returns>
    public static byte[] ByteConvert32Bytes(BigInteger n)
    {
        if (n == null)
        {
            return null;
        }

        // W-06 修复：缓存 ToByteArray() 结果，避免重复分配
        var nBytes = n.ToByteArray();
        byte[] tmpd;
        if (nBytes.Length == 33)
        {
            tmpd = new byte[32];
            Array.Copy(nBytes, 1, tmpd, 0, 32);
        }
        else if (nBytes.Length == 32)
        {
            tmpd = nBytes;
        }
        else
        {
            tmpd = new byte[32];
            Array.Copy(nBytes, 0, tmpd, 32 - nBytes.Length, nBytes.Length);
        }

        return tmpd;
    }

    /// <summary>
    /// 重置密码状态，初始化SM3摘要并准备进行新的加密/解密。
    /// 该方法会重新初始化SM3摘要对象，并使用椭圆曲线点p2的坐标更新摘要状态。
    /// </summary>
    /// <remarks>
    /// Resets the cipher state, initializes SM3 digest and prepares for new encryption/decryption.
    /// This method reinitializes the SM3 digest objects and updates the digest state using the coordinates of elliptic curve point p2.
    /// </remarks>
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
    /// <remarks>
    /// Generates the next key and updates the key offset.
    /// Uses the SM3 digest algorithm to generate a new key based on the current state.
    /// </remarks>
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
    /// <remarks>
    /// Initializes the encryption process, generates a key pair and prepares for encryption.
    /// </remarks>
    /// <param name="sm2">SM2算法的实例，用于生成密钥对 / SM2 algorithm instance used to generate key pair</param>
    /// <param name="userKey">用户的公钥，用于加密过程 / User's public key used for encryption</param>
    /// <returns>返回生成的临时公钥点c1 / Returns the generated ephemeral public key point c1</returns>
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
    /// <remarks>
    /// Performs the encryption operation.
    /// Encrypts the input data using the generated key and updates the SM3 digest.
    /// </remarks>
    /// <param name="data">要加密的数据字节数组 / Byte array of data to encrypt</param>
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
    /// <remarks>
    /// Initializes the decryption process and prepares for decryption.
    /// </remarks>
    /// <param name="userD">用户的私钥 / User's private key</param>
    /// <param name="c1">加密时生成的临时公钥点 / Ephemeral public key point generated during encryption</param>
    public void Init_dec(BigInteger userD, ECPoint c1)
    {
        _p2 = c1.Multiply(userD);
        Reset();
    }

    /// <summary>
    /// 执行解密操作。
    /// 使用密钥对加密的数据进行解密，并更新SM3摘要。
    /// </summary>
    /// <remarks>
    /// Performs the decryption operation.
    /// Decrypts the encrypted data using the key and updates the SM3 digest.
    /// </remarks>
    /// <param name="data">要解密的数据字节数组 / Byte array of data to decrypt</param>
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
    /// <remarks>
    /// Completes the encryption process and generates the final ciphertext c3.
    /// Uses the SM3 digest to generate the final authentication tag.
    /// </remarks>
    /// <param name="c3">用于存储生成的密文的字节数组 / Byte array to store the generated ciphertext</param>
    public void Dofinal(byte[] c3)
    {
        byte[] p = ByteConvert32Bytes(_p2.Normalize().YCoord.ToBigInteger());
        _sm3C3.BlockUpdate(p, 0, p.Length);
        _sm3C3.DoFinal(c3, 0);
        Reset();
    }
}