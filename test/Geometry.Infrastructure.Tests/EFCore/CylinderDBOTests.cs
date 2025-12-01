using Geometry.Infrastructure.Persistence.EFCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderDBOTests
{
    [Fact]
    public void Id_ShouldBeSetAndRetrieved()
    {
        // Arrange
        var cylinderDBO = new CylinderDBO();
        var id = Guid.NewGuid();

        // Act
        cylinderDBO.Id = id;

        // Assert
        Assert.Equal(id, cylinderDBO.Id);
    }

    [Fact]
    public void Radius_ShouldBeSetAndRetrieved()
    {
        // Arrange
        var cylinderDBO = new CylinderDBO();
        var radius = 5;

        // Act
        cylinderDBO.Radius = radius;

        // Assert
        Assert.Equal(radius, cylinderDBO.Radius);
    }

    [Fact]
    public void Height_ShouldBeSetAndRetrieved()
    {
        // Arrange
        var cylinderDBO = new CylinderDBO();
        var height = 10;

        // Act
        cylinderDBO.Height = height;

        // Assert
        Assert.Equal(height, cylinderDBO.Height);
    }

    [Fact]
    public void Constructor_ShouldCreateInstanceWithDefaultValues()
    {
        // Arrange & Act
        var cylinderDBO = new CylinderDBO();

        // Assert
        Assert.NotNull(cylinderDBO);
        Assert.Equal(Guid.Empty, cylinderDBO.Id);
        Assert.Equal(0, cylinderDBO.Radius);
        Assert.Equal(0, cylinderDBO.Height);
    }

    [Fact]
    public void Properties_ShouldBeMutable()
    {
        // Arrange
        var cylinderDBO = new CylinderDBO
        {
            Id = Guid.NewGuid(),
            Radius = 10,
            Height = 20
        };

        // Act
        var newId = Guid.NewGuid();
        cylinderDBO.Id = newId;
        cylinderDBO.Radius = 30;
        cylinderDBO.Height = 40;

        // Assert
        Assert.Equal(newId, cylinderDBO.Id);
        Assert.Equal(30, cylinderDBO.Radius);
        Assert.Equal(40, cylinderDBO.Height);
    }

    [Fact]
    public void ObjectInitializer_ShouldSetAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 15;
        var height = 25;

        // Act
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = radius,
            Height = height
        };

        // Assert
        Assert.Equal(id, cylinderDBO.Id);
        Assert.Equal(radius, cylinderDBO.Radius);
        Assert.Equal(height, cylinderDBO.Height);
    }
}