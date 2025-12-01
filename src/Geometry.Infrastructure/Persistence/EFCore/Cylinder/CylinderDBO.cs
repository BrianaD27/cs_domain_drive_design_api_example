namespace Geometry.Infrastructure.Persistence.EFCore.Cylinder;

/// <summary>
/// Database object representation of a Cylinder entity for Entity Framework Core persistence.
/// This class maps to the database table storing Cylinder information.
/// </summary>
public class CylinderDBO
{
    /// <summary>
    /// Gets or sets the unique identifier of the cube.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the radius the cylinder.
    /// </summary>
    public int Radius { get; set; }

    /// <summary>
    /// Gets or sets the height the cylinder.
    /// </summary>
    public int Height { get; set; }
}