// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 Apache License 2.0 许可证分发，
//  This project is distributed under the Apache License 2.0,
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
//  CNB Repository:    https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 基于 GUID 的 <see cref="IIdempotencyKeyGenerator"/> 默认实现 / The GUID-based default implementation of <see cref="IIdempotencyKeyGenerator"/>.
/// </summary>
/// <remarks>
/// 每次调用生成新的 GUID 字符串；返回值格式不保证稳定，使用方不得解析其结构。
/// <para>
/// Generates a fresh GUID string on every call; the returned format is not guaranteed to be stable,
/// and consumers must not parse its structure.
/// </para>
/// </remarks>
public sealed class GuidIdempotencyKeyGenerator : IIdempotencyKeyGenerator
{
    /// <summary>
    /// <see cref="GuidIdempotencyKeyGenerator"/> 的全局单例实例 / The global singleton instance of <see cref="GuidIdempotencyKeyGenerator"/>.
    /// </summary>
    /// <remarks>
    /// 实现无状态且线程安全，可直接共享。
    /// <para>The implementation is stateless and thread-safe; the instance can be shared freely.</para>
    /// </remarks>
    public static readonly GuidIdempotencyKeyGenerator Instance = new GuidIdempotencyKeyGenerator();

    private GuidIdempotencyKeyGenerator()
    {
    }

    /// <inheritdoc />
    public string NewIdempotencyKey()
    {
        return Guid.NewGuid().ToString("N");
    }
}
