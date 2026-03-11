using genricRepository.Application;
using genricRepository.Contracts.Roles.Requests;
using genricRepository.Contracts.Roles.Responses;
using Microsoft.AspNetCore.Mvc;

namespace genricRepository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoleResponse>> GetById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            return Ok(role);
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
            if (role is null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteRoleResponse>> Delete(Guid id)
        {
            var response = await _roleService.DeleteAsync(id);
            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
