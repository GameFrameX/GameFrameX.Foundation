using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM2非对称加密算法的实现类。
/// 提供SM2算法所需的参数和基本操作。
/// </summary>
internal sealed class Sm2
{
    private static Sm2 _instance;

    /// <summary>
    /// 获取SM2算法的单例实例。
    /// </summary>
    public static Sm2 Instance
    {
        get { return _instance ??= new Sm2(); }
    }

    /// <summary>
    /// SM2算法的标准参数数组。
    /// 包含椭圆曲线的参数：p（模数）、a（方程参数）、b（方程参数）、n（阶）、gx（基点x坐标）、gy（基点y坐标）。
    /// </summary>
    public static readonly string[] Sm2Param =
    {
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF", // p,0
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC", // a,1
        "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93", // b,2
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123", // n,3
        "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7", // gx,4
        "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" // gy,5
    };

    /// <summary>
    /// 当前使用的椭圆曲线参数数组。
    /// </summary>
    public string[] ecc_param = Sm2Param;

    /// <summary>
    /// 椭圆曲线的模数p。
    /// </summary>
    public readonly BigInteger ecc_p;

    /// <summary>
    /// 椭圆曲线方程的参数a。
    /// </summary>
    public readonly BigInteger ecc_a;

    /// <summary>
    /// 椭圆曲线方程的参数b。
    /// </summary>
    public readonly BigInteger ecc_b;

    /// <summary>
    /// 椭圆曲线的阶n。
    /// </summary>
    public readonly BigInteger ecc_n;

    /// <summary>
    /// 基点G的x坐标。
    /// </summary>
    public readonly BigInteger ecc_gx;

    /// <summary>
    /// 基点G的y坐标。
    /// </summary>
    public readonly BigInteger ecc_gy;

    /// <summary>
    /// 椭圆曲线对象。
    /// </summary>
    public readonly ECCurve ecc_curve;

    /// <summary>
    /// 椭圆曲线上的基点G。
    /// </summary>
    public readonly ECPoint ecc_point_g;

    /// <summary>
    /// 椭圆曲线域参数。
    /// </summary>
    public readonly ECDomainParameters ecc_bc_spec;

    /// <summary>
    /// 密钥对生成器。
    /// </summary>
    public readonly ECKeyPairGenerator ecc_key_pair_generator;

    /// <summary>
    /// 初始化SM2算法实例。
    /// 设置椭圆曲线参数并初始化密钥对生成器。
    /// </summary>
    private Sm2()
    {
        ecc_param = Sm2Param;

        ecc_p = new BigInteger(ecc_param[0], 16);
        ecc_a = new BigInteger(ecc_param[1], 16);
        ecc_b = new BigInteger(ecc_param[2], 16);
        ecc_n = new BigInteger(ecc_param[3], 16);
        ecc_gx = new BigInteger(ecc_param[4], 16);
        ecc_gy = new BigInteger(ecc_param[5], 16);

        ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, null, null);
        ecc_point_g = ecc_curve.CreatePoint(ecc_gx, ecc_gy);

        ecc_bc_spec = new ECDomainParameters(ecc_curve, ecc_point_g, ecc_n);

        var ecKeyGenerationParameters = new ECKeyGenerationParameters(ecc_bc_spec, new SecureRandom());

        ecc_key_pair_generator = new ECKeyPairGenerator();
        ecc_key_pair_generator.Init(ecKeyGenerationParameters);
    }

    /// <summary>
    /// 计算用户标识Z值。
    /// </summary>
    /// <param name="userId">用户标识字节数组。</param>
    /// <param name="userKey">用户公钥点。</param>
    /// <returns>返回计算得到的Z值字节数组。</returns>
    public byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
    {
        var sm3 = new Sm3Digest();
        byte[] p;
        // userId length
        int len = userId.Length * 8;
        sm3.Update((byte)(len >> 8 & 0x00ff));
        sm3.Update((byte)(len & 0x00ff));

        // userId
        sm3.BlockUpdate(userId, 0, userId.Length);

        // a,b
        p = ecc_a.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = ecc_b.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        // gx,gy
        p = ecc_gx.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = ecc_gy.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);

        // x,y
        p = userKey.AffineXCoord.ToBigInteger().ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = userKey.AffineYCoord.ToBigInteger().ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);

        // Z
        byte[] md = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(md, 0);

        return md;
    }
}