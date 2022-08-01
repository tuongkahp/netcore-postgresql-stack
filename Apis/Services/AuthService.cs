using Apis.Settings;
using AutoMapper;
using Commons.Helpers;
using Constants.Constants;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public interface IAuthService
{
    ResponseDto RegisterUser(RegisterUserDto registerUserDto);
    Task<ResponseDto> Login(LoginDto loginDto);
    Task<ResponseDto> RefeshToken(string refreshToken);
    Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto);
    ProfileResDto GetUserProfile();
}

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        ILogger<AuthService> logger,
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

    public ResponseDto RegisterUser(RegisterUserDto registerUserDto)
    {
        var res = new ResponseDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.Username == registerUserDto.Username).FirstOrDefault();

            if (user != null)
            {
                return res.Failure();
            }

            user = _mapper.Map<User>(registerUserDto);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = PasswordHelper.GenerateHash(registerUserDto.Password, user.SecurityStamp);
            _unitOfWork.Users.Add(user);
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }

    public async Task<ResponseDto> Login(LoginDto loginDto)
    {
        var res = new ResponseDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.Username == loginDto.Username).FirstOrDefault();

            if (user == null
                || !PasswordHelper.Equals(loginDto.Password, user.PasswordHash, user.SecurityStamp))
            {
                return res.Failure(ResCode.UserNameOrPasswordIsWrong);
            }

            var token = await Authenticate(user);

            return res.Success(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }

    public async Task<ResponseDto> RefeshToken(string refreshToken)
    {
        var res = new ResponseDto();

        try
        {
            var refreshTokenInfo = TokenHelper.GetRefreshTokenInfo(refreshToken, _jwtSettings.RefreshKey);
            var user = _unitOfWork.Users.GetBy(x => x.UserId == refreshTokenInfo.UserId).FirstOrDefault();

            if (user == null)
                return res.Failure(ResCode.UserInvalid);

            var userToken = _unitOfWork.UserTokens.GetBy(x => x.RefreshToken == refreshToken).FirstOrDefault();

            if (userToken == null)
                return res.Failure(ResCode.UserInvalid);

            var accessToken = CreateAccessToken(user);
            await _unitOfWork.SaveChangeAsync();

            return res.Success(new { AccessToken = accessToken });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RefeshToken err");
            return res.Error();
        }
    }

    public async Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var res = new ResponseDto();

        try
        {
            var tokenInfo = _httpContextAccessor.GetTokenInfo();
            var user = _unitOfWork.Users.GetById(tokenInfo.UserId);

            if (!PasswordHelper.Equals(changePasswordDto.OldPassword, user.PasswordHash, user.SecurityStamp))
            {
                return res.Failure(ResCode.OldPasswordIsWrong);
            }

            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = PasswordHelper.GenerateHash(changePasswordDto.NewPassword, user.SecurityStamp);
            await _unitOfWork.SaveChangeAsync();
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ChangePassword err");
            return res.Error();
        }
    }

    public ProfileResDto GetUserProfile()
    {
        var res = new ProfileResDto();

        try
        {
            var tokenInfo = _httpContextAccessor.GetTokenInfo();
            var user = _unitOfWork.Users.GetById(tokenInfo.UserId);

            res.Data = new()
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                ListRoles = _unitOfWork.Users.GetRoles(user.UserId).Select(x => x.RoleName).ToList()
            };

            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetProfile err");
            return res.Error();
        }
    }

    private async Task<Tokens> Authenticate(User user)
    {
        var accessToken = CreateAccessToken(user);
        var refreshToken = CreateRefreshToken(user);

        var token = new UserToken()
        {
            RefreshToken = refreshToken,
            UserId = user.UserId,
        };

        var userToken = _unitOfWork.UserTokens.GetBy(x => x.UserTokenId == user.UserId).FirstOrDefault();

        if (userToken == null)
            await _unitOfWork.UserTokens.AddAsync(token);
        else
        {
            userToken.RefreshToken = refreshToken;
            await _unitOfWork.SaveChangeAsync();
        }

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string CreateAccessToken(User user)
    {
        var lstRoles = _unitOfWork.Users.GetRoles(user.UserId);

        var lstClaims = lstRoles.Select(x => new Claim(ClaimTypes.Role, x.RoleName)).ToList();
        lstClaims.Add(new Claim(ConstJwtCode.UserId, user.UserId.ToString()));
        lstClaims.Add(new Claim(ClaimTypes.Name, user.FullName));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(lstClaims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpiredTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(accessToken);
    }

    private string CreateRefreshToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var refreshTokenDes = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>() { new Claim(ConstJwtCode.UserId, user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshExpiredTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshKey)), SecurityAlgorithms.HmacSha256Signature)
        };

        var refreshToken = tokenHandler.CreateToken(refreshTokenDes);
        return tokenHandler.WriteToken(refreshToken);
    }
}