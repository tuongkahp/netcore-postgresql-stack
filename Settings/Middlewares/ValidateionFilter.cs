using Constants.Enums;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Middlewares;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray());

            var responseDto = new ResponseDto().Failure(ResCode.InputIsInvalid);

            foreach (var error in errorsInModelState)
            {
                foreach (var subError in error.Value)
                {
                    responseDto.Errors = new();
                    responseDto.Errors.Add(new ()
                    {
                        Field = error.Key,
                        Description = subError
                    });
                }
            }

            if (errorsInModelState.Count > 0)
            {
                context.Result = new ObjectResult(responseDto);
                return;
            }
        }

        await next();
    }
}