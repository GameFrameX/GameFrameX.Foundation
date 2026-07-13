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

namespace GameFrameX.Foundation.Hash
{
    /// <summary>
    /// 指定密码哈希（密钥派生函数 KDF）的算法种类。
    /// 该枚举仅用于密码哈希场景，与用于校验和的 <see cref="HashAlgorithmKind"/> 语义不同，不应混用。
    /// </summary>
    /// <remarks>
    /// Specifies the password-hashing (key derivation function / KDF) algorithm kind.
    /// This enum is intended only for password hashing and is semantically distinct from
    /// <see cref="HashAlgorithmKind"/> which covers fast checksum hashes; do not mix them.
    /// </remarks>
    public enum PasswordHashAlgorithmKind
    {
        /// <summary>
        /// PBKDF2-HMAC-SHA256。基于 BCL 内置 <c>Rfc2898DeriveBytes</c> 实现，无额外依赖。兼容性与可移植性最好。
        /// </summary>
        /// <remarks>PBKDF2-HMAC-SHA256, backed by the BCL Rfc2898DeriveBytes with no extra dependency.</remarks>
        Pbkdf2,

        /// <summary>
        /// bcrypt。基于 <c>BCrypt.Net-Next.StrongName</c>，密码长度硬限制 72 字节（UTF-8）。
        /// </summary>
        /// <remarks>bcrypt via BCrypt.Net-Next.StrongName; password is hard-limited to 72 UTF-8 bytes.</remarks>
        Bcrypt,

        /// <summary>
        /// scrypt。基于 BouncyCastle，具备内存硬度，抗 GPU/ASIC。
        /// </summary>
        /// <remarks>scrypt via BouncyCastle, memory-hard to resist GPU/ASIC bruteforce.</remarks>
        Scrypt,

        /// <summary>
        /// Argon2id。基于 BouncyCastle，2015 PHC 竞赛冠军，OWASP 首选推荐。
        /// </summary>
        /// <remarks>Argon2id via BouncyCastle, 2015 PHC winner and OWASP first-choice recommendation.</remarks>
        Argon2id,
    }
}
