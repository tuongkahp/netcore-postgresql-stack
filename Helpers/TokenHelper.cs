using Constants.Constants;
using Dtos.Auth;
using Dtos.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

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

    //private static TokenInfo DecodeJwtToken(string token)
    //{
    //    try
    //    {
          

            
    //    }
    //    catch (Exception ex)
    //    {
    //        //Log.Logger.Error($"\n\tGet token info err: {ex.Message} - {ex.StackTrace}");
    //        return null;
    //    }
    //}
}