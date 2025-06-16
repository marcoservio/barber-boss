using BarberBoss.Communication.Responses;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberBoss.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BarberBossException barberBossException)
            HandleProjectException(context, barberBossException);
        else
            ThrowUnknowError(context);
    }

    private static void HandleProjectException(ExceptionContext context, BarberBossException barberBossException)
    {
        context.HttpContext.Response.StatusCode = barberBossException!.StatusCode;
        context.Result = new ObjectResult(new ResponseErrorJson(barberBossException.GetErrors()));
    }

    private static void ThrowUnknowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR));
    }
}
