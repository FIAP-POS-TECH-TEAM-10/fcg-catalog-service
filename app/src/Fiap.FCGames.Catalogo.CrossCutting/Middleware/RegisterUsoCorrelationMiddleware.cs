using Microsoft.AspNetCore.Builder;
using Serilog.Context;

namespace Fiap.FCGames.Catalogo.CrossCutting.Middleware
{
    public static class RegisterUsoCorrelationMiddleware
    {
        public static void UseCorrelationId(this WebApplication application)
        {
            application.Use(async (context, next) =>
            {
                var correlationId = context.Request.Headers["x-correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

                context.Response.Headers["x-correlation-ID"] = correlationId;

                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    await next();
                }

            });
        }
    }
}
