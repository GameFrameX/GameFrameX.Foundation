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

using System;

namespace GameFrameX.Foundation.Orm.Entity.Localization
{
    /// <summary>
    /// Orm.Entity 模块本地化资源键常量定义。
    /// 这个类定义了 Orm.Entity 模块中所有可本地化字符串的键常量。
    /// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
    /// </summary>
    /// <remarks>
    /// Orm.Entity module localization resource key constant definitions.
    /// This class defines key constants for all localizable strings in the Orm.Entity module.
    /// Using constants avoids string hardcoding and improves code maintainability and type safety.
    /// </remarks>
    /// <example>
    /// <code>
    /// // 在代码中使用本地化键常量
    /// // Use localization key constants in code
    /// throw new InvalidOperationException(
    ///     LocalizationService.GetString(LocalizationKeys.Exceptions.RowVersionConflict, expectedRowVersion, entity.RowVersion));
    /// </code>
    /// </example>
    public static class LocalizationKeys
    {
        /// <summary>
        /// 异常消息资源键。
        /// </summary>
        /// <remarks>
        /// Exception message resource keys.
        /// </remarks>
        public static class Exceptions
        {
            /// <summary>
            /// 行版本冲突的错误消息。
            /// </summary>
            /// <remarks>
            /// Error message for row version conflict.
            /// 键名: Orm.Entity.Exceptions.RowVersionConflict
            /// 用途: 当实体的当前行版本号与期望行版本号不一致时使用
            /// Usage: Used when the entity's current row version does not match the expected row version.
            /// 参数: {0} - 期望行版本, {1} - 实际行版本
            /// Parameters: {0} - expected row version, {1} - actual row version.
            /// </remarks>
            public const string RowVersionConflict = "Orm.Entity.Exceptions.RowVersionConflict";
        }
    }
}
