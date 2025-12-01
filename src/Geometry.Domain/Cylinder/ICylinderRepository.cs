using Geometry.Domain.CylinderModel;
namespace Geometry.Domain.CylinderInterface;

/// <summary>
/// Repository interface for managing Cylinder entities.
/// Provides methods for retrieving and persisting Cylinder instances.
/// </summary>
public interface ICylinderRepository
{
    /// <summary>
    /// Retrieves a Cylinder by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Cylinder to retrieve.</param>
    /// <returns>The Cylinder with the specified identifier, or null if not found.</returns>
    Task<Cylinder?> ReadById(Guid id);

    /// <summary>
    /// Creates a Cylinder entity in the repository.
    /// </summary>
    /// <param name="cylinder">The Cylinder entity to create.</param>
    Task<Guid> Insert(Cylinder cylinder);

    /// <summary>
    /// Updates a Cylinder entity in the repository.
    /// </summary>
    /// <param name="cylinder">The Cylinder entity to create.</param>
    /// <returns>True or false depending on if the cylinder was updated.</returns>
    Task<bool> Update(Cylinder cylinder);

    /// <summary>
    /// Deletes a Cylinder entity in the repository.
    /// </summary>
    /// <param name="cylinder">The Cylinder entity to create.</param>
    /// <returns>True or false depending on if the cylinder was deleted.</returns>
    Task<bool> Delete(Cylinder cylinder);
}