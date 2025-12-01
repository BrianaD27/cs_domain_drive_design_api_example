using Geometry.Domain.CylinderModel;
using Geometry.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderRepositoryTests
{
    private GeometryDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GeometryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new GeometryDbContext(options);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange
        GeometryDbContext context = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CylinderRepository(context));
    }

    [Fact]
    public void Constructor_WithValidContext_ShouldCreateInstance()
    {
        // Arrange
        using var context = CreateContext();

        // Act
        var repository = new CylinderRepository(context);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task ReadById_WithExistingCylinder_ShouldReturnCylinder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        await repository.Insert(cylinder);

        // Act
        var result = await repository.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(5, result.Radius);
        Assert.Equal(10, result.Height);
    }

    [Fact]
    public async Task ReadById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.ReadById(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Insert_WithNewCylinder_ShouldSaveAndReturnId()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 10, 20);

        // Act
        var result = await repository.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);

        // Verify it was saved
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(10, retrieved.Radius);
        Assert.Equal(20, retrieved.Height);
    }

    [Fact]
    public async Task Insert_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        Cylinder cylinder = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Insert(cylinder));
    }

    [Fact]
    public async Task Insert_WithExistingId_ShouldThrowException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder1 = new Cylinder(id, 5, 10);
        var cylinder2 = new Cylinder(id, 15, 20);

        await repository.Insert(cylinder1);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => repository.Insert(cylinder2));
    }

    [Fact]
    public async Task Insert_MultipleDifferentCylinders_ShouldSaveAll()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var cylinder1 = new Cylinder(id1, 5, 10);
        var cylinder2 = new Cylinder(id2, 15, 20);

        // Act
        await repository.Insert(cylinder1);
        await repository.Insert(cylinder2);

        // Assert
        var retrieved1 = await repository.ReadById(id1);
        var retrieved2 = await repository.ReadById(id2);

        Assert.NotNull(retrieved1);
        Assert.NotNull(retrieved2);
        Assert.Equal(5, retrieved1.Radius);
        Assert.Equal(10, retrieved1.Height);
        Assert.Equal(15, retrieved2.Radius);
        Assert.Equal(20, retrieved2.Height);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        var updatedCylinder = new Cylinder(id, 15, 20);

        // Act
        var result = await repository.Update(updatedCylinder);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldUpdateValues()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        var updatedCylinder = new Cylinder(id, 15, 20);

        // Act
        await repository.Update(updatedCylinder);

        // Assert
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(15, retrieved.Radius);
        Assert.Equal(20, retrieved.Height);
    }

    [Fact]
    public async Task Update_WithNonExistentCylinder_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentCylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act
        var result = await repository.Update(nonExistentCylinder);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        // Act
        var result = await repository.Delete(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldRemoveCylinder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        // Act
        await repository.Delete(id);

        // Assert
        var retrieved = await repository.ReadById(id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.Delete(nonExistentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ReadById_WithEmptyDatabase_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);

        // Act
        var result = await repository.ReadById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Insert_ShouldPersistDataAcrossReadOperations()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 20, 40);

        // Act
        await repository.Insert(cylinder);
        var result1 = await repository.ReadById(id);
        var result2 = await repository.ReadById(id);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(result1.Id, result2.Id);
        Assert.Equal(result1.Radius, result2.Radius);
        Assert.Equal(result1.Height, result2.Height);
    }

    [Fact]
    public async Task FullCrudCycle_ShouldWorkCorrectly()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // Act - Create
        var insertResult = await repository.Insert(cylinder);
        Assert.Equal(id, insertResult);

        // Act - Read
        var readResult = await repository.ReadById(id);
        Assert.NotNull(readResult);
        Assert.Equal(5, readResult.Radius);
        Assert.Equal(10, readResult.Height);

        // Act - Update
        var updatedCylinder = new Cylinder(id, 15, 20);
        var updateResult = await repository.Update(updatedCylinder);
        Assert.True(updateResult);

        // Verify update
        var afterUpdateResult = await repository.ReadById(id);
        Assert.NotNull(afterUpdateResult);
        Assert.Equal(15, afterUpdateResult.Radius);
        Assert.Equal(20, afterUpdateResult.Height);

        // Act - Delete
        var deleteResult = await repository.Delete(id);
        Assert.True(deleteResult);

        // Verify delete
        var afterDeleteResult = await repository.ReadById(id);
        Assert.Null(afterDeleteResult);
    }
}