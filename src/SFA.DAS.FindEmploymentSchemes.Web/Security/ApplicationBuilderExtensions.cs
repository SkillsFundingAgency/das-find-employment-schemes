using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.FindEmploymentSchemes.Web.Security
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppSecurityHeaders(this IApplicationBuilder app)//, IConfiguration configuration)
        {
            // https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
            // https://scotthelme.co.uk/content-security-policy-an-introduction/
            app.UseSecurityHeaders(policies =>
                policies.AddDefaultSecurityHeaders()
                    //todo: don't leave as report only!
                    .AddContentSecurityPolicyReportOnly(builder =>
                    {
                    }));
            return app;
        }
    }
}