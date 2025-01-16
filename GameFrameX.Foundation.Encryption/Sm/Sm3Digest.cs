namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM3密码杂凑算法的实现类
/// </summary>
internal sealed class Sm3Digest : GeneralDigest
{
    /// <summary>
    /// 获取算法名称
    /// </summary>
    public override string AlgorithmName
    {
        get { return "SM3"; }
    }

    /// <summary>
    /// 获取摘要大小
    /// </summary>
    /// <returns>返回摘要大小(以字节为单位)</returns>
    public override int GetDigestSize()
    {
        return DigestLength;
    }

    private const int DigestLength = 32;

    private static readonly int[] v0 = new int[] { 0x7380166f, 0x4914b2b9, 0x172442d7, unchecked((int)0xda8a0600), unchecked((int)0xa96f30bc), 0x163138aa, unchecked((int)0xe38dee4d), unchecked((int)0xb0fb0e4e) };

    private readonly int[] v = new int[8];
    private readonly int[] v_ = new int[8];

    private static readonly int[] X0 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private readonly int[] X = new int[68];
    private int xOff;

    private const int T_00_15 = 0x79cc4519;
    private const int T_16_63 = 0x7a879d8a;

    /// <summary>
    /// 初始化SM3Digest的新实例
    /// </summary>
    public Sm3Digest()
    {
        Reset();
    }

    /// <summary>
    /// 使用现有的SM3Digest实例初始化新实例
    /// </summary>
    /// <param name="t">用于复制的SM3Digest实例</param>
    public Sm3Digest(Sm3Digest t) : base(t)
    {
        Array.Copy(t.X, 0, X, 0, t.X.Length);
        xOff = t.xOff;

        Array.Copy(t.v, 0, v, 0, t.v.Length);
    }

    /// <summary>
    /// 重置摘要计算的状态
    /// </summary>
    public override void Reset()
    {
        base.Reset();

        Array.Copy(v0, 0, v, 0, v0.Length);

        xOff = 0;
        Array.Copy(X0, 0, X, 0, X0.Length);
    }

