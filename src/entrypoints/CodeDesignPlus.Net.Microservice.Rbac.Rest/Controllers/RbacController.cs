namespace CodeDesignPlus.Net.Microservice.Rbac.Rest.Controllers;

/// <summary>
/// Controller for managing the Rbac.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class RbacController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Rbac.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Rbac.</returns>
    [HttpGet]
    public async Task<IActionResult> GetRbac([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllRbacQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Rbac by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Rbac.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRbacById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRbacByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Rbac.
    /// </summary>
    /// <param name="data">Data for creating the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRbac([FromBody] CreateRbacDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateRbacCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Rbac.
    /// </summary>
    /// <param name="id">The unique identifier of the Rbac.</param>
    /// <param name="data">Data for updating the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRbac(Guid id, [FromBody] UpdateRbacDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateRbacCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Rbac.
    /// </summary>
    /// <param name="id">The unique identifier of the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRbac(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteRbacCommand(id), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Add a permission to a Rbac.
    /// </summary>
    /// <param name="id">The unique identifier of the Rbac.</param>
    /// <param name="data">Data for adding the permission to the Rbac.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AddPermission(Guid id, [FromBody] AddPermissionDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<AddPermissionCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Remove a permission from a Rbac.
    /// </summary>
    /// <param name="id">The unique identifier of the Rbac.</param>
    /// <param name="idRbacPermission">The unique identifier of the permission.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}/permissions/{idRbacPermission}")]
    public async Task<IActionResult> RemovePermission(Guid id, Guid idRbacPermission, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemovePermissionCommand(id, idRbacPermission), cancellationToken);

        return NoContent();
    }
}