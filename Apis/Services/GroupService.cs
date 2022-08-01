using Apis.Settings;
using AutoMapper;
using Constants.Enums;
using Datas.Entities;
using Dtos;
using Dtos.Auth;
using Dtos.Users;
using Commons.Helpers;
using Microsoft.Extensions.Options;
using Repositories;
using Dtos.Groups;

namespace Api.Services;

public interface IGroupService
{
    //GetUsersResDto GetUsers(int page = 1, int count = 10);
    //GetUsersDetailResDto GetUserDetail(long userId);
    //ResponseDto CreateUser(CreateUserReqDto createUserReqDto);
    //ResponseDto UpdateUser(UpdateUserReqDto updateUserReqDto);
    GetGroupsResDto GetGroups();
}

public class GroupService : IGroupService
{
    private readonly ILogger<GroupService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public GroupService(
        ILogger<GroupService> logger,
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

    public GetGroupsResDto GetGroups()
    {
        var res = new GetGroupsResDto();

        try
        {
            res.Data = _unitOfWork.Groups.GetAll().Select(x => _mapper.Map<GetGroupsDataResDto>(x)).ToList();
            return res.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetGroups err");
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