using Geometry.Domain.CylinderInterface;
using Geometry.Domain.CylinderModel;

namespace Geometry.Application.Tests;

/// <summary>
/// Mock implementation of ICylinderRepository for testing purposes.
/// </summary>
public class MockCylinderRepository : ICylinderRepository
{
    private readonly Dictionary<Guid, Cylinder> _cylinders = new();
    public int ReadByIdCallCount { get; private set; }
    public int InsertCallCount { get; private set; }
    public int UpdateCallCount { get; private set; }
    public int DeleteCallCount { get; private set; }
    public Guid? LastReadByIdParameter { get; private set; }
    public Cylinder? LastInsertParameter { get; private set; }
    public Cylinder? LastUpdateParameter { get; private set; }
    public Guid? LastDeleteParameter { get; private set; }

    public Task<Cylinder?> ReadById(Guid id)
    {
        ReadByIdCallCount++;
        LastReadByIdParameter = id;
        _cylinders.TryGetValue(id, out var cylinder);
        return Task.FromResult<Cylinder?>(cylinder);
    }

    public Task<Guid> Insert(Cylinder cylinder)
    {
        InsertCallCount++;
        LastInsertParameter = cylinder;
        if (cylinder != null)
        {
            _cylinders[cylinder.Id] = cylinder;
            return Task.FromResult(cylinder.Id);
        }
        throw new ArgumentNullException(nameof(cylinder));
    }

    public Task<bool> Update(Cylinder cylinder)
    {
        UpdateCallCount++;
        LastUpdateParameter = cylinder;
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
        DeleteCallCount++;
        LastDeleteParameter = id;

        if (!_cylinders.ContainsKey(id))
        {
            return Task.FromResult(false);
        }

        _cylinders.Remove(id);
        return Task.FromResult(true);
    }

    public void Reset()
    {
        _cylinders.Clear();
        ReadByIdCallCount = 0;
        InsertCallCount = 0;
        UpdateCallCount = 0;
        DeleteCallCount = 0;
        LastReadByIdParameter = null;
        LastInsertParameter = null;
        LastUpdateParameter = null;
        LastDeleteParameter = null;
    }
}

public class CylinderServiceTests
{
    [Fact]
    public void Constructor_WithNullRepository_ShouldCreateInstance()
    {
        // Arrange
        ICylinderRepository repository = null!;

        // Act
        var service = new CylinderService(repository);

        // Assert
        Assert.NotNull(service);
        // Note: Methods will throw NullReferenceException if repository is null
    }

