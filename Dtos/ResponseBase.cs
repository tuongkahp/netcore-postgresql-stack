using Constants.Enums;

namespace Dtos;

public class ResponseBase<T> where T : ResponseBase<T>
{
    public ResCode ResCode { get; set; } = ResCode.Failure;
    public string Description { get; set; } = ResCode.Failure.Description();
    public bool Status { get; set; } = false;
    public List<ErrorModel> Errors { get; set; }
    public object Data { get; set; }

    public T Success()
    {
        Status = true;
        ResCode = ResCode.Success;
        Description = ResCode.Success.Description();
        return (T)this;
    }

    public T Success(object data)
    {
        Data = data;
        Status = true;
        ResCode = ResCode.Success;
        Description = ResCode.Success.Description();
        return (T)this;
    }

    public T Failure()
    {
        Status = false;
        ResCode = ResCode.Failure;
        Description = ResCode.Failure.Description();
        return (T)this;
    }

    public T Failure(ResCode resCode)
    {
        Status = false;
        ResCode = resCode;
        Description = resCode.Description();
        return (T)this; 
    }

    public T Error()
    {
        Status = false;
        ResCode = ResCode.Error;
        Description = ResCode.Error.Description();
        return (T)this;
    }
}

public class ErrorModel
{
    public string Field { get; set; }
    public string Description { get; set; }
}