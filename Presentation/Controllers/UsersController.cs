using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Users;
using GenricRepository.Application.Handlers.Users;
using Microsoft.AspNetCore.Mvc;

namespace GenricRepository.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private const string EntityName = "User";
    private const string EntityPluralName = "Users";

    private readonly IUserQueryHandler _userQueryHandler;
    private readonly IUserCommandHandler _userCommandHandler;

    public UsersController(IUserQueryHandler userQueryHandler, IUserCommandHandler userCommandHandler)
    {
        _userQueryHandler = userQueryHandler;
        _userCommandHandler = userCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<ApiEnvelope<PagedResult<UserResponse>>>> GetAll([FromQuery] UsersListQuery query)
    {
        var users = await _userQueryHandler.GetAllAsync(query);
        return Ok(ApiEnvelope.Ok(users, CommonMessages.ListFetched(EntityPluralName)));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<UserResponse>>> GetById(Guid id)
    {
        var user = await _userQueryHandler.GetByIdAsync(id);
        return user is null
            ? NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)))
            : Ok(ApiEnvelope.Ok(user, CommonMessages.Fetched(EntityName)));
    }

    [HttpPost]
    public async Task<ActionResult<ApiEnvelope<UserResponse>>> Create(CreateUserRequest request)
    {
        var user = await _userCommandHandler.CreateAsync(request);
        var response = ApiEnvelope.Created(user, CommonMessages.Created(EntityName));
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<UserResponse>>> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _userCommandHandler.UpdateAsync(id, request);
        return user is null
            ? NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)))
            : Ok(ApiEnvelope.Ok(user, CommonMessages.Updated(EntityName)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiEnvelope<object?>>> Delete(Guid id)
    {
        if (!await _userCommandHandler.DeleteAsync(id))
        {
            return NotFound(ApiEnvelope.NotFound(CommonMessages.NotFound(EntityName)));
        }

        return Ok(ApiEnvelope.SuccessMessage(CommonMessages.Deleted(EntityName)));
    }
}
