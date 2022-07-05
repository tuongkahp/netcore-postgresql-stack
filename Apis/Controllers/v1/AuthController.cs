using Api.Services;
using Dtos;
using Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/auth")]
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
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        return Ok(await _authService.Login(loginDto));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(Tokens tokens)
    {
        var refreshResult = await _authService.RefeshToken(tokens.RefreshToken);

        if (refreshResult.ResCode != Constants.Enums.ResCode.Success)
            return Unauthorized(refreshResult);

        return Ok(refreshResult);
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