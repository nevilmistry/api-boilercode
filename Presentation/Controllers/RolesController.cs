using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Roles;
using GenricRepository.Application.Handlers.Roles;
using Microsoft.AspNetCore.Mvc;

namespace GenricRepository.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RolesController : ControllerBase
{
    private readonly IRoleQueryHandler _roleQueryHandler;
    private readonly IRoleCommandHandler _roleCommandHandler;

    public RolesController(IRoleQueryHandler roleQueryHandler, IRoleCommandHandler roleCommandHandler)
    {
        _roleQueryHandler = roleQueryHandler;
        _roleCommandHandler = roleCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<RoleResponse>>> GetAll([FromQuery] RolesListQuery query)
    {
        return Ok(await _roleQueryHandler.GetAllAsync(query));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> GetById(Guid id)
    {
        var role = await _roleQueryHandler.GetByIdAsync(id);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponse>> Create(CreateRoleRequest request)
    {
        var role = await _roleCommandHandler.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> Update(Guid id, UpdateRoleRequest request)
    {
        var role = await _roleCommandHandler.UpdateAsync(id, request);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _roleCommandHandler.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
