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

using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM3工具类，提供SM3密码杂凑算法的实现。
/// 该类用于计算数据的SM3摘要值，可用于数据完整性校验和数字签名。
/// </summary>
internal static class SM3Util
{
    /// <summary>
    /// 使用SM3算法计算输入数据的摘要值。
    /// </summary>
    /// <param name="data">要计算摘要的输入字符串。</param>
    /// <returns>返回十六进制格式的SM3摘要值字符串。</returns>
    /// <remarks>
    /// I-01 修复：原方法名 Encrypt 语义错误（SM3 是哈希算法，不是加密算法），重命名为 Hash。
    /// </remarks>
    public static string Hash(string data)
    {
        // W-03 修复：使用 UTF8 代替 Encoding.Default，确保跨平台哈希结果一致
        byte[] msg1 = Encoding.UTF8.GetBytes(data);

        var sm3 = new Sm3Digest();
        sm3.BlockUpdate(msg1, 0, msg1.Length);
        byte[] result = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(result, 0);
        return Encoding.ASCII.GetString(Hex.Encode(result));
    }
}