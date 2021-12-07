using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.FindEmploymentSchemes.Web.Security
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppSecurityHeaders(this IApplicationBuilder app)//, IConfiguration configuration)
        {
            app.UseSecurityHeaders();
            return app;
        }
    }
}