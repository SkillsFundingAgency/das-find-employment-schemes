using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.FindEmploymentSchemes.Web.Security
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppSecurityHeaders(
            this IApplicationBuilder app,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            string cdnUrl = configuration["cdn:url"]!;

#pragma warning disable S1075
            app.UseSecurityHeaders(policies =>
                policies.AddDefaultSecurityHeaders()
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddDefaultSrc()
                            .Self()
                            .From("*");
                        
                        builder.AddScriptSrc()
                            .Self()
                            .From("*")
                            .UnsafeEval()
                            .UnsafeInline();
                        
                        builder.AddStyleSrc()
                            .Self()
                            .From("*")
                            .UnsafeInline();
                        
                        builder.AddImgSrc()
                            .Self()
                            .From("*")
                            .Data();
                        
                        builder.AddFontSrc()
                            .Self()
                            .From("*");
                        
                        builder.AddConnectSrc()
                            .Self()
                            .From("*");
                        
                        builder.AddFrameSrc()
                            .From("*");
                        
                        builder.AddObjectSrc()
                            .From("*");
                        
                        builder.AddMediaSrc()
                            .From("*");
                        
                        builder.AddFormAction()
                            .Self()
                            .From("*");
                    })
                    .AddCustomHeader("X-Frame-Options", "ALLOW-FROM https://app.contentful.com/")
                    .AddCustomHeader("X-Permitted-Cross-Domain-Policies", "none")
                    .AddXssProtectionBlock());
#pragma warning restore S1075

            return app;
        }
    }
}