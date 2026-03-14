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

namespace GameFrameX.Foundation.Encryption.Localization;

/// <summary>
/// Encryption 模块本地化资源键常量定义
/// </summary>
/// <remarks>
/// 这个类定义了 Encryption 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量
/// throw new ArgumentException(
///     nameof(keySize),
///     LocalizationService.GetString(LocalizationKeys.Exceptions.InvalidKeySize, expectedSize, actualSize));
/// </code>
/// </example>
public static class LocalizationKeys
{
    /// <summary>
    /// 异常消息资源键
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// 密钥数组不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.KeyArrayCannotBeEmpty
        /// 用途: 当传入的密钥数组为null或空时使用
        /// </remarks>
        public const string KeyArrayCannotBeEmpty = "Encryption.Exceptions.KeyArrayCannotBeEmpty";

        /// <summary>
        /// 明文不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PlainTextCannotBeNullOrEmpty
        /// 用途: 当待加密的明文为null或空字符串时使用
        /// </remarks>
        public const string PlainTextCannotBeNullOrEmpty = "Encryption.Exceptions.PlainTextCannotBeNullOrEmpty";

        /// <summary>
        /// 加密密钥不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.EncryptionKeyCannotBeNullOrEmpty
        /// 用途: 当加密密钥为null或空字符串时使用
        /// </remarks>
        public const string EncryptionKeyCannotBeNullOrEmpty = "Encryption.Exceptions.EncryptionKeyCannotBeNullOrEmpty";

        /// <summary>
        /// 密文不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.CipherTextCannotBeNullOrEmpty
        /// 用途: 当待解密的密文为null或空字符串时使用
        /// </remarks>
        public const string CipherTextCannotBeNullOrEmpty = "Encryption.Exceptions.CipherTextCannotBeNullOrEmpty";

        /// <summary>
        /// 解密密钥不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.DecryptionKeyCannotBeNullOrEmpty
        /// 用途: 当解密密钥为null或空字符串时使用
        /// </remarks>
        public const string DecryptionKeyCannotBeNullOrEmpty = "Encryption.Exceptions.DecryptionKeyCannotBeNullOrEmpty";

        /// <summary>
        /// 密钥长度无效的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.InvalidKeySize
        /// 用途: 当密钥长度不符合算法要求时使用
        /// 参数: {0} - 实际长度, {1} - 期望长度
        /// </remarks>
        public const string InvalidKeySize = "Encryption.Exceptions.InvalidKeySize";

        /// <summary>
        /// 密钥字符串必须为32个字符的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.KeyMustBe32Characters
        /// 用途: SM4算法要求密钥为32个字符（16字节）
        /// </remarks>
        public const string KeyMustBe32Characters = "Encryption.Exceptions.KeyMustBe32Characters";

        /// <summary>
        /// 密钥字符串必须为16个字符的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.KeyMustBe16Characters
        /// 用途: SM4算法的IV要求为16个字符（8字节）
        /// </remarks>
        public const string KeyMustBe16Characters = "Encryption.Exceptions.KeyMustBe16Characters";

        /// <summary>
        /// 公钥不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PublicKeyCannotBeEmpty
        /// 用途: 当SM2公钥为null或空时使用
        /// </remarks>
        public const string PublicKeyCannotBeEmpty = "Encryption.Exceptions.PublicKeyCannotBeEmpty";

        /// <summary>
        /// 私钥不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PrivateKeyCannotBeEmpty
        /// 用途: 当SM2私钥为null或空时使用
        /// </remarks>
        public const string PrivateKeyCannotBeEmpty = "Encryption.Exceptions.PrivateKeyCannotBeEmpty";

        /// <summary>
        /// 公钥字符串不能为null的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PublicKeyStringCannotBeNull
        /// 用途: 当SM2公钥字符串为null时使用
        /// </remarks>
        public const string PublicKeyStringCannotBeNull = "Encryption.Exceptions.PublicKeyStringCannotBeNull";

        /// <summary>
        /// 公钥字符串不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PublicKeyStringCannotBeEmpty
        /// 用途: 当SM2公钥字符串为空时使用
        /// </remarks>
        public const string PublicKeyStringCannotBeEmpty = "Encryption.Exceptions.PublicKeyStringCannotBeEmpty";

        /// <summary>
        /// 私钥字符串不能为null的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PrivateKeyStringCannotBeNull
        /// 用途: 当SM2私钥字符串为null时使用
        /// </remarks>
        public const string PrivateKeyStringCannotBeNull = "Encryption.Exceptions.PrivateKeyStringCannotBeNull";

