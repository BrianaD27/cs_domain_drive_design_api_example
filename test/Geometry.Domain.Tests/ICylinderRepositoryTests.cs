using Geometry.Domain.CylinderInterface;
using Geometry.Domain.CylinderModel;

namespace Geometry.Domain.Tests;

/// <summary>
/// In-memory implementation of ICylinderRepository for testing purposes.
/// </summary>
public class InMemoryCylinderRepository : ICylinderRepository
{
    private readonly Dictionary<Guid, Cylinder> _cylinders = new();

    public Task<Cylinder?> ReadById(Guid id)
    {
        _cylinders.TryGetValue(id, out var cylinder);
        return Task.FromResult<Cylinder?>(cylinder);
    }

    public Task<Guid> Insert(Cylinder cylinder)
    {
        if (cylinder == null)
        {
            throw new ArgumentNullException(nameof(cylinder));
        }

        _cylinders[cylinder.Id] = cylinder;
        return Task.FromResult(cylinder.Id);
    }

    public Task<bool> Update(Cylinder cylinder)
    {
        if (cylinder == null)
        {
            throw new ArgumentNullException(nameof(cylinder));
        }

        if (!_cylinders.ContainsKey(cylinder.Id))
        {
            return Task.FromResult(false);
        }

        _cylinders[cylinder.Id] = cylinder;
        return Task.FromResult(true);
    }

    public Task<bool> Delete(Guid id)
    {
        if (!_cylinders.ContainsKey(id))
        {
            return Task.FromResult(false);
        }

        _cylinders.Remove(id);
        return Task.FromResult(true);
    }

    public void Clear()
    {
        _cylinders.Clear();
    }

    public int Count => _cylinders.Count;
}

public class ICylinderRepositoryTests
{
    [Fact]
    public async Task ReadById_WithExistingCylinder_ShouldReturnCylinder()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
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
        var repository = new InMemoryCylinderRepository();
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.ReadById(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Insert_WithValidCylinder_ShouldReturnCylinderId()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // Act
        var result = await repository.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task Insert_WithValidCylinder_ShouldStoreCylinder()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // Act
        await repository.Insert(cylinder);

        // Assert
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(id, retrieved.Id);
        Assert.Equal(5, retrieved.Radius);
        Assert.Equal(10, retrieved.Height);
    }

    [Fact]
    public async Task Insert_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Insert(null!));
    }

    [Fact]
    public async Task Insert_WithSameId_ShouldUpdateExistingCylinder()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
        var id = Guid.NewGuid();
        var cylinder1 = new Cylinder(id, 5, 10);
        var cylinder2 = new Cylinder(id, 15, 20);

        // Act
        await repository.Insert(cylinder1);
        await repository.Insert(cylinder2);

        // Assert
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(15, retrieved.Radius); // Should be updated to cylinder2's value
        Assert.Equal(20, retrieved.Height);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
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
        var repository = new InMemoryCylinderRepository();
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
        var repository = new InMemoryCylinderRepository();
        var nonExistentCylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act
        var result = await repository.Update(nonExistentCylinder);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Update_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Update(null!));
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
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
        var repository = new InMemoryCylinderRepository();
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
        var repository = new InMemoryCylinderRepository();
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.Delete(nonExistentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ReadById_WithEmptyRepository_ShouldReturnNull()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();

        // Act
        var result = await repository.ReadById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Insert_MultipleCylinders_ShouldStoreAllCylinders()
    {
        // Arrange
        var repository = new InMemoryCylinderRepository();
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
}