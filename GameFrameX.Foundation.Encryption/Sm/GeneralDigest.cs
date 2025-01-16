using Org.BouncyCastle.Crypto;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// 通用摘要算法的基类,实现了IDigest接口
/// </summary>
internal abstract class GeneralDigest : IDigest
{
    private const int ByteLength = 64;

    private readonly byte[] _xBuf;
    private int _xBufOff;

    private long _byteCount;

    /// <summary>
    /// 初始化GeneralDigest的新实例
    /// </summary>
    internal GeneralDigest()
    {
        _xBuf = new byte[4];
    }

    /// <summary>
    /// 使用现有的GeneralDigest实例初始化新实例
    /// </summary>
    /// <param name="t">用于复制的GeneralDigest实例</param>
    internal GeneralDigest(GeneralDigest t)
    {
        _xBuf = new byte[t._xBuf.Length];
        Array.Copy(t._xBuf, 0, _xBuf, 0, t._xBuf.Length);

        _xBufOff = t._xBufOff;
        _byteCount = t._byteCount;
    }

    /// <summary>
    /// 更新摘要计算,处理单个字节
    /// </summary>
    /// <param name="input">要处理的输入字节</param>
    public void Update(byte input)
    {
        _xBuf[_xBufOff++] = input;

        if (_xBufOff == _xBuf.Length)
        {
            ProcessWord(_xBuf, 0);
            _xBufOff = 0;
        }

        _byteCount++;
    }

    /// <summary>
    /// 更新摘要计算,处理字节数组
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <param name="inOff">输入数组的起始偏移量</param>
    /// <param name="length">要处理的字节长度</param>
    public void BlockUpdate(byte[] input, int inOff, int length)
    {
        //
        // fill the current word
        //
        while ((_xBufOff != 0) && (length > 0))
        {
            Update(input[inOff]);
            inOff++;
            length--;
        }

        //
        // process whole words.
        //
        while (length > _xBuf.Length)
        {
            ProcessWord(input, inOff);

            inOff += _xBuf.Length;
            length -= _xBuf.Length;
            _byteCount += _xBuf.Length;
        }

        //
        // load in the remainder.
        //
        while (length > 0)
        {
            Update(input[inOff]);

            inOff++;
            length--;
        }
    }

    /// <summary>
    /// 完成摘要计算
    /// </summary>
    public void Finish()
    {
        long bitLength = (_byteCount << 3);

        //
        // add the pad bytes.
        //
        Update(128);

        while (_xBufOff != 0)
        {
            Update(0);
        }

        ProcessLength(bitLength);
        ProcessBlock();
    }

    /// <summary>
    /// 重置摘要计算的状态
    /// </summary>
    public virtual void Reset()
    {
        _byteCount = 0;
        _xBufOff = 0;
        Array.Clear(_xBuf, 0, _xBuf.Length);
    }

    /// <summary>
    /// 获取摘要算法的字节长度
    /// </summary>
    /// <returns>返回摘要算法的字节长度</returns>
    public int GetByteLength()
    {
        return ByteLength;
    }

    /// <summary>
    /// 处理输入字节数组中的一个字
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <param name="inOff">输入数组的起始偏移量</param>
    internal abstract void ProcessWord(byte[] input, int inOff);

    /// <summary>
    /// 处理消息长度
    /// </summary>
    /// <param name="bitLength">消息的位长度</param>
    internal abstract void ProcessLength(long bitLength);

    /// <summary>
    /// 处理数据块
    /// </summary>
    internal abstract void ProcessBlock();

    /// <summary>
    /// 获取算法名称
    /// </summary>
    public abstract string AlgorithmName { get; }

    /// <summary>
    /// 获取摘要大小
    /// </summary>
    /// <returns>返回摘要大小(以字节为单位)</returns>
    public abstract int GetDigestSize();

    /// <summary>
    /// 使用ReadOnlySpan更新摘要计算
    /// </summary>
    /// <param name="input">输入数据的只读跨度</param>
    public abstract void BlockUpdate(ReadOnlySpan<byte> input);

    /// <summary>
    /// 完成摘要计算并将结果写入输出数组
    /// </summary>
    /// <param name="output">输出字节数组</param>
    /// <param name="outOff">输出数组的起始偏移量</param>
    /// <returns>写入的字节数</returns>
    public abstract int DoFinal(byte[] output, int outOff);

    /// <summary>
    /// 完成摘要计算并将结果写入输出跨度
    /// </summary>
    /// <param name="output">输出数据的跨度</param>
    /// <returns>写入的字节数</returns>
    public abstract int DoFinal(Span<byte> output);
}