using Geometry.Domain.CylinderInterface;
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
    /// Initializes a new instance of the <see cref="CubeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to use for persistence operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
    public CubeRepository(GeometryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Retrieves a Cube by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Cube to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the cube with the specified identifier, or null if not found.
    /// </returns>
    public async Task<Cube?> ReadById(Guid id)
    {
        var cubeDBO = await _context.Cubes
            .FirstOrDefaultAsync(c => c.Id == id);

        return cubeDBO == null ? null : CubeMapper.ToDomain(cubeDBO);
    }

    /// <summary>
    /// Saves or updates a Cube entity in the repository.
    /// </summary>
    /// <param name="cube">The Cube entity to save or update.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the unique identifier of the saved or updated cube.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when cube is null.</exception>
    public async Task<Guid> Insert(Cube cube)
    {
        if (cube == null)
        {
            throw new ArgumentNullException(nameof(cube));
        }

        var existingCube = await _context.Cubes
            .FirstOrDefaultAsync(c => c.Id == cube.Id);

        if (existingCube != null)
        {
            // Update existing entity
            existingCube.SideLength = cube.SideLength;
            _context.Cubes.Update(existingCube);
        }
        else
        {
            // Insert new entity
            var cubeDBO = CubeMapper.ToDBO(cube);
            await _context.Cubes.AddAsync(cubeDBO);
        }

        await _context.SaveChangesAsync();

        return cube.Id;
    }
}
