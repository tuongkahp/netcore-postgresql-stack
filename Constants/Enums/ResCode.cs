namespace Constants.Enums;

public enum ResCode
{
    Success = 1,
    Error = -2,
    Failure = -1,

    InputIsInvalid = -10,
    UserNameOrPasswordIsWrong = -11,
    OldPasswordIsWrong = -12,

    UserNotFound = -50,
    UserInvalid = -51,
}