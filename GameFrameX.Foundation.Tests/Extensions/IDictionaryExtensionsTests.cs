using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

public class IDictionaryExtensionsTests
{
    [Fact]
    public void AddOrUpdate_IDictionary_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate("key", 1, (k, v) => v + 1));
    }
    
    [Fact]
    public void AddOrUpdate_IDictionary_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate(null, 1, (k, v) => v + 1));
    }
    
    [Fact]
    public void AddOrUpdate_IDictionary_ShouldThrowArgumentNullException_WhenUpdateValueFactoryIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate("key", 1, null));
    }
    
    [Fact]
    public void AddOrUpdate_IDictionary_ShouldAddNewKeyValue_WhenKeyDoesNotExist()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act
        var result = dictionary.AddOrUpdate("key", 5, (k, v) => v + 1);
        
        // Assert
        Assert.Equal(5, result);
        Assert.Equal(5, dictionary["key"]);
    }
    
    [Fact]
    public void AddOrUpdate_IDictionary_ShouldUpdateExistingValue_WhenKeyExists()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["key"] = 10 };
        
        // Act
        var result = dictionary.AddOrUpdate("key", 5, (k, v) => v + 1);
        
        // Assert
        Assert.Equal(11, result);
        Assert.Equal(11, dictionary["key"]);
    }
    
    [Fact]
    public void GetOrAdd_IDictionary_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd("key", () => 1));
    }
    
    [Fact]
    public void GetOrAdd_IDictionary_ShouldThrowArgumentNullException_WhenKeyIsNull()
     {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd(null, () => 1));
    }
    
    [Fact]
    public void GetOrAdd_IDictionary_ShouldReturnExistingValue_WhenKeyExists()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["key"] = 10 };
        
        // Act
        var result = dictionary.GetOrAdd("key", () => 5);
        
        // Assert
        Assert.Equal(10, result);
        Assert.Equal(10, dictionary["key"]);
    }
    
    [Fact]
    public void GetOrAdd_IDictionary_ShouldAddAndReturnNewValue_WhenKeyDoesNotExist()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act
        var result = dictionary.GetOrAdd("key", () => 5);
        
        // Assert
        Assert.Equal(5, result);
        Assert.Equal(5, dictionary["key"]);
    }
    
    [Fact]
    public void ForEach_IDictionary_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.ForEach((k, v) => { }));
    }
    
    [Fact]
    public void ForEach_IDictionary_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.ForEach(null));
    }
    
    [Fact]
    public void ForEach_IDictionary_ShouldExecuteActionForEachKeyValuePair()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>
        {
            ["key1"] = 1,
            ["key2"] = 2,
            ["key3"] = 3
        };
        var sum = 0;
        
        // Act
        dictionary.ForEach((k, v) => sum += v);
        
        // Assert
        Assert.Equal(6, sum);
    }
    
    [Fact]
    public void ToDictionarySafety_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        // Arrange
        IEnumerable<string> source = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToDictionarySafety(s => s.Length));
    }
    
    [Fact]
    public void ToDictionarySafety_ShouldThrowArgumentNullException_WhenKeySelectorIsNull()
    {
        // Arrange
        IEnumerable<string> source = new[] { "a", "bb", "ccc" };
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToDictionarySafety<string, int>(null));
    }
    
    [Fact]
    public void ToDictionarySafety_ShouldCreateDictionary_WhenValidInputProvided()
    {
        // Arrange
        var source = new[] { "a", "bb", "ccc" };
        
        // Act
        var result = source.ToDictionarySafety(s => s.Length);
        
        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("a", result[1]);
        Assert.Equal("bb", result[2]);
        Assert.Equal("ccc", result[3]);
    }
    
    [Fact]
    public void ToDictionarySafety_ShouldHandleDuplicateKeys_ByOverwriting()
    {
        // Arrange
        var source = new[] { "a", "b", "c" }; // All have same length
        
        // Act
        var result = source.ToDictionarySafety(s => s.Length);
        
        // Assert
        Assert.Single(result);
        Assert.Equal("c", result[1]); // Last value should win
    }
    
    [Fact]
    public void ToConcurrentDictionary_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        // Arrange
        IEnumerable<string> source = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToConcurrentDictionary(s => s.Length));
    }
    
    [Fact]
    public void ToConcurrentDictionary_ShouldThrowArgumentNullException_WhenKeySelectorIsNull()
    {
        // Arrange
        IEnumerable<string> source = new[] { "a", "bb", "ccc" };
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToConcurrentDictionary<string, int>(null));
    }
    
    [Fact]
    public void ToConcurrentDictionary_ShouldCreateConcurrentDictionary_WhenValidInputProvided()
    {
        // Arrange
        var source = new[] { "a", "bb", "ccc" };
        
        // Act
        var result = source.ToConcurrentDictionary(s => s.Length);
        
        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("a", result[1]);
        Assert.Equal("bb", result[2]);
        Assert.Equal("ccc", result[3]);
    }
    
    [Fact]
    public void ToLookupX_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        // Arrange
        IEnumerable<string> source = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToLookupX(s => s.Length));
    }
    
    [Fact]
    public void ToLookupX_ShouldThrowArgumentNullException_WhenKeySelectorIsNull()
    {
        // Arrange
        IEnumerable<string> source = new[] { "a", "bb", "ccc" };
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.ToLookupX<string, int>(null));
    }
    
    [Fact]
    public void ToLookupX_ShouldCreateLookup_WhenValidInputProvided()
    {
        // Arrange
        var source = new[] { "a", "b", "cc", "dd" };
        
        // Act
        var result = source.ToLookupX(s => s.Length);
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[1].Count());
        Assert.Equal(2, result[2].Count());
        Assert.Contains("a", result[1]);
        Assert.Contains("b", result[1]);
        Assert.Contains("cc", result[2]);
        Assert.Contains("dd", result[2]);
    }
    
    [Fact]
    public void AsConcurrentDictionary_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
    {
        // Arrange
        Dictionary<string, int> dictionary = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AsConcurrentDictionary());
    }
    
    [Fact]
    public void AsConcurrentDictionary_ShouldConvertToConcurrentDictionary_WhenValidInputProvided()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>
        {
            ["key1"] = 1,
            ["key2"] = 2
        };
        
        // Act
        var result = dictionary.AsConcurrentDictionary();
        
        // Assert
        Assert.IsType<NullableConcurrentDictionary<string, int>>(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result["key1"]);
        Assert.Equal(2, result["key2"]);
    }
    
    [Fact]
    public void AsDictionary_ShouldThrowArgumentNullException_WhenConcurrentDictionaryIsNull()
    {
        // Arrange
        ConcurrentDictionary<string, int> dictionary = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AsDictionary());
    }
    
    [Fact]
    public void AsDictionary_ShouldConvertToNullableDictionary_WhenValidInputProvided()
    {
        // Arrange
        var dictionary = new ConcurrentDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        
        // Act
        var result = dictionary.AsDictionary();
        
        // Assert
        Assert.IsType<NullableDictionary<string, int>>(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result["key1"]);
        Assert.Equal(2, result["key2"]);
    }
}