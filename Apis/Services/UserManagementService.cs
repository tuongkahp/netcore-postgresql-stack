using Apis.Settings;
using AutoMapper;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Response;
using Dtos.Users;
using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public interface IUserManagementService
{
    ResponseDto RegisterUser(RegisterUserDto registerUserDto);
    LoginResDto Login(LoginDto loginDto);
}

public class UserManagementService : IUserManagementService
{
    private readonly ILogger<UserManagementService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public UserManagementService(
        ILogger<UserManagementService> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    public ResponseDto RegisterUser(RegisterUserDto registerUserDto)
    {
        var res = new ResponseDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.UserName == registerUserDto.UserName).FirstOrDefault();

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
            var user = _unitOfWork.Users.GetBy(x => x.UserName == loginDto.UserName).FirstOrDefault();

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

    private Tokens Authenticate(User users)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, users.FullName)
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshTokenDes = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var refreshToken = tokenHandler.CreateToken(refreshTokenDes);

        return new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = tokenHandler.WriteToken(refreshToken) };
    }
}