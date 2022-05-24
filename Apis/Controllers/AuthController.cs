using Api.Services;
using Dtos;
using Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService
        )
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public ResponseDto Register(RegisterUserDto registerUserDto)
    {
        return _authService.RegisterUser(registerUserDto);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        return Ok(_authService.Login(loginDto));
    }

    [HttpPost]
    [Route("refresh-token")]
    public IActionResult RefreshToken(LoginDto loginDto)
    {
        return Ok(_authService.Login(loginDto));
    }

    [HttpPost]
    [Route("change-password")]
    public async Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        return await _authService.ChangePassword(changePasswordDto);
    }

    [HttpGet]
    [Route("user-profile")]
    public ProfileResDto UserProfile()
    {
        return _authService.GetUserProfile();
    }
}