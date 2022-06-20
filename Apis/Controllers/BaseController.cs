using Api.Services;
using Dtos;
using Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BaseApiController : ControllerBase
{
    //protected override void ini(HttpContextAccessor httpContextAccessor)
    //{
    //    IEnumerable<string> lang;
    //    controllerContext.Request.Headers.TryGetValues("lang", out lang);
    //    MyResource.Culture = new System.Globalization.CultureInfo(lang.FirstOrDefault());

    //    base.Initialize(controllerContext);
    //}
}