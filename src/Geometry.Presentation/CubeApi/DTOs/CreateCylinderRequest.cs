namespace Geometry.Presentation.CubeApi.DTOs;

/// <summary>
/// Data Transfer Object for creating a new cube.
/// Represents the request payload for the POST /cube endpoint.
/// </summary>
public class CreateCylinderRequest
{
    /// <summary>
    /// Gets or sets the radius and height of the cylinder.
    /// Must be greater than 0.
    /// </summary>
    /// <example>5</example>
    public int Radius { get; set; }
    public int Height { get; set; }

}
