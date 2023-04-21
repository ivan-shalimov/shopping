using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Shopping.Server.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async context =>
                {
                    var exceptionId = Guid.NewGuid();
                    var logger = context.RequestServices.GetRequiredService<ILogger<IHttpConnectionFeature>>();
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var connection = context.Features.Get<IHttpConnectionFeature>();

                    var scope = new Dictionary<string, object>
                    {
                        { "ExceptionId", exceptionId.ToString() },
                    };
                    using (logger.BeginScope(scope))
                    {
                        logger.LogError(exceptionFeature.Error, exceptionFeature.Error.Message);
                    }

                    var routeData = context.GetRouteData() ?? new RouteData();
                    var actionContext = new ActionContext(context, routeData, new ActionDescriptor());
                    var result = new ObjectResult(new
                    {
                        Error = exceptionId,
                        Message = "Error processing request. Server error.",
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                    };

                    var executor = context.RequestServices.GetService<IActionResultExecutor<ObjectResult>>();

                    await executor.ExecuteAsync(actionContext, result).ConfigureAwait(false);
                },
            });
        }
    }
}