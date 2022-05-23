using Api.Services;
using Dtos;
using Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupController : ControllerBase
{
    private readonly ILogger<GroupController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManagementService _userManagementService;

    public GroupController(
        ILogger<GroupController> logger,
        IUnitOfWork unitOfWork,
        IUserManagementService userManagementService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManagementService = userManagementService;
    }

    [HttpGet]
    public IActionResult Get(string page, string count)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }

    [HttpGet]
    [Route("{groupId?}")]
    public IActionResult Get(string groupId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }

    [HttpPost]
    public ResponseDto AddUser(RegisterUserDto registerUserDto)
    {
        return _userManagementService.RegisterUser(registerUserDto);
    }

    [HttpPut]
    public IActionResult EditUser(LoginDto loginDto)
    {
        return Ok(_userManagementService.Login(loginDto));
    }

    [HttpDelete]
    public IActionResult DeleteUser(LoginDto loginDto)
    {
        return Ok(_userManagementService.Login(loginDto));
    }

    [HttpGet]
    [Route("{groupId?}/users")]
    public IActionResult GetGroupRoles(string groupId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }

    [HttpGet]
    [Route("{groupId?}/roles")]
    public IActionResult GetGroupUsers(string groupId)
    {
        return Ok(new ResponseDto().Success(_unitOfWork.Users.GetAll().ToList()));
    }
}