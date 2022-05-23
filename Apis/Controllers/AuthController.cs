using Api.Services;
using Dtos;
using Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManagementService _userManagementService;

    public AuthController(
        ILogger<UserController> logger,
        IUnitOfWork unitOfWork,
        IUserManagementService userManagementService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManagementService = userManagementService;
    }

    [HttpPost]
    [Route("register")]
    public ResponseDto Register(RegisterUserDto registerUserDto)
    {
        return _userManagementService.RegisterUser(registerUserDto);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        return Ok(_userManagementService.Login(loginDto));
    }

    [HttpPost]
    [Route("refresh-token")]
    public IActionResult RefreshToken(LoginDto loginDto)
    {
        return Ok(_userManagementService.Login(loginDto));
    }

    [HttpPost]
    [Route("change-password")]
    public IActionResult ChangePassword(LoginDto loginDto)
    {
        return Ok(_userManagementService.Login(loginDto));
    }
}