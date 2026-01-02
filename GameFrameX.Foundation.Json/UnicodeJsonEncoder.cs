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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Text;
using System.Text.Encodings.Web;

namespace GameFrameX.Foundation.Json;

internal sealed class UnicodeJsonEncoder : JavaScriptEncoder
{
    internal static readonly UnicodeJsonEncoder Singleton = new UnicodeJsonEncoder();

    private readonly bool _preferHexEscape;
    private readonly bool _preferUppercase;

    private UnicodeJsonEncoder() : this(preferHexEscape: false, preferUppercase: false)
    {
    }

    private UnicodeJsonEncoder(bool preferHexEscape, bool preferUppercase)
    {
        _preferHexEscape = preferHexEscape;
        _preferUppercase = preferUppercase;
    }

    public override int MaxOutputCharactersPerInputCharacter
    {
        get
        {
            return 6;
            // "\uXXXX" for a single char ("\uXXXX\uYYYY" [12 chars] for supplementary scalar value)
        }
    }

    public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
    {
        for (int index = 0; index < textLength; ++index)
        {
            char value = text[index];

            if (NeedsEncoding(value))
            {
                return index;
            }
        }

        return -1;
    }

    public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
    {
        bool encode = WillEncode(unicodeScalar);

        if (!encode)
        {
            Span<char> span = new Span<char>(buffer, bufferLength);
            bool succeeded = new Rune(unicodeScalar).TryEncodeToUtf16(span, out var spanWritten);
            numberOfCharactersWritten = spanWritten;
            return succeeded;
        }

        if (!_preferHexEscape && unicodeScalar <= char.MaxValue && HasTwoCharacterEscape((char)unicodeScalar))
        {
            if (bufferLength < 2)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            buffer[0] = '\\';
            buffer[1] = GetTwoCharacterEscapeSuffix((char)unicodeScalar);
            numberOfCharactersWritten = 2;
            return true;
        }
        else
        {
            if (bufferLength < 6)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            buffer[0] = '\\';
            buffer[1] = 'u';
            buffer[2] = '0';
            buffer[3] = '0';
            buffer[4] = ToHexDigit((unicodeScalar & 0xf0) >> 4, _preferUppercase);
            buffer[5] = ToHexDigit(unicodeScalar & 0xf, _preferUppercase);
            numberOfCharactersWritten = 6;
            return true;
        }
    }

    public override bool WillEncode(int unicodeScalar)
    {
        if (unicodeScalar > char.MaxValue)
        {
            return false;
        }

        return NeedsEncoding((char)unicodeScalar);
    }

    // https://datatracker.ietf.org/doc/html/rfc8259#section-7
    private static bool NeedsEncoding(char value)
    {
        if (value == '"' || value == '\\')
        {
            return true;
        }

        return value <= '\u001f';
    }

    private static bool HasTwoCharacterEscape(char value)
    {
        // RFC 8259, Section 7, "char = " BNF
        switch (value)
        {
            case '"':
            case '\\':
            case '/':
            case '\b':
            case '\f':
            case '\n':
            case '\r':
            case '\t':
                return true;
            default:
                return false;
        }
    }

    private static char GetTwoCharacterEscapeSuffix(char value)
    {
        // RFC 8259, Section 7, "char = " BNF
        switch (value)
        {
            case '"':
                return '"';
            case '\\':
                return '\\';
            case '/':
                return '/';
            case '\b':
                return 'b';
            case '\f':
                return 'f';
            case '\n':
                return 'n';
            case '\r':
                return 'r';
            case '\t':
                return 't';
            default:
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    private static char ToHexDigit(int value, bool uppercase)
    {
        if (value > 0xf)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        if (value < 10)
        {
            return (char)(value + '0');
        }
        else
        {
            return (char)(value - 0xa + (uppercase ? 'A' : 'a'));
        }
    }
}