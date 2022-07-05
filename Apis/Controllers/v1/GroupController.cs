using Api.Services;
using Dtos;
using Dtos.Auth;
using Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/groups")]
public class GroupController : ControllerBase
{
    private readonly ILogger<GroupController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public GroupController(
        ILogger<GroupController> logger,
        IUnitOfWork unitOfWork,
        IUserService userService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    [HttpGet]
    public GetUsersResDto Get(int page, int count)
    {
        return _userService.GetUsers(page, count);
    }

    [HttpGet]
    [Route("{groupId?}")]
    public IActionResult Get(string groupId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }

    [HttpPost]
    public ResponseDto AddUser(CreateUserReqDto registerUserDto)
    {
        return new();
    }

    [HttpPut]
    public IActionResult EditUser(LoginDto loginDto)
    {
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteUser(LoginDto loginDto)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{groupId?}/users")]
    public IActionResult GetGroupRoles(string groupId)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{groupId?}/roles")]
    public IActionResult GetGroupUsers(string groupId)
    {
        return Ok();
    }
}