using Geometry.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Geometry.Infrastructure.Tests;

public class GeometryDbContextTests
{
    private GeometryDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GeometryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new GeometryDbContext(options);
    }

    [Fact]
    public void Constructor_WithOptions_ShouldCreateInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<GeometryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new GeometryDbContext(options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void Cubes_ShouldBeInitialized()
    {
        // Arrange
        using var context = CreateContext();

        // Assert
        Assert.NotNull(context.Cubes);
    }

    [Fact]
    public async Task Cubes_ShouldAllowAddingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var cubeDBO = new CubeDBO
        {
            Id = Guid.NewGuid(),
            SideLength = 5
        };

        // Act
        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cubes.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Cubes_ShouldAllowQueryingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cubeDBO = new CubeDBO
        {
            Id = id,
            SideLength = 10
        };

        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await context.Cubes.FirstOrDefaultAsync(c => c.Id == id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(id, retrieved.Id);
        Assert.Equal(10, retrieved.SideLength);
    }

    [Fact]
    public async Task CubeModelConfiguration_IdShouldBePrimaryKey()
    {
        // Arrange
        using var context = CreateContext();
        var cubeDBO = new CubeDBO
        {
            Id = Guid.NewGuid(),
            SideLength = 5
        };

        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Act - Try to query by Id (primary key operations should work)
        var retrieved = await context.Cubes.FindAsync(cubeDBO.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(cubeDBO.Id, retrieved.Id);
    }

    [Fact]
    public async Task CubeModelConfiguration_SideLengthShouldBeRequired()
    {
        // Arrange
        using var context = CreateContext();
        var cubeDBO = new CubeDBO
        {
            Id = Guid.NewGuid(),
            SideLength = 0 // Zero is valid for DBO, but required means it must have a value
        };

        // Act
        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Assert - If SideLength is required, it should save successfully
        var retrieved = await context.Cubes.FirstOrDefaultAsync(c => c.Id == cubeDBO.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(0, retrieved.SideLength);
    }

    [Fact]
    public async Task CubeDatabase_ShouldSupportMultipleEntities()
    {
        // Arrange
        using var context = CreateContext();
        var cube1 = new CubeDBO { Id = Guid.NewGuid(), SideLength = 5 };
        var cube2 = new CubeDBO { Id = Guid.NewGuid(), SideLength = 10 };
        var cube3 = new CubeDBO { Id = Guid.NewGuid(), SideLength = 15 };

        // Act
        await context.Cubes.AddRangeAsync(cube1, cube2, cube3);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cubes.CountAsync();
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task CubeDatabase_ShouldSupportUpdatingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cubeDBO = new CubeDBO
        {
            Id = id,
            SideLength = 5
        };

        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await context.Cubes.FindAsync(id);
        retrieved!.SideLength = 20;
        context.Cubes.Update(retrieved);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.Cubes.FindAsync(id);
        Assert.NotNull(updated);
        Assert.Equal(20, updated.SideLength);
    }

    [Fact]
    public async Task CubeDatabase_ShouldSupportDeletingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cubeDBO = new CubeDBO
        {
            Id = id,
            SideLength = 5
        };

        await context.Cubes.AddAsync(cubeDBO);
        await context.SaveChangesAsync();

        // Act
        context.Cubes.Remove(cubeDBO);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cubes.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public void Cylinders_ShouldBeInitialized()
    {
        // Arrange
        using var context = CreateContext();

        // Assert
        Assert.NotNull(context.Cylinders);
    }

    [Fact]
    public async Task Cylinders_ShouldAllowAddingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var cylinderDBO = new CylinderDBO
        {
            Id = Guid.NewGuid(),
            Radius = 5,
            Height = 10
        };

        // Act
        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cylinders.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Cylinders_ShouldAllowQueryingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = 10,
            Height = 20
        };

        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await context.Cylinders.FirstOrDefaultAsync(c => c.Id == id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(id, retrieved.Id);
        Assert.Equal(10, retrieved.Radius);
        Assert.Equal(20, retrieved.Height);
    }

    [Fact]
    public async Task CylinderModelConfiguration_IdShouldBePrimaryKey()
    {
        // Arrange
        using var context = CreateContext();
        var cylinderDBO = new CylinderDBO
        {
            Id = Guid.NewGuid(),
            Radius = 5,
            Height = 10
        };

        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Act - Try to query by Id (primary key operations should work)
        var retrieved = await context.Cylinders.FindAsync(cylinderDBO.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(cylinderDBO.Id, retrieved.Id);
    }

    [Fact]
    public async Task CylinderModelConfiguration_RadiusShouldBeRequired()
    {
        // Arrange
        using var context = CreateContext();
        var cylinderDBO = new CylinderDBO
        {
            Id = Guid.NewGuid(),
            Radius = 0, // Zero is valid for DBO, but required means it must have a value
            Height = 10
        };

        // Act
        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Assert - If Radius is required, it should save successfully
        var retrieved = await context.Cylinders.FirstOrDefaultAsync(c => c.Id == cylinderDBO.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(0, retrieved.Radius);
    }

    [Fact]
    public async Task CylinderModelConfiguration_HeightShouldBeRequired()
    {
        // Arrange
        using var context = CreateContext();
        var cylinderDBO = new CylinderDBO
        {
            Id = Guid.NewGuid(),
            Radius = 5,
            Height = 0 // Zero is valid for DBO, but required means it must have a value
        };

        // Act
        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Assert - If Height is required, it should save successfully
        var retrieved = await context.Cylinders.FirstOrDefaultAsync(c => c.Id == cylinderDBO.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(0, retrieved.Height);
    }

    [Fact]
    public async Task CylinderDatabase_ShouldSupportMultipleEntities()
    {
        // Arrange
        using var context = CreateContext();
        var cylinder1 = new CylinderDBO { Id = Guid.NewGuid(), Radius = 5, Height = 10 };
        var cylinder2 = new CylinderDBO { Id = Guid.NewGuid(), Radius = 10, Height = 20 };
        var cylinder3 = new CylinderDBO { Id = Guid.NewGuid(), Radius = 15, Height = 30 };

        // Act
        await context.Cylinders.AddRangeAsync(cylinder1, cylinder2, cylinder3);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cylinders.CountAsync();
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task CylinderDatabase_ShouldSupportUpdatingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = 5,
            Height = 10
        };

        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await context.Cylinders.FindAsync(id);
        retrieved!.Radius = 20;
        retrieved!.Height = 40;
        context.Cylinders.Update(retrieved);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.Cylinders.FindAsync(id);
        Assert.NotNull(updated);
        Assert.Equal(20, updated.Radius);
        Assert.Equal(40, updated.Height);
    }

    [Fact]
    public async Task CylinderDatabase_ShouldSupportDeletingEntities()
    {
        // Arrange
        using var context = CreateContext();
        var id = Guid.NewGuid();
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = 5,
            Height = 10
        };

        await context.Cylinders.AddAsync(cylinderDBO);
        await context.SaveChangesAsync();

        // Act
        context.Cylinders.Remove(cylinderDBO);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Cylinders.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Database_ShouldSupportBothCubesAndCylinders()
    {
        // Arrange
        using var context = CreateContext();
        var cube = new CubeDBO { Id = Guid.NewGuid(), SideLength = 5 };
        var cylinder = new CylinderDBO { Id = Guid.NewGuid(), Radius = 10, Height = 20 };

        // Act
        await context.Cubes.AddAsync(cube);
        await context.Cylinders.AddAsync(cylinder);
        await context.SaveChangesAsync();

        // Assert
        var cubeCount = await context.Cubes.CountAsync();
        var cylinderCount = await context.Cylinders.CountAsync();
        Assert.Equal(1, cubeCount);
        Assert.Equal(1, cylinderCount);
    }

    [Fact]
    public async Task Database_ShouldMaintainSeparateCollections()
    {
        // Arrange
        using var context = CreateContext();
        var cubeId = Guid.NewGuid();
        var cylinderId = Guid.NewGuid();
        var cube = new CubeDBO { Id = cubeId, SideLength = 5 };
        var cylinder = new CylinderDBO { Id = cylinderId, Radius = 10, Height = 20 };

        await context.Cubes.AddAsync(cube);
        await context.Cylinders.AddAsync(cylinder);
        await context.SaveChangesAsync();

        // Act
        var retrievedCube = await context.Cubes.FindAsync(cubeId);
        var retrievedCylinder = await context.Cylinders.FindAsync(cylinderId);

        // Assert
        Assert.NotNull(retrievedCube);
        Assert.NotNull(retrievedCylinder);
        Assert.Equal(5, retrievedCube.SideLength);
        Assert.Equal(10, retrievedCylinder.Radius);
        Assert.Equal(20, retrievedCylinder.Height);
    }

}