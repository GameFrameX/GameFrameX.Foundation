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

using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

/// <summary>
/// TimerHelper nullable 重载的单元测试：
/// 验证 null 输入短路返回 null，非 null 输入与非空重载结果完全一致（覆盖 utc: true / utc: false）。
/// </summary>
[Collection("TimerHelper")]
public class TimerHelperNullableTests : IDisposable
{
    public TimerHelperNullableTests()
    {
        // 固定当前时区为 UTC，保证 utc:false 分支（走 CurrentTimeZone）结果确定。
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
    }

    public void Dispose()
    {
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        TimerHelper.ResetTimeOffset();
    }

    #region null 短路返回 null

    [Fact]
    public void DateTimeToSecond_Null_ShouldReturnNull()
    {
        Assert.Null(TimerHelper.DateTimeToSecond(null));
        Assert.Null(TimerHelper.DateTimeToSecond(null, true));
    }

    [Fact]
    public void DateTimeToMilliseconds_Null_ShouldReturnNull()
    {
        Assert.Null(TimerHelper.DateTimeToMilliseconds(null));
        Assert.Null(TimerHelper.DateTimeToMilliseconds(null, true));
    }

    [Fact]
    public void TimestampSecondToDateTime_Null_ShouldReturnNull()
    {
        Assert.Null(TimerHelper.TimestampSecondToDateTime(null));
        Assert.Null(TimerHelper.TimestampSecondToDateTime(null, true));
    }

    [Fact]
    public void TimeStampMillisecondToDateTime_Null_ShouldReturnNull()
    {
        Assert.Null(TimerHelper.TimeStampMillisecondToDateTime(null));
        Assert.Null(TimerHelper.TimeStampMillisecondToDateTime(null, true));
    }

    #endregion

    #region 非 null 与非空重载结果一致（utc 双分支）

    [Fact]
    public void DateTimeToSecond_NonNull_ShouldMatchNonNullOverload()
    {
        var time = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        Assert.Equal(TimerHelper.DateTimeToSecond(time, true), TimerHelper.DateTimeToSecond((DateTime?)time, true));
        Assert.Equal(TimerHelper.DateTimeToSecond(time, false), TimerHelper.DateTimeToSecond((DateTime?)time, false));
    }

    [Fact]
    public void DateTimeToMilliseconds_NonNull_ShouldMatchNonNullOverload()
    {
        var time = new DateTime(2024, 6, 15, 10, 30, 0, 123, DateTimeKind.Utc);

        Assert.Equal(TimerHelper.DateTimeToMilliseconds(time, true), TimerHelper.DateTimeToMilliseconds((DateTime?)time, true));
        Assert.Equal(TimerHelper.DateTimeToMilliseconds(time, false), TimerHelper.DateTimeToMilliseconds((DateTime?)time, false));
    }

    [Fact]
    public void TimestampSecondToDateTime_NonNull_ShouldMatchNonNullOverload()
    {
        const long timestamp = 1718447400L; // 2024-06-15 10:30:00 UTC

        Assert.Equal(TimerHelper.TimestampSecondToDateTime(timestamp, true), TimerHelper.TimestampSecondToDateTime((long?)timestamp, true));
        Assert.Equal(TimerHelper.TimestampSecondToDateTime(timestamp, false), TimerHelper.TimestampSecondToDateTime((long?)timestamp, false));
    }

    [Fact]
    public void TimeStampMillisecondToDateTime_NonNull_ShouldMatchNonNullOverload()
    {
        const long timestamp = 1718447400123L; // 2024-06-15 10:30:00.123 UTC

        Assert.Equal(TimerHelper.TimeStampMillisecondToDateTime(timestamp, true), TimerHelper.TimeStampMillisecondToDateTime((long?)timestamp, true));
        Assert.Equal(TimerHelper.TimeStampMillisecondToDateTime(timestamp, false), TimerHelper.TimeStampMillisecondToDateTime((long?)timestamp, false));
    }

    #endregion
}
