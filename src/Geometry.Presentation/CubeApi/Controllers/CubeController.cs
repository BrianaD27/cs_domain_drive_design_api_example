using Geometry.Application;
using Geometry.Presentation.CubeApi.DTOs;
using Geometry.Presentation.CubeApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Geometry.Presentation.CubeApi.Controllers;

/// <summary>
/// Controller for managing cube operations via REST API.
/// Provides endpoints for creating and retrieving cube entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CubeController : ControllerBase
{
    private readonly CubeService _cubeService;
    private readonly ILogger<CubeController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CubeController"/> class.
    /// </summary>
    /// <param name="cubeService">The cube service for business logic operations.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public CubeController(CubeService cubeService, ILogger<CubeController> logger)
    {
        _cubeService = cubeService ?? throw new ArgumentNullException(nameof(cubeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new cube with the specified side length.
    /// </summary>
    /// <param name="request">The cube creation request containing the side length.</param>
    /// <returns>
    /// Created (201) with the created cube's identifier and location header,
    /// or BadRequest (400) if the request is invalid.
    /// </returns>
    /// <response code="201">Returns the created cube identifier</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateCube([FromBody] CreateCubeRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("CreateCube called with null request");
            return BadRequest("Request body cannot be null.");
        }

        if (request.SideLength <= 0)
        {
            _logger.LogWarning("CreateCube called with invalid side length: {SideLength}", request.SideLength);
            return BadRequest("SideLength must be greater than 0.");
        }

        try
        {
            var cube = CubeDtoMapper.ToDomain(request);
            var id = await _cubeService.Insert(cube);

            _logger.LogInformation("Cube created successfully with Id: {CubeId} and SideLength: {SideLength}", id, request.SideLength);

            return CreatedAtAction(
                nameof(GetCubeById),
                new { id = id },
                id);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "ArgumentException occurred while creating cube");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating cube");
            return StatusCode(500, "An error occurred while creating the cube.");
        }
    }

    /// <summary>
    /// Retrieves a cube by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the cube to retrieve.</param>
    /// <returns>
    /// OK (200) with the cube data if found,
    /// or NotFound (404) if the cube does not exist.
    /// </returns>
    /// <response code="200">Returns the cube data</response>
    /// <response code="404">If the cube is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CubeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CubeResponse>> GetCubeById(Guid id)
    {
        try
        {
            var cube = await _cubeService.ReadById(id);

            if (cube == null)
            {
                _logger.LogWarning("Cube with Id {CubeId} not found", id);
                return NotFound($"Cube with Id {id} was not found.");
            }

            _logger.LogInformation("Cube retrieved successfully with Id: {CubeId}", id);
            var response = CubeDtoMapper.ToDto(cube);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while retrieving cube with Id: {CubeId}", id);
            return StatusCode(500, "An error occurred while retrieving the cube.");
        }
    }
}
