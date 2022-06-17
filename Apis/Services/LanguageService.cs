using Apis.Settings;
using AutoMapper;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Auth;
using Dtos.Users;
using Helpers;
using Microsoft.Extensions.Options;
using Repositories;

namespace Api.Services;

public interface ILanguageService
{
    GetUsersResDto GetLanguageConfig(string languageCode);
    GetUsersResDto GetTranslations(string languageCode);
}

public class LanguageService : ILanguageService
{
    private readonly ILogger<UserService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public UserService(
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings
        )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    public GetUsersResDto GetUsers(int page = 1, int count = 10)
    {
        var res = new GetUsersResDto();

        try
        {
            var qUser = _unitOfWork.Users.GetAll();

            res.Data = new()
            {
                Total = qUser.Count(),
                Users = qUser.Skip((page - 1) * count).Take(count).Select(x => _mapper.Map<UserDto>(x)).ToList()
            };

            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }

    public GetUsersDetailResDto GetUserDetail(long userId)
    {
        var res = new GetUsersDetailResDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.UserId == userId).FirstOrDefault();

            if (user == null)
                return res.Failure(ResCode.OldPasswordIsWrong);

            res.Data = _mapper.Map<UserDto>(user);
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }
}