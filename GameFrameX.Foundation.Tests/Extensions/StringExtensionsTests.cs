using System;
using System.Text;
using System.Text.RegularExpressions;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// StringExtensions 扩展类单元测试
/// </summary>
public class StringExtensionsTests
{
    #region ToUrlSafeBase64 Tests

    [Fact]
    public void ToUrlSafeBase64_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.ToUrlSafeBase64());
    }

    [Fact]
    public void ToUrlSafeBase64_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var result = value.ToUrlSafeBase64();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToUrlSafeBase64_StringWithSpecialChars_ShouldReplaceCorrectly()
    {
        // Arrange
        var value = "abc+def/ghi";

        // Act
        var result = value.ToUrlSafeBase64();

        // Assert
        Assert.Equal("abc-def_ghi", result);
    }

    [Fact]
    public void ToUrlSafeBase64_StringWithPadding_ShouldRemovePadding()
    {
        // Arrange
        var value = "YWJjMTIzNDU2Nzg5MA==";

        // Act
        var result = value.ToUrlSafeBase64();

        // Assert
        Assert.Equal("YWJjMTIzNDU2Nzg5MA", result);
    }

    #endregion

    #region FromUrlSafeBase64 Tests

    [Fact]
    public void FromUrlSafeBase64_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.FromUrlSafeBase64());
    }

    [Fact]
    public void FromUrlSafeBase64_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var result = value.FromUrlSafeBase64();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FromUrlSafeBase64_StringWithUrlSafeChars_ShouldReplaceCorrectly()
    {
        // Arrange
        var value = "abc-def_ghi";

        // Act
        var result = value.FromUrlSafeBase64();

        // Assert
        Assert.Equal("abc+def/ghi=", result);
    }

    [Fact]
    public void FromUrlSafeBase64_StringWithoutPadding_ShouldAddPadding()
    {
        // Arrange
        var value = "YWJjMTIzNDU2Nzg5MA";

        // Act
        var result = value.FromUrlSafeBase64();

        // Assert
        Assert.Equal("YWJjMTIzNDU2Nzg5MA==", result);
    }

    [Fact]
    public void FromUrlSafeBase64_RoundTripConversion_ShouldReturnOriginal()
    {
        // Arrange
        var original = "YWJjMTIzNDU2Nzg5MA==";

        // Act
        var urlSafe = original.ToUrlSafeBase64();
        var restored = urlSafe.FromUrlSafeBase64();

        // Assert
        Assert.Equal(original, restored);
    }

    #endregion

    #region RepeatChar Tests

    [Fact]
    public void RepeatChar_ZeroCount_ShouldReturnEmptyString()
    {
        // Arrange
        var c = 'a';

        // Act
        var result = c.RepeatChar(0);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RepeatChar_NegativeCount_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var c = 'a';

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => c.RepeatChar(-5));
    }

    [Fact]
    public void RepeatChar_PositiveCount_ShouldRepeatCharacter()
    {
        // Arrange
        var c = 'a';

        // Act
        var result = c.RepeatChar(5);

        // Assert
        Assert.Equal("aaaaa", result);
    }

    #endregion

    #region CenterAlignedText Tests

    [Fact]
    public void CenterAlignedText_NullText_ShouldThrowArgumentNullException()
    {
        // Arrange
        string text = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => text.CenterAlignedText(10));
    }

    [Fact]
    public void CenterAlignedText_NegativeWidth_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var text = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => text.CenterAlignedText(-1));
    }

    [Fact]
    public void CenterAlignedText_WidthLessThanTextLength_ShouldUseTextLength()
    {
        // Arrange
        var text = "Hello";

        // Act
        var result = text.CenterAlignedText(3);

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void CenterAlignedText_EvenWidthEvenTextLength_ShouldCenterProperly()
    {
        // Arrange
        var text = "ab";

        // Act
        var result = text.CenterAlignedText(6);

        // Assert
        Assert.Equal("  ab  ", result);
    }

    [Fact]
    public void CenterAlignedText_EvenWidthOddTextLength_ShouldCenterProperly()
    {
        // Arrange
        var text = "abc";

        // Act
        var result = text.CenterAlignedText(6);

        // Assert
        Assert.Equal(" abc  ", result);
    }

    #endregion

    #region RemoveSuffix Tests

    [Fact]
    public void RemoveSuffix_NullString_ShouldReturnNull()
    {
        // Arrange
        string self = null;

        // Act
        var result = self.RemoveSuffix('a');

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RemoveSuffix_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var self = string.Empty;

        // Act
        var result = self.RemoveSuffix('a');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveSuffix_StringEndsWithChar_ShouldRemoveChar()
    {
        // Arrange
        var self = "Hello!";

        // Act
        var result = self.RemoveSuffix('!');

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void RemoveSuffix_StringDoesNotEndWithChar_ShouldReturnOriginalString()
    {
        // Arrange
        var self = "Hello";

        // Act
        var result = self.RemoveSuffix('!');

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void RemoveSuffix_StringEndsWithString_ShouldRemoveString()
    {
        // Arrange
        var self = "Hello World";

        // Act
        var result = self.RemoveSuffix(" World");

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void RemoveSuffix_StringDoesNotEndWithString_ShouldReturnOriginalString()
    {
        // Arrange
        var self = "Hello World";

        // Act
        var result = self.RemoveSuffix(" World!");

        // Assert
        Assert.Equal("Hello World", result);
    }

    #endregion

    #region RemoveWhiteSpace Tests

    [Fact]
    public void RemoveWhiteSpace_NullString_ShouldReturnNull()
    {
        // Arrange
        string self = null;

        // Act
        var result = self.RemoveWhiteSpace();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RemoveWhiteSpace_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var self = string.Empty;

        // Act
        var result = self.RemoveWhiteSpace();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveWhiteSpace_StringWithWhitespace_ShouldRemoveAllWhitespace()
    {
        // Arrange
        var self = "Hello World\t\r\n";

        // Act
        var result = self.RemoveWhiteSpace();

        // Assert
        Assert.Equal("HelloWorld", result);
    }

    #endregion

    #region IsNullOrEmpty Tests

    [Fact]
    public void IsNullOrEmpty_NullString_ShouldReturnTrue()
    {
        // Arrange
        string str = null;

        // Act
        var result = str.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_EmptyString_ShouldReturnTrue()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var result = str.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyString_ShouldReturnFalse()
    {
        // Arrange
        var str = "Hello";

        // Act
        var result = str.IsNullOrEmpty();

        // Assert
        Assert.False(result);
    }

    #endregion

    #region SplitToIntArray Tests

    [Fact]
    public void SplitToIntArray_NullString_ShouldReturnEmptyArray()
    {
        // Arrange
        string str = null;

        // Act
        var result = str.SplitToIntArray();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SplitToIntArray_EmptyString_ShouldReturnEmptyArray()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var result = str.SplitToIntArray();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SplitToIntArray_ValidString_ShouldReturnCorrectArray()
    {
        // Arrange
        var str = "1+2+3";

        // Act
        var result = str.SplitToIntArray();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void SplitToIntArray_InvalidParts_ShouldReturnZerosForInvalidParts()
    {
        // Arrange
        var str = "1+a+3";

        // Act
        var result = str.SplitToIntArray();

        // Assert
        Assert.Equal(new[] { 1, 0, 3 }, result);
    }

    [Fact]
    public void SplitToIntArray_CustomSeparator_ShouldUseProvidedSeparator()
    {
        // Arrange
        var str = "1,2,3";

        // Act
        var result = str.SplitToIntArray(',');

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    #endregion

    #region SplitTo2IntArray Tests

    [Fact]
    public void SplitTo2IntArray_NullString_ShouldReturnEmptyArray()
    {
        // Arrange
        string str = null;

        // Act
        var result = str.SplitTo2IntArray();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SplitTo2IntArray_EmptyString_ShouldReturnEmptyArray()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var result = str.SplitTo2IntArray();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SplitTo2IntArray_ValidString_ShouldReturnCorrectArray()
    {
        // Arrange
        var str = "1+2;3+4;5+6";

        // Act
        var result = str.SplitTo2IntArray();

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(new[] { 1, 2 }, result[0]);
        Assert.Equal(new[] { 3, 4 }, result[1]);
        Assert.Equal(new[] { 5, 6 }, result[2]);
    }

    [Fact]
    public void SplitTo2IntArray_CustomSeparators_ShouldUseProvidedSeparators()
    {
        // Arrange
        var str = "1,2|3,4|5,6";

        // Act
        var result = str.SplitTo2IntArray('|', ',');

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(new[] { 1, 2 }, result[0]);
        Assert.Equal(new[] { 3, 4 }, result[1]);
        Assert.Equal(new[] { 5, 6 }, result[2]);
    }

    #endregion

    #region ConvertToSnakeCase Tests

    [Fact]
    public void ConvertToSnakeCase_NullString_ShouldReturnNull()
    {
        // Arrange
        string input = null;

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertToSnakeCase_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConvertToSnakeCase_CamelCase_ShouldConvertToSnakeCase()
    {
        // Arrange
        var input = "camelCaseString";

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Equal("camel_case_string", result);
    }

    [Fact]
    public void ConvertToSnakeCase_PascalCase_ShouldConvertToSnakeCase()
    {
        // Arrange
        var input = "PascalCaseString";

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Equal("pascal_case_string", result);
    }

    [Fact]
    public void ConvertToSnakeCase_WithNumbers_ShouldConvertCorrectly()
    {
        // Arrange
        var input = "User123Name";

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Equal("user123_name", result);
    }

    [Fact]
    public void ConvertToSnakeCase_WithLeadingUnderscore_ShouldPreserveUnderscore()
    {
        // Arrange
        var input = "_UserName";

        // Act
        var result = input.ConvertToSnakeCase();

        // Assert
        Assert.Equal("_user_name", result);
    }

    #endregion

    #region TrimZhCn Tests

    [Fact]
    public void TrimZhCn_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string self = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.TrimZhCn());
    }

    [Fact]
    public void TrimZhCn_StringWithChineseChars_ShouldRemoveChineseChars()
    {
        // Arrange
        var self = "Hello你好World世界";

        // Act
        var result = self.TrimZhCn();

        // Assert
        Assert.Equal("HelloWorld", result);
    }

    [Fact]
    public void TrimZhCn_StringWithoutChineseChars_ShouldReturnOriginalString()
    {
        // Arrange
        var self = "Hello World";

        // Act
        var result = self.TrimZhCn();

        // Assert
        Assert.Equal("Hello World", result);
    }

    #endregion

    #region EqualsFast Tests

    [Fact]
    public void EqualsFast_BothNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        string self = null;
        string target = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.EqualsFast(target));
    }

    [Fact]
    public void EqualsFast_SelfNullTargetNotNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        string self = null;
        var target = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.EqualsFast(target));
    }

    [Fact]
    public void EqualsFast_SelfNotNullTargetNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var self = "Hello";
        string target = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.EqualsFast(target));
    }

    [Fact]
    public void EqualsFast_DifferentLengths_ShouldReturnFalse()
    {
        // Arrange
        var self = "Hello";
        var target = "HelloWorld";

        // Act
        var result = self.EqualsFast(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EqualsFast_SameStrings_ShouldReturnTrue()
    {
        // Arrange
        var self = "Hello World";
        var target = "Hello World";

        // Act
        var result = self.EqualsFast(target);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EqualsFast_DifferentStrings_ShouldReturnFalse()
    {
        // Arrange
        var self = "Hello World";
        var target = "Hello world";

        // Act
        var result = self.EqualsFast(target);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region EndsWithFast Tests

    [Fact]
    public void EndsWithFast_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string self = null;
        var target = "World";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.EndsWithFast(target));
    }

    [Fact]
    public void EndsWithFast_StringEndsWithTarget_ShouldReturnTrue()
    {
        // Arrange
        var self = "Hello World";
        var target = "World";

        // Act
        var result = self.EndsWithFast(target);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EndsWithFast_StringDoesNotEndWithTarget_ShouldReturnFalse()
    {
        // Arrange
        var self = "Hello World";
        var target = "Hello";

        // Act
        var result = self.EndsWithFast(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWithFast_TargetLongerThanString_ShouldReturnFalse()
    {
        // Arrange
        var self = "World";
        var target = "Hello World";

        // Act
        var result = self.EndsWithFast(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWithFast_NullTarget_ShouldThrowArgumentNullException()
    {
        // Arrange
        var self = "Hello World";
        string target = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.EndsWithFast(target));
    }

    #endregion

    #region StartsWithFast Tests

    [Fact]
    public void StartsWithFast_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string self = null;
        var target = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.StartsWithFast(target));
    }

    [Fact]
    public void StartsWithFast_StringStartsWithTarget_ShouldReturnTrue()
    {
        // Arrange
        var self = "Hello World";
        var target = "Hello";

        // Act
        var result = self.StartsWithFast(target);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StartsWithFast_StringDoesNotStartWithTarget_ShouldReturnFalse()
    {
        // Arrange
        var self = "Hello World";
        var target = "World";

        // Act
        var result = self.StartsWithFast(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StartsWithFast_TargetLongerThanString_ShouldReturnFalse()
    {
        // Arrange
        var self = "Hello";
        var target = "Hello World";

        // Act
        var result = self.StartsWithFast(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StartsWithFast_NullTarget_ShouldThrowArgumentNullException()
    {
        // Arrange
        var self = "Hello World";
        string target = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.StartsWithFast(target));
    }

    #endregion

    #region CheckNotNullOrEmpty Tests

    [Fact]
    public void CheckNotNullOrEmpty_ValidString_ShouldNotThrow()
    {
        // Arrange
        var value = "Hello";
        var name = "testParam";

        // Act & Assert
        var exception = Record.Exception(() => value.CheckNotNullOrEmpty(name));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckNotNullOrEmpty_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = null;
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.CheckNotNullOrEmpty(name));
        Assert.Equal(name, exception.ParamName);
    }

    [Fact]
    public void CheckNotNullOrEmpty_EmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var value = string.Empty;
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => value.CheckNotNullOrEmpty(name));
        Assert.Equal(name, exception.ParamName);
    }

    [Fact]
    public void CheckNotNullOrEmpty_WhitespaceString_ShouldNotThrow()
    {
        // Arrange
        var value = "   ";
        var name = "testParam";

        // Act & Assert
        var exception = Record.Exception(() => value.CheckNotNullOrEmpty(name));
        Assert.Null(exception);
    }

    #endregion

    #region CheckNotNullOrEmptyOrWhiteSpace Tests

    [Fact]
    public void CheckNotNullOrEmptyOrWhiteSpace_ValidString_ShouldNotThrow()
    {
        // Arrange
        var value = "Hello";
        var name = "testParam";

        // Act & Assert
        var exception = Record.Exception(() => value.CheckNotNullOrEmptyOrWhiteSpace(name));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckNotNullOrEmptyOrWhiteSpace_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = null;
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => value.CheckNotNullOrEmptyOrWhiteSpace(name));
        Assert.Equal(name, exception.ParamName);
    }

    [Fact]
    public void CheckNotNullOrEmptyOrWhiteSpace_EmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var value = string.Empty;
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => value.CheckNotNullOrEmptyOrWhiteSpace(name));
        Assert.Equal(name, exception.ParamName);
    }

    [Fact]
    public void CheckNotNullOrEmptyOrWhiteSpace_WhitespaceString_ShouldThrowArgumentException()
    {
        // Arrange
        var value = "   ";
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => value.CheckNotNullOrEmptyOrWhiteSpace(name));
        Assert.Equal(name, exception.ParamName);
    }

    [Fact]
    public void CheckNotNullOrEmptyOrWhiteSpace_TabAndNewlineString_ShouldThrowArgumentException()
    {
        // Arrange
        var value = "\t\r\n";
        var name = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => value.CheckNotNullOrEmptyOrWhiteSpace(name));
        Assert.Equal(name, exception.ParamName);
    }

    #endregion

    #region CreateAsDirectory Tests

    [Fact]
    public void CreateAsDirectory_NullPath_ShouldThrowArgumentNullException()
    {
        // Arrange
        string path = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => path.CreateAsDirectory());
    }

    #endregion
}