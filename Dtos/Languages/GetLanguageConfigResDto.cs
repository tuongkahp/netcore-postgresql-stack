namespace Dtos.Languages;

public class GetLanguageConfigResDto : ResponseBase<GetLanguageConfigResDto>
{
    public LanguageDto Data { get; set; }
}