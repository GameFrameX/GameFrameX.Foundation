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
// ==========================================================================================

namespace GameFrameX.Foundation.Json;

/// <summary>
/// JSON 帮助类 Try API 的错误分类。
/// </summary>
/// <remarks>
/// Error kind for JsonHelper Try APIs.
/// </remarks>
public enum JsonHelperErrorKind
{
    /// <summary>
    /// 未发生错误。
    /// </summary>
    None = 0,

    /// <summary>
    /// 输入不是有效 JSON 或参数无效。
    /// </summary>
    InvalidInput = 1,

    /// <summary>
    /// JSON 有效，但无法转换为目标 CLR 类型。
    /// </summary>
    TypeMismatch = 2,

    /// <summary>
    /// 序列化失败。
    /// </summary>
    Serialization = 3,

    /// <summary>
    /// 未分类错误。
    /// </summary>
    Unknown = 4,
}

/// <summary>
/// JSON 帮助类 Try API 返回的错误详情。
/// </summary>
/// <remarks>
/// Error details returned by JsonHelper Try APIs.
/// </remarks>
public sealed class JsonHelperError
{
    /// <summary>
    /// 初始化错误详情。
    /// </summary>
    /// <param name="kind">错误分类 / Error kind</param>
    /// <param name="exceptionType">异常类型 / Exception type</param>
    /// <param name="message">错误消息 / Error message</param>
    /// <param name="path">JSON 路径 / JSON path</param>
    /// <param name="lineNumber">行号 / Line number</param>
    /// <param name="bytePositionInLine">行内字节位置 / Byte position in line</param>
    public JsonHelperError(JsonHelperErrorKind kind, Type exceptionType, string message, string path = null, long? lineNumber = null, long? bytePositionInLine = null)
    {
        Kind = kind;
        ExceptionType = exceptionType ?? typeof(Exception);
        Message = message ?? string.Empty;
        Path = path;
        LineNumber = lineNumber;
        BytePositionInLine = bytePositionInLine;
    }

    /// <summary>
    /// 错误分类。
    /// </summary>
    public JsonHelperErrorKind Kind { get; }

    /// <summary>
    /// 原始异常类型。
    /// </summary>
    public Type ExceptionType { get; }

    /// <summary>
    /// 错误消息。
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// JSON 路径。
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// 行号。
    /// </summary>
    public long? LineNumber { get; }

    /// <summary>
    /// 行内字节位置。
    /// </summary>
    public long? BytePositionInLine { get; }
}
