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

public interface IUserService
{
    GetUsersResDto GetUsers(int page = 1, int count = 10);
    GetUsersDetailResDto GetUserDetail(long userId);
    ResponseDto CreateUser(CreateUserReqDto createUserReqDto);
    ResponseDto UpdateUser(UpdateUserReqDto updateUserReqDto);
}

public class UserService : IUserService
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

            var group = _unitOfWork.Groups.GetByUser(userId);
            var lstRoles = _unitOfWork.Users.GetRoles(user.UserId);

            //var lstRolesFromGroup = _unitOfWork.Roles.GetByGroup(group.GroupId);
            //var lstRolesFromUser = _unitOfWork.Roles.GetByUser(userId);

            res.Data = new()
            {
                User = _mapper.Map<UserDto>(user),
                Roles = _mapper.Map<List<RoleDto>>(lstRoles),
                Group = _mapper.Map<GroupDto>(group)
            };

            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterUser err");
            return res.Error();
        }
    }

    public ResponseDto CreateUser(CreateUserReqDto createUserReqDto)
    {
        var res = new ResponseDto();

        try
        {
            if (_unitOfWork.Users.GetBy(x => x.Username == createUserReqDto.Username).Any())
            {
                return res.Failure(ResCode.UserNameIsExist);
            }

            var user = _mapper.Map<User>(createUserReqDto);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = PasswordHelper.GenerateHash(createUserReqDto.Password, user.SecurityStamp);
            _unitOfWork.Users.Add(user);
            _unitOfWork.Users.UpdateRoles(user.UserId, createUserReqDto.RoleIds);
            _unitOfWork.Users.UpdateGroups(user.UserId, createUserReqDto.GroupIds);
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser err");
            return res.Error();
        }
    }

    public ResponseDto UpdateUser(UpdateUserReqDto updateUserReqDto)
    {
        var res = new ResponseDto();

        try
        {
            var user = _unitOfWork.Users.GetBy(x => x.UserId == updateUserReqDto.UserId).FirstOrDefault();

            if (user == null)
            {
                return res.Failure(ResCode.UserNotFound);
            }

            user = _mapper.Map<User>(updateUserReqDto);

            if (!string.IsNullOrEmpty(updateUserReqDto.Password))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = PasswordHelper.GenerateHash(updateUserReqDto.Password, user.SecurityStamp);
            }

            _unitOfWork.Users.Add(user);
            _unitOfWork.Users.UpdateRoles(user.UserId, updateUserReqDto.RoleIds);
            _unitOfWork.Users.UpdateGroups(user.UserId, updateUserReqDto.GroupIds);
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser err");
            return res.Error();
        }
    }
}