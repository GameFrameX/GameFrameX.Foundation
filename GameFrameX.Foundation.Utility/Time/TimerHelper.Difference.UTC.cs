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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Utility;

public partial class TimerHelper
{
    /// <summary>
    /// 计算指定Unix时间戳到当前时间经过了多少秒
    /// </summary>
    /// <param name="timestamp">Unix时间戳（秒）。应为UTC时间戳。</param>
    /// <returns>经过的秒数。如果timestamp在未来，返回负数。</returns>
    /// <remarks>
    /// 此方法直接使用Unix时间戳计算经过的秒数
    /// 使用 <see cref="UnixTimeSeconds"/> 获取当前UTC时间戳进行计算
    /// 计算效率高于DateTime转换方式
    /// 适用于Unix时间戳的剩余时间计算
    /// </remarks>
    public static long GetElapsedSeconds(long timestamp)
    {
        var currentTimestamp = UnixTimeSeconds();
        return currentTimestamp - timestamp;
    }

    /// <summary>
    /// 计算指定Unix时间戳到当前时间经过了多少毫秒
    /// </summary>
    /// <param name="timestampMs">Unix时间戳（毫秒）。应为UTC时间戳。</param>
    /// <returns>经过的毫秒数。如果timestampMs在未来，返回负数。</returns>
    /// <remarks>
    /// 此方法直接使用Unix毫秒时间戳计算经过的毫秒数
    /// 使用 <see cref="UnixTimeMilliseconds"/> 获取当前UTC时间戳进行计算
    /// 计算效率高于DateTime转换方式
    /// 适用于需要毫秒级精度的剩余时间计算
    /// </remarks>
    public static long GetElapsedMilliseconds(long timestampMs)
    {
        var currentTimestamp = UnixTimeMilliseconds();
        return currentTimestamp - timestampMs;
    }
}
