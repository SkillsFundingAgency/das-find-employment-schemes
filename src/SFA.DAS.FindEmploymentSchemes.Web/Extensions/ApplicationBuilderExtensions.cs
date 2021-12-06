
using Joonasw.AspNetCore.SecurityHeaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace SFA.DAS.FindEmploymentSchemes.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                if (!string.IsNullOrEmpty(context.Session?.Id))
                    context.Response.Headers.Add("X-STAX-SessionId", context.Session.Id);

                await next();
            })
                .UseCsp(csp =>
                {
                    csp.ByDefaultAllow.FromSelf();

                    csp.AllowScripts
                        .FromSelf()
                        .AllowUnsafeInline()
                        .AllowUnsafeEval()
                        .From("code.jquery.com")
                        .From("cdn.jsdelivr.net")
                        .From("cdnjs.cloudflare.com");

                    csp.AllowStyles.FromSelf()
                        .AllowUnsafeInline()
                        .From("fonts.googleapis.com")
                        .From("code.jquery.com")
                        .From("cdn.jsdelivr.net")
                        .From("cdnjs.cloudflare.com");

                    csp.AllowImages.FromSelf()
                        .DataScheme();

                    csp.AllowFonts.FromSelf()
                        .From("fonts.gstatic.com")
                        .From("cdn.jsdelivr.net");

                    csp.AllowConnections
                        .ToSelf();

                    csp.AllowImages
                       .FromSelf();

                    csp.AllowFrames
                        .FromSelf()
                        .From("googletagmanager.com");

                    csp.AllowPlugins
                        .FromNowhere();

                    csp.AllowFraming
                        .FromNowhere();
                });

            return app;
        }
    }
}