    [Fact]
    public async Task Insert_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var cylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.Insert(cylinder));
    }

    [Fact]
    public async Task ReadById_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.ReadById(id));
    }

    [Fact]
    public async Task Update_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var cylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.Update(cylinder));
    }

    [Fact]
    public async Task Delete_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.Delete(id));
    }

    [Fact]
    public void Constructor_WithValidRepository_ShouldCreateInstance()
    {
        // Arrange
        var repository = new MockCylinderRepository();

        // Act
        var service = new CylinderService(repository);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task Insert_WithValidCylinder_ShouldDelegateToRepository()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // Act
        var result = await service.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);
        Assert.Equal(1, repository.InsertCallCount);
        Assert.Equal(cylinder, repository.LastInsertParameter);
    }

    [Fact]
    public async Task Insert_WithValidCylinder_ShouldReturnCylinderId()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 10, 20);

        // Act
        var result = await service.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task Insert_WithNullCylinder_ShouldPropagateException()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        Cylinder cylinder = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.Insert(cylinder));
    }

    [Fact]
    public async Task ReadById_WithExistingCylinder_ShouldDelegateToRepository()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // First insert the cylinder
        await repository.Insert(cylinder);

        // Act
        var result = await service.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(5, result.Radius);
        Assert.Equal(10, result.Height);
        Assert.Equal(1, repository.ReadByIdCallCount);
        Assert.Equal(id, repository.LastReadByIdParameter);
    }

    [Fact]
    public async Task ReadById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await service.ReadById(nonExistentId);

        // Assert
        Assert.Null(result);
        Assert.Equal(1, repository.ReadByIdCallCount);
        Assert.Equal(nonExistentId, repository.LastReadByIdParameter);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldDelegateToRepository()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        var updatedCylinder = new Cylinder(id, 15, 20);

        // Act
        var result = await service.Update(updatedCylinder);

        // Assert
        Assert.True(result);
        Assert.Equal(1, repository.UpdateCallCount);
        Assert.Equal(updatedCylinder, repository.LastUpdateParameter);
    }

    [Fact]
    public async Task Update_WithNonExistentCylinder_ShouldReturnFalse()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentCylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act
        var result = await service.Update(nonExistentCylinder);

        // Assert
        Assert.False(result);
        Assert.Equal(1, repository.UpdateCallCount);
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldDelegateToRepository()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);
        await repository.Insert(cylinder);

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result);
        Assert.Equal(1, repository.DeleteCallCount);
        Assert.Equal(id, repository.LastDeleteParameter);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await service.Delete(nonExistentId);

        // Assert
        Assert.False(result);
        Assert.Equal(1, repository.DeleteCallCount);
        Assert.Equal(nonExistentId, repository.LastDeleteParameter);
    }

    [Fact]
    public async Task Insert_ThenReadById_ShouldReturnInsertedCylinder()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 7, 14);

        // Act
        await service.Insert(cylinder);
        var result = await service.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(7, result.Radius);
        Assert.Equal(14, result.Height);
    }

    [Fact]
    public async Task MultipleInserts_ShouldDelegateToRepositoryEachTime()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var cylinder1 = new Cylinder(Guid.NewGuid(), 5, 10);
        var cylinder2 = new Cylinder(Guid.NewGuid(), 10, 20);
        var cylinder3 = new Cylinder(Guid.NewGuid(), 15, 30);

        // Act
        await service.Insert(cylinder1);
        await service.Insert(cylinder2);
        await service.Insert(cylinder3);

        // Assert
        Assert.Equal(3, repository.InsertCallCount);
        Assert.Equal(cylinder3, repository.LastInsertParameter);
    }

    [Fact]
    public async Task MultipleReads_ShouldDelegateToRepositoryEachTime()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var cylinder1 = new Cylinder(id1, 5, 10);
        var cylinder2 = new Cylinder(id2, 10, 20);

        await repository.Insert(cylinder1);
        await repository.Insert(cylinder2);

        // Act
        var result1 = await service.ReadById(id1);
        var result2 = await service.ReadById(id2);
        var result3 = await service.ReadById(Guid.NewGuid());

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Null(result3);
        Assert.Equal(3, repository.ReadByIdCallCount);
    }

    [Fact]
    public async Task Insert_WithDifferentDimensions_ShouldWorkCorrectly()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var dimensions = new[] { (1, 2), (5, 10), (10, 20), (100, 200), (1000, 2000) };

        // Act & Assert
        foreach (var (radius, height) in dimensions)
        {
            var id = Guid.NewGuid();
            var cylinder = new Cylinder(id, radius, height);
            var result = await service.Insert(cylinder);

            Assert.Equal(id, result);
            var retrieved = await service.ReadById(id);
            Assert.NotNull(retrieved);
            Assert.Equal(radius, retrieved.Radius);
            Assert.Equal(height, retrieved.Height);
        }
    }

    [Fact]
    public async Task Service_ShouldMaintainRepositoryReference()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var cylinder = new Cylinder(Guid.NewGuid(), 5, 10);

        // Act
        await service.Insert(cylinder);
        var retrieved = await service.ReadById(cylinder.Id);

        // Assert
        Assert.NotNull(retrieved);
        // Verify that the same repository instance was used
        Assert.Equal(1, repository.InsertCallCount);
        Assert.Equal(1, repository.ReadByIdCallCount);
    }

    [Fact]
    public async Task FullCrudCycle_ShouldWorkCorrectly()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5, 10);

        // Act - Create
        var insertResult = await service.Insert(cylinder);
        Assert.Equal(id, insertResult);

        // Act - Read
        var readResult = await service.ReadById(id);
        Assert.NotNull(readResult);
        Assert.Equal(5, readResult.Radius);
        Assert.Equal(10, readResult.Height);

        // Act - Update
        var updatedCylinder = new Cylinder(id, 15, 20);
        var updateResult = await service.Update(updatedCylinder);
        Assert.True(updateResult);

        // Verify update
        var afterUpdateResult = await service.ReadById(id);
        Assert.NotNull(afterUpdateResult);
        Assert.Equal(15, afterUpdateResult.Radius);
        Assert.Equal(20, afterUpdateResult.Height);

        // Act - Delete
        var deleteResult = await service.Delete(id);
        Assert.True(deleteResult);

        // Verify delete
        var afterDeleteResult = await service.ReadById(id);
        Assert.Null(afterDeleteResult);
    }
}