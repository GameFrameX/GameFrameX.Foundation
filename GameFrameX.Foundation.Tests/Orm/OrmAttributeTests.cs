using GameFrameX.Foundation.Orm.Attribute;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm;

public class OrmAttributeTests
{
    [Fact]
    public void SoftDeleteAttribute_ShouldPreserveConfiguredFieldsAndDefaults()
    {
        var attribute = new SoftDeleteAttribute("IsDeleted", "DeletedTime", "DeletedBy");

        Assert.Equal("IsDeleted", attribute.DeletedField);
        Assert.Equal("DeletedTime", attribute.DeletedTimeField);
        Assert.Equal("DeletedBy", attribute.DeletedByField);
        Assert.Equal(true, attribute.DeletedValue);
        Assert.Equal(false, attribute.NotDeletedValue);
        Assert.True(attribute.AutoFilter);
    }

    [Fact]
    public void SoftDeleteAttribute_WithNullDeletedField_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => new SoftDeleteAttribute(null!));
    }
}