using Geometry.Domain.CylinderModel;
using Geometry.Infrastructure.Persistence.EFCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderMapperTests
{
    [Fact]
    public void ToDBO_WithValidCylinder_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 5;
        var height = 10;
        var cylinder = new Cylinder(id, radius, height);

        // Act
        var cylinderDBO = CylinderMapper.ToDBO(cylinder);

        // Assert
        Assert.NotNull(cylinderDBO);
        Assert.Equal(id, cylinderDBO.Id);
        Assert.Equal(radius, cylinderDBO.Radius);
        Assert.Equal(height, cylinderDBO.Height);
    }

    [Fact]
    public void ToDBO_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        Cylinder cylinder = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CylinderMapper.ToDBO(cylinder));
    }

    [Fact]
    public void ToDomain_WithValidCylinderDBO_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 10;
        var height = 20;
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = radius,
            Height = height
        };

        // Act
        var cylinder = CylinderMapper.ToDomain(cylinderDBO);

        // Assert
        Assert.NotNull(cylinder);
        Assert.Equal(id, cylinder.Id);
        Assert.Equal(radius, cylinder.Radius);
        Assert.Equal(height, cylinder.Height);
    }

    [Fact]
    public void ToDomain_WithNullCylinderDBO_ShouldThrowArgumentNullException()
    {
        // Arrange
        CylinderDBO cylinderDBO = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CylinderMapper.ToDomain(cylinderDBO));
    }

    [Fact]
    public void ToDBO_ThenToDomain_ShouldRoundTripCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 7;
        var height = 14;
        var originalCylinder = new Cylinder(id, radius, height);

        // Act
        var cylinderDBO = CylinderMapper.ToDBO(originalCylinder);
        var roundTrippedCylinder = CylinderMapper.ToDomain(cylinderDBO);

        // Assert
        Assert.Equal(originalCylinder.Id, roundTrippedCylinder.Id);
        Assert.Equal(originalCylinder.Radius, roundTrippedCylinder.Radius);
        Assert.Equal(originalCylinder.Height, roundTrippedCylinder.Height);
    }

    [Fact]
    public void ToDomain_ThenToDBO_ShouldRoundTripCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 12;
        var height = 24;
        var originalCylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = radius,
            Height = height
        };

        // Act
        var cylinder = CylinderMapper.ToDomain(originalCylinderDBO);
        var roundTrippedCylinderDBO = CylinderMapper.ToDBO(cylinder);

        // Assert
        Assert.Equal(originalCylinderDBO.Id, roundTrippedCylinderDBO.Id);
        Assert.Equal(originalCylinderDBO.Radius, roundTrippedCylinderDBO.Radius);
        Assert.Equal(originalCylinderDBO.Height, roundTrippedCylinderDBO.Height);
    }

    [Fact]
    public void ToDBO_WithDifferentDimensions_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dimensions = new[] { (1, 1), (5, 10), (10, 20), (100, 200), (1000, 2000) };

        foreach (var (radius, height) in dimensions)
        {
            var cylinder = new Cylinder(id, radius, height);

            // Act
            var cylinderDBO = CylinderMapper.ToDBO(cylinder);

            // Assert
            Assert.Equal(radius, cylinderDBO.Radius);
            Assert.Equal(height, cylinderDBO.Height);
        }
    }

    [Fact]
    public void ToDomain_WithDifferentDimensions_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dimensions = new[] { (1, 1), (5, 10), (10, 20), (100, 200), (1000, 2000) };

        foreach (var (radius, height) in dimensions)
        {
            var cylinderDBO = new CylinderDBO
            {
                Id = id,
                Radius = radius,
                Height = height
            };

            // Act
            var cylinder = CylinderMapper.ToDomain(cylinderDBO);

            // Assert
            Assert.Equal(radius, cylinder.Radius);
            Assert.Equal(height, cylinder.Height);
        }
    }
}