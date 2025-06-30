using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// DSA 数字签名算法工具类
/// 提供了DSA数字签名的创建、签名和验证功能
/// DSA算法专门用于数字签名，不能用于加密解密
/// </summary>
public sealed class DsaHelper
{
    private readonly DSACryptoServiceProvider _dsa;

    /// <summary>
    /// 使用现有的 DSACryptoServiceProvider 实例初始化 Dsa 类
    /// </summary>
    /// <param name="dsa">DSACryptoServiceProvider 实例，不能为null</param>
    /// <exception cref="ArgumentNullException">当dsa参数为null时抛出</exception>
    public DsaHelper(DSACryptoServiceProvider dsa)
    {
        _dsa = dsa ?? throw new ArgumentNullException(nameof(dsa));
    }

    /// <summary>
    /// 使用 XML 格式的密钥字符串初始化 Dsa 类
    /// </summary>
    /// <param name="key">XML 格式的密钥字符串，可以是公钥或私钥</param>
    /// <exception cref="ArgumentException">当key参数为null或空时抛出</exception>
    /// <exception cref="CryptographicException">当密钥格式无效时抛出</exception>
    public DsaHelper(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        var dsa = new DSACryptoServiceProvider();
        dsa.FromXmlString(key);
        _dsa = dsa;
    }

    /// <summary>
    /// 生成新的 DSA 密钥对，并以 XML 字符串形式返回
    /// </summary>
    /// <returns>包含私钥和公钥的字典，其中：
    /// - privatekey: 包含完整密钥信息的XML字符串
    /// - publickey: 仅包含公钥信息的XML字符串</returns>
    public static Dictionary<string, string> Make()
    {
        var dic = new Dictionary<string, string>();
        var dsa = new DSACryptoServiceProvider();
        dic["privatekey"] = dsa.ToXmlString(true);
        dic["publickey"] = dsa.ToXmlString(false);
        return dic;
    }

    /// <summary>
    /// 使用私钥对数据进行签名
    /// </summary>
    /// <param name="dataToSign">要签名的数据字节数组，不能为null</param>
    /// <param name="privateKey">XML 格式的私钥字符串，必须包含私钥信息</param>
    /// <returns>签名后的字节数组，如果签名过程出错则返回null</returns>
    /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
    public static byte[] SignData(byte[] dataToSign, string privateKey)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
            ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));
            var dsa = new DSACryptoServiceProvider();
            dsa.FromXmlString(privateKey);
            return dsa.SignData(dataToSign);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 使用私钥对字符串数据进行签名，并返回 Base64 编码的签名字符串
    /// </summary>
    /// <param name="dataToSign">要签名的字符串数据，不能为null或空</param>
    /// <param name="privateKey">XML 格式的私钥字符串，必须包含私钥信息</param>
    /// <returns>Base64 编码的签名字符串，如果签名过程出错则返回null</returns>
    /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
    public static string SignData(string dataToSign, string privateKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToSign, nameof(dataToSign));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign), privateKey);
        return res != null ? Convert.ToBase64String(res) : null;
    }

    /// <summary>
    /// 使用实例化的 DSACryptoServiceProvider 对数据进行签名
    /// </summary>
    /// <param name="dataToSign">要签名的数据字节数组，不能为null</param>
    /// <returns>签名后的字节数组，如果签名过程出错则返回null</returns>
    /// <exception cref="ArgumentNullException">当dataToSign为null时抛出</exception>
    public byte[] SignData(byte[] dataToSign)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
            return _dsa.SignData(dataToSign);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 使用实例化的 DSACryptoServiceProvider 对字符串数据进行签名，并返回 Base64 编码的签名字符串
    /// </summary>
    /// <param name="dataToSign">要签名的字符串数据，不能为null或空</param>
    /// <returns>Base64 编码的签名字符串，如果签名过程出错则返回null</returns>
    /// <exception cref="ArgumentNullException">当dataToSign为null或空时抛出</exception>
    public string SignData(string dataToSign)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToSign, nameof(dataToSign));
        var res = SignData(Encoding.UTF8.GetBytes(dataToSign));
        return res != null ? Convert.ToBase64String(res) : null;
    }

    /// <summary>
    /// 使用公钥验证数据的签名
    /// </summary>
    /// <param name="dataToVerify">要验证的数据字节数组，不能为null</param>
    /// <param name="signedData">签名后的字节数组，不能为null</param>
    /// <param name="publicKey">XML 格式的公钥字符串</param>
    /// <returns>如果签名有效，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当任意参数为null时抛出</exception>
    public static bool VerifyData(byte[] dataToVerify, byte[] signedData, string publicKey)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
            ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
            ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
            var dsa = new DSACryptoServiceProvider();
            dsa.FromXmlString(publicKey);
            return dsa.VerifyData(dataToVerify, signedData);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 使用公钥验证字符串数据的签名
    /// </summary>
    /// <param name="dataToVerify">要验证的字符串数据，不能为null或空</param>
    /// <param name="signedData">Base64 编码的签名字符串，不能为null或空</param>
    /// <param name="publicKey">XML 格式的公钥字符串</param>
    /// <returns>如果签名有效，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当任意参数为null或空时抛出</exception>
    public static bool VerifyData(string dataToVerify, string signedData, string publicKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData), publicKey);
    }

    /// <summary>
    /// 使用实例化的 DSACryptoServiceProvider 验证数据的签名
    /// </summary>
    /// <param name="dataToVerify">要验证的数据字节数组，不能为null</param>
    /// <param name="signedData">签名后的字节数组，不能为null</param>
    /// <returns>如果签名有效，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当任意参数为null时抛出</exception>
    public bool VerifyData(byte[] dataToVerify, byte[] signedData)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
            ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
            return _dsa.VerifyData(dataToVerify, signedData);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 使用实例化的 DSACryptoServiceProvider 验证字符串数据的签名
    /// </summary>
    /// <param name="dataToVerify">要验证的字符串数据，不能为null或空</param>
    /// <param name="signedData">Base64 编码的签名字符串，不能为null或空</param>
    /// <returns>如果签名有效，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当任意参数为null或空时抛出</exception>
    public bool VerifyData(string dataToVerify, string signedData)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(dataToVerify, nameof(dataToVerify));
            ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
            return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData));
        }
        catch
        {
            return false;
        }
    }
}