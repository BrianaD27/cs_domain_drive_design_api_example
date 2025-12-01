namespace Geometry.Presentation.CubeApi.DTOs;

/// <summary>
/// Data Transfer Object representing a cube entity in API responses.
/// Used to return cube data from GET /cube/{id} endpoint.
/// </summary>
public class CylinderResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the Cylinder.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Radius of the Cylinder.
    /// </summary>
    /// <example>5</example>
    public int Radius { get; set; }

    /// <summary>
    /// Gets or sets the Height of the Cylinder.
    /// </summary>
    /// <example>5</example>
    public int Height { get; set; }
}