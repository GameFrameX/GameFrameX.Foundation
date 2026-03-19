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

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 目录扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for directory operations.
/// </remarks>
public static class DirectoryExtensions
{
    /// <summary>
    /// 根据路径创建目录，支持递归创建。
    /// </summary>
    /// <remarks>
    /// Creates a directory based on the path, supporting recursive creation.
    /// This method uses <see cref="Directory.CreateDirectory(string)"/> which automatically creates all necessary parent directories recursively.
    /// When isFile is true, it automatically gets the directory path where the file is located.
    /// Supports both relative and absolute paths.
    /// If the directory already exists, no operation is performed.
    /// </remarks>
    /// <param name="path">目录路径 / The directory path.</param>
    /// <param name="isFile">是否为文件路径，如果为 <c>true</c>，则创建文件所在的目录，默认为 <c>false</c> / Whether it is a file path, if <c>true</c>, creates the directory where the file is located, defaults to <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="path"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="path"/> is <c>null</c>.</exception>
    public static void CreateAsDirectory(this string path, bool isFile = false)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        if (isFile)
        {
            path = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
        }

        if (!Directory.Exists(path))
        {
            // Directory.CreateDirectory 会递归创建所有父目录
            Directory.CreateDirectory(path);
        }
    }
}