    /// <summary>
    /// 处理数据块
    /// </summary>
    internal override void ProcessBlock()
    {
        int i;

        int[] ww = X;
        int[] ww_ = new int[64];

        for (i = 16; i < 68; i++)
        {
            ww[i] = P1(ww[i - 16] ^ ww[i - 9] ^ (ROTATE(ww[i - 3], 15))) ^ (ROTATE(ww[i - 13], 7)) ^ ww[i - 6];
        }

        for (i = 0; i < 64; i++)
        {
            ww_[i] = ww[i] ^ ww[i + 4];
        }

        int[] vv = v;
        int[] vv_ = v_;

        Array.Copy(vv, 0, vv_, 0, v0.Length);

        int SS1, SS2, TT1, TT2, aaa;
        for (i = 0; i < 16; i++)
        {
            aaa = ROTATE(vv_[0], 12);
            SS1 = aaa + vv_[4] + ROTATE(T_00_15, i);
            SS1 = ROTATE(SS1, 7);
            SS2 = SS1 ^ aaa;

            TT1 = FF_00_15(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
            TT2 = GG_00_15(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
            vv_[3] = vv_[2];
            vv_[2] = ROTATE(vv_[1], 9);
            vv_[1] = vv_[0];
            vv_[0] = TT1;
            vv_[7] = vv_[6];
            vv_[6] = ROTATE(vv_[5], 19);
            vv_[5] = vv_[4];
            vv_[4] = P0(TT2);
        }

        for (i = 16; i < 64; i++)
        {
            aaa = ROTATE(vv_[0], 12);
            SS1 = aaa + vv_[4] + ROTATE(T_16_63, i);
            SS1 = ROTATE(SS1, 7);
            SS2 = SS1 ^ aaa;

            TT1 = FF_16_63(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
            TT2 = GG_16_63(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
            vv_[3] = vv_[2];
            vv_[2] = ROTATE(vv_[1], 9);
            vv_[1] = vv_[0];
            vv_[0] = TT1;
            vv_[7] = vv_[6];
            vv_[6] = ROTATE(vv_[5], 19);
            vv_[5] = vv_[4];
            vv_[4] = P0(TT2);
        }

        for (i = 0; i < 8; i++)
        {
            vv[i] ^= vv_[i];
        }

        // Reset
        xOff = 0;
        Array.Copy(X0, 0, X, 0, X0.Length);
    }

    /// <summary>
    /// 处理输入字节数组中的一个字
    /// </summary>
    /// <param name="in_Renamed">输入字节数组</param>
    /// <param name="inOff">输入数组的起始偏移量</param>
    internal override void ProcessWord(byte[] in_Renamed, int inOff)
    {
        int n = in_Renamed[inOff] << 24;
        n |= (in_Renamed[++inOff] & 0xff) << 16;
        n |= (in_Renamed[++inOff] & 0xff) << 8;
        n |= (in_Renamed[++inOff] & 0xff);
        X[xOff] = n;

        if (++xOff == 16)
        {
            ProcessBlock();
        }
    }

    /// <summary>
    /// 处理消息长度
    /// </summary>
    /// <param name="bitLength">消息的位长度</param>
    internal override void ProcessLength(long bitLength)
    {
        if (xOff > 14)
        {
            ProcessBlock();
        }

        X[14] = (int)(SupportClass.URShift(bitLength, 32));
        X[15] = (int)(bitLength & unchecked((int)0xffffffff));
    }

    /// <summary>
    /// 将整数转换为大端字节序
    /// </summary>
    /// <param name="n">要转换的整数</param>
    /// <param name="bs">输出字节数组</param>
    /// <param name="off">输出数组的起始偏移量</param>
    public static void IntToBigEndian(int n, byte[] bs, int off)
    {
        bs[off] = (byte)(SupportClass.UrShift(n, 24));
        bs[++off] = (byte)(SupportClass.UrShift(n, 16));
        bs[++off] = (byte)(SupportClass.UrShift(n, 8));
        bs[++off] = (byte)(n);
    }

    /// <summary>
    /// 完成摘要计算并将结果写入输出数组
    /// </summary>
    /// <param name="out_Renamed">输出字节数组</param>
    /// <param name="outOff">输出数组的起始偏移量</param>
    /// <returns>写入的字节数</returns>
    public override int DoFinal(byte[] out_Renamed, int outOff)
    {
        Finish();

        for (int i = 0; i < 8; i++)
        {
            IntToBigEndian(v[i], out_Renamed, outOff + i * 4);
        }

        Reset();

        return DigestLength;
    }

    /// <summary>
    /// 循环左移运算
    /// </summary>
    /// <param name="x">要移位的值</param>
    /// <param name="n">移位的位数</param>
    /// <returns>移位后的结果</returns>
    private static int ROTATE(int x, int n)
    {
        return (x << n) | (SupportClass.UrShift(x, (32 - n)));
    }

    /// <summary>
    /// P0置换函数
    /// </summary>
    /// <param name="X">输入值</param>
    /// <returns>置换后的结果</returns>
    private static int P0(int X)
    {
        return ((X) ^ ROTATE((X), 9) ^ ROTATE((X), 17));
    }

    /// <summary>
    /// P1置换函数
    /// </summary>
    /// <param name="X">输入值</param>
    /// <returns>置换后的结果</returns>
    private static int P1(int X)
    {
        return ((X) ^ ROTATE((X), 15) ^ ROTATE((X), 23));
    }

    /// <summary>
    /// FF函数(0-15轮)
    /// </summary>
    private static int FF_00_15(int X, int Y, int Z)
    {
        return (X ^ Y ^ Z);
    }

    /// <summary>
    /// FF函数(16-63轮)
    /// </summary>
    private static int FF_16_63(int X, int Y, int Z)
    {
        return ((X & Y) | (X & Z) | (Y & Z));
    }

    /// <summary>
    /// GG函数(0-15轮)
    /// </summary>
    private static int GG_00_15(int X, int Y, int Z)
    {
        return (X ^ Y ^ Z);
    }

    /// <summary>
    /// GG函数(16-63轮)
    /// </summary>
    private static int GG_16_63(int X, int Y, int Z)
    {
        return ((X & Y) | (~X & Z));
    }

    /// <summary>
    /// 使用ReadOnlySpan更新摘要计算
    /// </summary>
    /// <param name="input">输入数据的只读跨度</param>
    public override void BlockUpdate(ReadOnlySpan<byte> input)
    {
    }

    /// <summary>
    /// 完成摘要计算并将结果写入输出跨度
    /// </summary>
    /// <param name="output">输出数据的跨度</param>
    /// <returns>写入的字节数</returns>
    public override int DoFinal(Span<byte> output)
    {
        return DigestLength;
    }

    //[STAThread]
    //public static void  Main()
    //{
    //    byte[] md = new byte[32];
    //    byte[] msg1 = Encoding.Default.GetBytes("ererfeiisgod");
    //    SM3Digest sm3 = new SM3Digest();
    //    sm3.BlockUpdate(msg1, 0, msg1.Length);
    //    sm3.DoFinal(md, 0);
    //    System.String s = new UTF8Encoding().GetString(Hex.Encode(md));
    //    System.Console.Out.WriteLine(s.ToUpper());

    //    Console.ReadLine();
    //}
}