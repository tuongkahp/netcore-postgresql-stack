using Apis.Settings;
using AutoMapper;
using Constants.Constants;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Auth;
using Helpers;
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
    LoginResDto Login(LoginDto loginDto);
    LoginResDto RefeshToken();
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

    public LoginResDto Login(LoginDto loginDto)
    {
        var res = new LoginResDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.Username == loginDto.Username).FirstOrDefault();

            if (user == null
                || !PasswordHelper.Equals(loginDto.Password, user.PasswordHash, user.SecurityStamp))
            {
                return res.Failure(ResCode.UserNameOrPasswordIsWrong);
            }

            var token = Authenticate(user);

            return res.Success(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }

    public LoginResDto RefeshToken()
    {
        var res = new LoginResDto();

        try
        {
            var tokenInfo = _httpContextAccessor.GetTokenInfo();
            var user = _unitOfWork.Users.GetBy(x => x.UserId == tokenInfo.UserId).FirstOrDefault();

            if (user == null)
            {
                return res.Failure(ResCode.UserNameOrPasswordIsWrong);
            }

            var token = Authenticate(user);
            token.RefreshToken = tokenInfo.Token;

            return res.Success(token);
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

    private Tokens Authenticate(User user)
    {
        var lstRoles = _unitOfWork.Users.GetRoles(user.UserId);

        var lstClaims = lstRoles.Select(x => new Claim(ClaimTypes.Role, x.RoleName)).ToList();
        lstClaims.Add(new Claim(ConstJwtCode.UserId, user.UserId.ToString()));
        lstClaims.Add(new Claim(ClaimTypes.Name, user.FullName));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(lstClaims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshTokenDes = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>() { new Claim(ConstJwtCode.UserId, user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var refreshToken = tokenHandler.CreateToken(refreshTokenDes);

        return new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = tokenHandler.WriteToken(refreshToken) };
    }
}