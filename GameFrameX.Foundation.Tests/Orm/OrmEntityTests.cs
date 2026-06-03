using System.ComponentModel.DataAnnotations;
using GameFrameX.Foundation.Orm.Entity;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm;

public class OrmEntityTests
{
    [Fact]
    public void EntityBase_ShouldExposeDocumentedDefaultStates()
    {
        var entity = new TestEntity();

        Assert.Equal(0, entity.Id);
        Assert.False(entity.IsDeleted);
        Assert.Equal(0, entity.Version);
        Assert.Null(entity.IsEnabled);
    }

    [Fact]
    public void EntityBaseId_ShouldExposeKeyAndRequiredMetadata()
    {
        var property = typeof(EntityBaseId).GetProperty(nameof(EntityBaseId.Id));

        Assert.NotNull(property);
        Assert.Contains(property!.GetCustomAttributes(false), attribute => attribute is KeyAttribute);
        Assert.Contains(property.GetCustomAttributes(false), attribute => attribute is RequiredAttribute);
    }

    private sealed class TestEntity : EntityBase
    {
    }
}