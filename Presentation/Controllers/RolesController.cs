using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Roles;
using GenricRepository.Application.Handlers.Roles;
using Microsoft.AspNetCore.Mvc;

namespace GenricRepository.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RolesController : ControllerBase
{
    private const string EntityName = "Role";
    private const string EntityPluralName = "Roles";

    private readonly IRoleQueryHandler _roleQueryHandler;
    private readonly IRoleCommandHandler _roleCommandHandler;

    public RolesController(IRoleQueryHandler roleQueryHandler, IRoleCommandHandler roleCommandHandler)
    {
        _roleQueryHandler = roleQueryHandler;
        _roleCommandHandler = roleCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<ApiEnvelope<PagedResult<RoleResponse>>>> GetAll([FromQuery] RolesListQuery query)
    {
        var roles = await _roleQueryHandler.GetAllAsync(query);
        return Ok(ApiEnvelope.Ok(roles, CommonMessages.ListFetched(EntityPluralName)));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<RoleResponse>>> GetById(Guid id)
    {
        var role = await _roleQueryHandler.GetByIdAsync(id);
        return role is null
            ? NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)))
            : Ok(ApiEnvelope.Ok(role, CommonMessages.Fetched(EntityName)));
    }

    [HttpPost]
    public async Task<ActionResult<ApiEnvelope<RoleResponse>>> Create(CreateRoleRequest request)
    {
        var role = await _roleCommandHandler.CreateAsync(request);
        var response = ApiEnvelope.Created(role, CommonMessages.Created(EntityName));
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<RoleResponse>>> Update(Guid id, UpdateRoleRequest request)
    {
        var role = await _roleCommandHandler.UpdateAsync(id, request);
        return role is null
            ? NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)))
            : Ok(ApiEnvelope.Ok(role, CommonMessages.Updated(EntityName)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<object?>>> Delete(Guid id)
    {
        if (!await _roleCommandHandler.DeleteAsync(id))
        {
            return NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)));
        }

        return Ok(ApiEnvelope.SuccessMessage(CommonMessages.Deleted(EntityName)));
    }
}
