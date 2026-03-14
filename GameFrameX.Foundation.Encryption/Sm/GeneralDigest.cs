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