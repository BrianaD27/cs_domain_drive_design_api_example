using Geometry.Domain.CylinderModel;

namespace Geometry.Infrastructure.Persistence.EFCore;

/// <summary>
/// Mapper class for converting between Cylinder domain entities and CylinderDBO database objects.
/// Provides bidirectional mapping functionality for persistence operations.
/// </summary>
public static class CylinderMapper
{
    /// <summary>
    /// Maps a Cylinder domain entity to a CylinderDBO database object.
    /// </summary>
    /// <param name="cylinder">The Cylinder domain entity to map.</param>
    /// <returns>A new CylinderDBO instance with properties mapped from the domain entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown when Cylinder is null.</exception>
    public static CylinderDBO ToDBO(Cylinder cylinder)
    {
        if (cylinder == null)
        {
            throw new ArgumentNullException(nameof(cylinder));
        }

        return new CylinderDBO
        {
            Id = cylinder.Id,
            Radius = cylinder.Radius,
            Height = cylinder.Height
        };
    }

    /// <summary>
    /// Maps a Cylinder database object to a Cylinder domain entity.
    /// </summary>
    /// <param name="cylinderDBO">The Cylinder database object to map.</param>
    /// <returns>A new Cylinder domain entity instance with properties mapped from the database object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when CylinderDBO is null.</exception>
    public static Cylinder ToDomain(CylinderDBO cylinderDBO)
    {
        if (cylinderDBO == null)
        {
            throw new ArgumentNullException(nameof(cylinderDBO));
        }

        return new Cylinder(cylinderDBO.Id, cylinderDBO.Radius, cylinderDBO.Height);
    }
}
