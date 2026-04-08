using GenricRepository.Application;
using GenricRepository.Application.Contracts.Roles;
using Microsoft.AspNetCore.Mvc;

namespace GenricRepository.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<RoleResponse>>> GetAll()
    {
        return Ok(await _roleService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> GetById(Guid id)
    {
        var role = await _roleService.GetByIdAsync(id);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponse>> Create(CreateRoleRequest request)
    {
        var role = await _roleService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> Update(Guid id, UpdateRoleRequest request)
    {
        var role = await _roleService.UpdateAsync(id, request);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _roleService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