        /// <summary>
        /// 私钥字符串不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PrivateKeyStringCannotBeEmpty
        /// 用途: 当SM2私钥字符串为空时使用
        /// </remarks>
        public const string PrivateKeyStringCannotBeEmpty = "Encryption.Exceptions.PrivateKeyStringCannotBeEmpty";

        /// <summary>
        /// 公钥字节数组不能为null的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PublicKeyByteArrayCannotBeNull
        /// 用途: 当SM2公钥字节数组为null时使用
        /// </remarks>
        public const string PublicKeyByteArrayCannotBeNull = "Encryption.Exceptions.PublicKeyByteArrayCannotBeNull";

        /// <summary>
        /// 公钥字节数组不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PublicKeyByteArrayCannotBeEmpty
        /// 用途: 当SM2公钥字节数组为空时使用
        /// </remarks>
        public const string PublicKeyByteArrayCannotBeEmpty = "Encryption.Exceptions.PublicKeyByteArrayCannotBeEmpty";

        /// <summary>
        /// 私钥字节数组不能为null的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PrivateKeyByteArrayCannotBeNull
        /// 用途: 当SM2私钥字节数组为null时使用
        /// </remarks>
        public const string PrivateKeyByteArrayCannotBeNull = "Encryption.Exceptions.PrivateKeyByteArrayCannotBeNull";

        /// <summary>
        /// 私钥字节数组不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.PrivateKeyByteArrayCannotBeEmpty
        /// 用途: 当SM2私钥字节数组为空时使用
        /// </remarks>
        public const string PrivateKeyByteArrayCannotBeEmpty = "Encryption.Exceptions.PrivateKeyByteArrayCannotBeEmpty";

        /// <summary>
        /// IV必须为32个字符的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.IVMustBe32Characters
        /// 用途: SM4算法要求IV为32个十六进制字符
        /// </remarks>
        public const string IVMustBe32Characters = "Encryption.Exceptions.IVMustBe32Characters";

        /// <summary>
        /// IV必须为16个字符的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Exceptions.IVMustBe16Characters
        /// 用途: SM4算法要求IV为16个十六进制字符
        /// </remarks>
        public const string IVMustBe16Characters = "Encryption.Exceptions.IVMustBe16Characters";
    }

    /// <summary>
    /// 日志消息资源键
    /// </summary>
    public static class Logs
    {
        /// <summary>
        /// 加密操作完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Logs.EncryptionCompleted
        /// 用途: 记录加密操作成功完成
        /// 参数: {0} - 算法名称, {1} - 数据长度
        /// </remarks>
        public const string EncryptionCompleted = "Encryption.Logs.EncryptionCompleted";

        /// <summary>
        /// 解密操作完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Logs.DecryptionCompleted
        /// 用途: 记录解密操作成功完成
        /// 参数: {0} - 算法名称, {1} - 数据长度
        /// </remarks>
        public const string DecryptionCompleted = "Encryption.Logs.DecryptionCompleted";

        /// <summary>
        /// 密钥生成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Logs.KeyGenerated
        /// 用途: 记录密钥生成操作
        /// 参数: {0} - 算法名称, {1} - 密钥长度
        /// </remarks>
        public const string KeyGenerated = "Encryption.Logs.KeyGenerated";
    }

    /// <summary>
    /// 状态消息资源键
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// 加密算法初始化成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Status.AlgorithmInitialized
        /// 用途: 表示加密算法初始化完成
        /// 参数: {0} - 算法名称
        /// </remarks>
        public const string AlgorithmInitialized = "Encryption.Status.AlgorithmInitialized";

        /// <summary>
        /// 密钥验证成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Status.KeyValidated
        /// 用途: 表示密钥验证通过
        /// 参数: {0} - 密钥长度, {1} - 算法要求
        /// </remarks>
        public const string KeyValidated = "Encryption.Status.KeyValidated";
    }

    /// <summary>
    /// 验证消息资源键
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// 密钥格式验证失败的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Validation.InvalidKeyFormat
        /// 用途: 当密钥格式不符合要求时使用
        /// 参数: {0} - 期望的格式
        /// </remarks>
        public const string InvalidKeyFormat = "Encryption.Validation.InvalidKeyFormat";

        /// <summary>
        /// 数据长度验证失败的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Encryption.Validation.InvalidDataLength
        /// 用途: 当数据长度不符合算法要求时使用
        /// 参数: {0} - 实际长度, {1} - 期望长度
        /// </remarks>
        public const string InvalidDataLength = "Encryption.Validation.InvalidDataLength";
    }
}