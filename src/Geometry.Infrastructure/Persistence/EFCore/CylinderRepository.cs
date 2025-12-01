using Geometry.Domain.CylinderInterface;
using Geometry.Domain.CylinderModel;
using Microsoft.EntityFrameworkCore;

namespace Geometry.Infrastructure.Persistence.EFCore;

/// <summary>
/// Entity Framework Core implementation of the ICylinderRepository interface.
/// Provides persistence operations for Cylinder entities using EF Core.
/// </summary>
public class CylinderRepository : ICylinderRepository
{
    private readonly GeometryDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CylinderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to use for persistence operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
    public CylinderRepository(GeometryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Retrieves a Cylinder by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Cylinder to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the Cylinder with the specified identifier, or null if not found.
    /// </returns>
    public async Task<Cylinder?> ReadById(Guid id)
    {
        var cylinderDBO = await _context.Cylinders
            .FirstOrDefaultAsync(c => c.Id == id);

        return cylinderDBO == null ? null : CylinderMapper.ToDomain(cylinderDBO);
    }

    /// <summary>
    /// Creates a Cylinder entity in the repository.
    /// </summary>
    /// <param name="cylinder">The Cylinder entity to create.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the unique identifier of the created cylinder.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when cylinder is null.</exception>
    public async Task<Guid> Insert(Cylinder cylinder)
    {
        if (cylinder == null)
        {
            throw new ArgumentNullException(nameof(cylinder));
        }

        var existingCylinder = await _context.Cylinders
            .FirstOrDefaultAsync(c => c.Id == cylinder.Id);

        if (existingCylinder == null)
        {
            // Insert new entity
            var cylinderDBO = CylinderMapper.ToDBO(cylinder);
            await _context.Cylinders.AddAsync(cylinderDBO);
        } 
        else
        {
            throw new Exception("Cylinder already exists. Please update this entity instead.");
        };

        await _context.SaveChangesAsync();

        return cylinder.Id;
    }

    /// <summary>
    /// Updates an existing Cylinder entity in the repository.
    /// </summary>
    /// <param name="cylinder">The Cylinder entity to update.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains true or false depending on if the update operation was successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when cylinder is null.</exception>
    public async Task<bool> Update(Cylinder cylinder)
    {
        var existingCylinder = await _context.Cylinders
            .FirstOrDefaultAsync(c => c.Id == cylinder.Id);

        if (existingCylinder != null)
        {
            // Update existing entity
            existingCylinder.Radius = existingCylinder.Radius;
            existingCylinder.Height = existingCylinder.Height;
            _context.Cylinders.Update(existingCylinder);
            await _context.SaveChangesAsync();
            return true;
        } 

        return false;
        throw new ArgumentNullException(nameof(existingCylinder));
    }

     /// <summary>
    /// Deletes an existing Cylinder entity in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the Cylinder to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains true or false depending on if the delete operation was successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when cylinder is null.</exception>
    
    public async Task<bool> Delete(Guid id)
    {

        var existingCylinder = await _context.Cylinders
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCylinder != null)
        {
            // Delete existing entity
            _context.Cylinders.Remove(existingCylinder);
            await _context.SaveChangesAsync();
            return true;
        } 
        return false;
    }
}
