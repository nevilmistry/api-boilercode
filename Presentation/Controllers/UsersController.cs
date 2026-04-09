using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Users;
using GenricRepository.Application.Handlers.Users;
using Microsoft.AspNetCore.Mvc;

namespace GenricRepository.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserQueryHandler _userQueryHandler;
    private readonly IUserCommandHandler _userCommandHandler;

    public UsersController(IUserQueryHandler userQueryHandler, IUserCommandHandler userCommandHandler)
    {
        _userQueryHandler = userQueryHandler;
        _userCommandHandler = userCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetAll([FromQuery] UsersListQuery query)
    {
        return Ok(await _userQueryHandler.GetAllAsync(query));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id)
    {
        var user = await _userQueryHandler.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
    {
        var user = await _userCommandHandler.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _userCommandHandler.UpdateAsync(id, request);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _userCommandHandler.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
