namespace Geometry.Presentation.CubeApi.DTOs;

/// <summary>
/// Data Transfer Object for updating an existing cylinder.
/// </summary>
public class UpdateCylinderRequest
{
    /// <summary>
    /// Gets or sets the radius of the cylinder.
    /// Must be greater than 0.
    /// </summary>
    /// <example>5</example>
    public int Radius { get; set; }

    /// <summary>
    /// Gets or sets the height of the cylinder.
    /// Must be greater than 0.
    /// </summary>
    /// <example>10</example>
    public int Height { get; set; }
}