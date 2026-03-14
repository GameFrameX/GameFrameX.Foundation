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

using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// DSA 数字签名算法工具类。
/// 提供了 DSA 数字签名的创建、签名和验证功能。
/// DSA 算法专门用于数字签名，不能用于加密解密。
/// </summary>
/// <remarks>
/// C-07 修复：使用 <see cref="DSA.Create()"/> 代替已弃用的 <see cref="DSACryptoServiceProvider"/>，
/// 后者仅支持 1024-bit（FIPS 186-2 已弃用），且在非 Windows 平台不可用。
/// C-11 修复：实现 <see cref="IDisposable"/> 以正确释放非托管资源。
/// </remarks>
public sealed class DsaHelper : IDisposable
{
    private readonly DSA _dsa;
    private bool _disposed;

    /// <summary>
    /// 使用现有的 <see cref="DSA"/> 实例初始化。
    /// </summary>
    /// <param name="dsa"><see cref="DSA"/> 实例，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dsa"/> 为 null 时抛出。</exception>
    public DsaHelper(DSA dsa)
    {
        _dsa = dsa ?? throw new ArgumentNullException(nameof(dsa));
    }

    /// <summary>
    /// 使用 XML 格式的密钥字符串初始化。
    /// </summary>
    /// <param name="key">XML 格式的密钥字符串，可以是公钥或私钥。</param>
    /// <exception cref="ArgumentException">当 <paramref name="key"/> 为 null 或空时抛出。</exception>
    /// <exception cref="CryptographicException">当密钥格式无效时抛出。</exception>
    public DsaHelper(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        // C-07 修复：DSA.Create() 代替 DSACryptoServiceProvider
        var dsa = DSA.Create();
        try
        {
            dsa.FromXmlString(key);
        }
        catch
        {
            // C-11 修复：构造失败时立即释放
            dsa.Dispose();
            throw;
        }

        _dsa = dsa;
    }

    /// <summary>释放 DSA 非托管资源。</summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _dsa.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// 生成新的 DSA 密钥对，并以 XML 字符串形式返回。
    /// </summary>
    /// <returns>包含私钥（"privatekey"）和公钥（"publickey"）的字典。</returns>
    public static Dictionary<string, string> Make()
    {
        // C-07/C-11 修复：DSA.Create() + using
        // 注意：使用默认密钥大小以确保跨平台兼容性
        // macOS 的 DSA 实现不支持显式指定 2048 位密钥
        using var dsa = DSA.Create();
        return new Dictionary<string, string>
        {
            ["privatekey"] = dsa.ToXmlString(true),
            ["publickey"] = dsa.ToXmlString(false)
        };
    }

    /// <summary>
    /// 使用私钥对数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">要签名的数据字节数组，不能为 null。</param>
    /// <param name="privateKey">XML 格式的私钥字符串。</param>
    /// <returns>签名后的字节数组；如果私钥格式无效则返回 null。</returns>
    /// <exception cref="ArgumentNullException">当任意参数为 null 时抛出。</exception>
    public static byte[] SignData(byte[] dataToSign, string privateKey)
    {
        // W-11 修复：参数校验移出 try 块，不被 catch 吞掉
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));

        try
        {
            // C-07/C-11 修复：DSA.Create() + using
            using var dsa = DSA.Create();
            dsa.FromXmlString(privateKey);
            return dsa.SignData(dataToSign, HashAlgorithmName.SHA256);
        }
        catch (CryptographicException)
        {
            return null;
        }
        catch (XmlException)
        {
            return null;
        }
    }

    /// <summary>
    /// 使用私钥对字符串数据进行签名，并返回 Base64 编码的签名字符串。
    /// </summary>
    /// <param name="dataToSign">要签名的字符串数据，不能为 null 或空。</param>
    /// <param name="privateKey">XML 格式的私钥字符串。</param>
    /// <returns>Base64 编码的签名字符串。</returns>
    /// <exception cref="ArgumentNullException">当参数为 null 时抛出。</exception>
    /// <exception cref="CryptographicException">当私钥格式无效或签名失败时抛出。</exception>
    public static string SignData(string dataToSign, string privateKey)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign), privateKey);
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用实例化的 DSA 对数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">要签名的数据字节数组，不能为 null。</param>
    /// <returns>签名后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    /// <exception cref="CryptographicException">当签名失败时抛出。</exception>
    public byte[] SignData(byte[] dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        return _dsa.SignData(dataToSign, HashAlgorithmName.SHA256);
    }

    /// <summary>
    /// 使用实例化的 DSA 对字符串数据进行签名，并返回 Base64 编码的签名字符串。
    /// </summary>
    /// <param name="dataToSign">要签名的字符串数据，不能为 null 或空。</param>
    /// <returns>Base64 编码的签名字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    public string SignData(string dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        var res = SignData(Encoding.UTF8.GetBytes(dataToSign));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用公钥验证数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">要验证的数据字节数组，不能为 null。</param>
    /// <param name="signedData">签名后的字节数组，不能为 null。</param>
    /// <param name="publicKey">XML 格式的公钥字符串。</param>
    /// <returns>如果签名有效，返回 true；如果公钥格式无效或签名无效，返回 false。</returns>
    /// <exception cref="ArgumentNullException">当任意参数为 null 时抛出。</exception>
    public static bool VerifyData(byte[] dataToVerify, byte[] signedData, string publicKey)
    {
        // W-11 修复：参数校验移出 try 块
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));

        try
        {
            // C-07/C-11 修复：DSA.Create() + using
            using var dsa = DSA.Create();
            dsa.FromXmlString(publicKey);
            return dsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA256);
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch (XmlException)
        {
            return false;
        }
    }

    /// <summary>
    /// 使用公钥验证字符串数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">要验证的字符串数据，不能为 null 或空。</param>
    /// <param name="signedData">Base64 编码的签名字符串，不能为 null 或空。</param>
    /// <param name="publicKey">XML 格式的公钥字符串。</param>
    /// <returns>如果签名有效，返回 true；否则返回 false。</returns>
    public static bool VerifyData(string dataToVerify, string signedData, string publicKey)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData), publicKey);
    }

    /// <summary>
    /// 使用实例化的 DSA 验证数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">要验证的数据字节数组，不能为 null。</param>
    /// <param name="signedData">签名后的字节数组，不能为 null。</param>
    /// <returns>如果签名有效，返回 true；否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当任意参数为 null 时抛出。</exception>
    public bool VerifyData(byte[] dataToVerify, byte[] signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
        return _dsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA256);
    }

    /// <summary>
    /// 使用实例化的 DSA 验证字符串数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">要验证的字符串数据，不能为 null 或空。</param>
    /// <param name="signedData">Base64 编码的签名字符串，不能为 null 或空。</param>
    /// <returns>如果签名有效，返回 true；否则返回 false。</returns>
    public bool VerifyData(string dataToVerify, string signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData));
    }
}
