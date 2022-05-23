﻿using Dtos.Users;

namespace Dtos.Response;

public class LoginResDto : ResponseBase<LoginResDto>
{
    public Tokens Data { get; set; }

    public LoginResDto Success(Tokens tokens)
    {
        this.Data = tokens;
        return base.Success();
    }
}