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

namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 雪花算法ID解析器，用于从已生成的ID中提取各组成部分。
/// </summary>
/// <remarks>
/// Snowflake ID parser for extracting components from a generated ID.
/// Bit layout: 1 sign bit | 41 timestamp bits | 5 datacenter bits | 5 worker bits | 12 sequence bits.
/// </remarks>
public static class SnowFlakeIdParser
{
    private const int WorkerIdBits = 5;
    private const int DatacenterIdBits = 5;
    private const int SequenceBits = 12;
    private const int WorkerIdShift = SequenceBits;
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);
    private const long WorkerIdMask = -1L ^ (-1L << WorkerIdBits);
    private const long DatacenterIdMask = -1L ^ (-1L << DatacenterIdBits);

    /// <summary>
    /// 使用默认BaseTime解析雪花ID。
    /// </summary>
    /// <remarks>
    /// Parses a Snowflake ID using the default BaseTime (2025-01-01 00:00:00 UTC).
    /// </remarks>
    /// <param name="id">要解析的雪花ID / The Snowflake ID to parse</param>
    /// <returns>解析结果 / The parsed result</returns>
    public static SnowFlakeIdInfo Parse(long id)
    {
        return Parse(id, IdWorker.DefaultBaseTime);
    }

    /// <summary>
    /// 使用指定的BaseTime解析雪花ID。
    /// </summary>
    /// <remarks>
    /// Parses a Snowflake ID using the specified BaseTime (in milliseconds since Unix epoch).
    /// </remarks>
    /// <param name="id">要解析的雪花ID / The Snowflake ID to parse</param>
    /// <param name="baseTimeMs">基准时间（毫秒，Unix纪元起）/ Base time in milliseconds since Unix epoch</param>
    /// <returns>解析结果 / The parsed result</returns>
    public static SnowFlakeIdInfo Parse(long id, long baseTimeMs)
    {
        var timestamp = (id >> TimestampLeftShift) + baseTimeMs;
        var dataCenterId = (id >> DatacenterIdShift) & DatacenterIdMask;
        var workerId = (id >> WorkerIdShift) & WorkerIdMask;
        var sequence = id & SequenceMask;

        var timestampOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

        return new SnowFlakeIdInfo(id, timestampOffset, workerId, dataCenterId, sequence);
    }
}
