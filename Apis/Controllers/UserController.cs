using Api.Services;
using Dtos;
using Dtos.Auth;
using Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public UserController(
        ILogger<UserController> logger,
        IUnitOfWork unitOfWork,
        IUserService userManagementService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userService = userManagementService;
    }

    [HttpGet]
    public GetUsersResDto GetUsers(int page, int count)
    {
        return _userService.GetUsers(page, count);
    }

    [HttpGet]
    [Route("{userId?}")]
    public IActionResult GetUserDetail(string userId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }

    [HttpPost]
    public ResponseDto AddUser(RegisterUserDto registerUserDto)
    {
        return new ResponseDto();
    }

    [HttpPut]
    [Route("{userId?}")]
    public IActionResult EditUser([FromBody] LoginDto loginDto, [FromQuery] long userId)
    {
        return Ok();
    }

    [HttpDelete]
    [Route("{userId?}")]
    public IActionResult DeleteUser([FromBody] LoginDto loginDto, [FromQuery] long userId)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{userId?}/roles")]
    public IActionResult GetUserRoles(string userId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }
}