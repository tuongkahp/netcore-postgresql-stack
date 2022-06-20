using Api.Services;
using Dtos;
using Dtos.Auth;
using Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Api.Controllers;

[ApiController]
[Route("api/languages")]
public class LanguageController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public LanguageController(
        ILogger<UserController> logger,
        IUnitOfWork unitOfWork,
        IUserService userManagementService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userService = userManagementService;
    }

    //[HttpGet]
    //[Route("{languageCode?}")]
    //public GetUsersDetailResDto Get(string languageCode)
    //{
    //    return _userService.GetUserDetail(userId);
    //}
}