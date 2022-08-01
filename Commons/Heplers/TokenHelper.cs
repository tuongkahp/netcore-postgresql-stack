using Constants.Constants;
using Dtos.Auth;
using Dtos.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helpers;

public static class TokenHelper
{
    public static TokenInfo GetTokenInfo(this IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            var token = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString() ?? "";

            if (string.IsNullOrEmpty(token))
            {
                //Log.Logger.Information("\n\tCannot get token");
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token.ToString().Replace("Bearer ", ""));

            return new TokenInfo()
            {
                Token = token,
                UserId = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type.Equals(ConstJwtCode.UserId))?.Value.ToString() ?? "0"),
                FullName = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type.Equals(ConstJwtCode.FullName))?.Value.ToString() ?? "",
                ListRoles = jwtSecurityToken.Claims.Where(x => x.Type.Equals(ConstJwtCode.Role)).Select(x => x.Value).ToList()
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static TokenInfo GetRefreshTokenInfo(string token, string key)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            //ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return new TokenInfo()
        {
            UserId = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type.Equals(ConstJwtCode.UserId))?.Value.ToString() ?? "0"),
        };
    }
}