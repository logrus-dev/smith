using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Logrus.Smith.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logrus.Smith;

public static class WebApplicationExtensions
{
    public static void UseLogrusSmith(this WebApplication app)
    {
        var filter = new BasicAuthAuthorizationFilter(
            new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                SslRedirect = false,
                LoginCaseSensitive = true,
                Users =
                [
                    new BasicAuthAuthorizationUser
                    {
                        Login = app.Configuration["LogrusSmithSettings:HangfireUser"],
                        PasswordClear = app.Configuration["LogrusSmithSettings:HangfirePassword"]
                    }
                ]
            });
        var options = new DashboardOptions
        {
            Authorization = [filter],
        };
        app.UseHangfireDashboard("/hangfire", options);

        app.MapPost("/agent/{code}", async ([FromRoute] string code, [FromServices] AgentCallbackManager callbackManager, HttpRequest request) =>
        {
            var result = await callbackManager.InvokeCallback(code, request);
            return Results.Ok(result);
        });
    }
}
