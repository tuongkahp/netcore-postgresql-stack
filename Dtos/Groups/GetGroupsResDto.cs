namespace Dtos.Groups;

public class GetGroupsResDto : ResponseBase<GetGroupsResDto>
{
    public List<GetGroupsDataResDto> Data { get; set; }
}

public class GetGroupsDataResDto
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public bool IsActived { get; set; }
}