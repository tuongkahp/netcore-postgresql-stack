using Apis.Settings;
using AutoMapper;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Auth;
using Dtos.Languages;
using Dtos.Users;
using Helpers;
using Microsoft.Extensions.Options;
using Repositories;
using System.Text.Json;

namespace Api.Services;

public interface ILanguageService
{
    //GetLanguageConfigResDto GetLanguageConfig(string languageCode);
    Dictionary<string, object> GetTranslations(string languageCode);
}

public class LanguageService : ILanguageService
{
    private readonly ILogger<LanguageService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public LanguageService(
        ILogger<LanguageService> logger,
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

    //public GetUsersResDto GetLanguageConfig(string languageCode)
    //{
    //    var res = new GetUsersResDto();
    //    var dictionary = new Dictionary<string, string>();
    //    JsonSerializer.Serialize(res);

    //    try
    //    {
    //        var qUser = _unitOfWork.Users.GetAll();

    //        res.Data = new()
    //        {
    //            Total = qUser.Count(),
    //            Users = qUser.Skip((page - 1) * count).Take(count).Select(x => _mapper.Map<UserDto>(x)).ToList()
    //        };

    //        return res.Success();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "RegisterUser err");
    //        return res.Error();
    //    }
    //}

    public Dictionary<string, object> GetTranslations(string languageCode)
    {
        var dictionary = new Dictionary<string, object>();

        try
        {
            // Get from cache
            // TODO
            dictionary = _unitOfWork.Translations
                .GetBy(x => x.TranslationCode == languageCode.ToLower())
                .GroupBy(x => x.NameSpace)
                .ToDictionary(x => x.Key ?? x.FirstOrDefault().TranslationCode, x => (object));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }
}