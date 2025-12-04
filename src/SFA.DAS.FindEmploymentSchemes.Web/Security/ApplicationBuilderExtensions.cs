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
                    
                    // TEMPORARY: Very permissive CSP for ad campaign testing
                    // Remove this and use proper CSP below after testing
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
                    
                    /*
                    // ORIGINAL CSP - COMMENTED OUT FOR NOW
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddUpgradeInsecureRequests();

                        var defaultSrc = builder.AddDefaultSrc()
                            .Self()
                            .From(cdnUrl);

                        var connectSrc = builder.AddConnectSrc()
                            .Self()
                            .From(new[]
                            {
                                "https://consent-api-bgzqvpmbyq-nw.a.run.app/api/v1/consent/",
                                "https://stats.g.doubleclick.net/j/collect",
                                "https://region1.google-analytics.com/g/collect",
                                "https://www.google-analytics.com",
                                "https://www.youtube-nocookie.com",
                                "*.qualtrics.com",
                                "https://dc.services.visualstudio.com/v2/track", "rt.services.visualstudio.com/v2/track",
                                "cdn.linkedin.oribi.io",
                                "*.clarity.ms",
                                "https://td.doubleclick.net",
                                "https://px.ads.linkedin.com/wa/",
                                "https://px.ads.linkedin.com/attribution_trigger",
                                "https://ib.adnxs.com/pixie/up",
                                "https://www.google.com/ccm/collect",
                                "https://connect.facebook.net",
                                "https://www.facebook.com",
                                "https://platform.linkedin.com",
                                "https://ib.adnxs-simple.com"
                            });

                        builder.AddFontSrc()
                            .Self()
                            .From(new[] { cdnUrl, "https://fonts.gstatic.com" });

                        builder.AddObjectSrc()
                            .None();

                        builder.AddFormAction()
                            .Self()
                            .From(new[]
                            {
                                "https://www.facebook.com",
                                "*.qualtrics.com",
                                "*.clarity.ms",
                                "https://td.doubleclick.net",
                                "https://platform.linkedin.com"
                            });

                        builder.AddImgSrc()
                            .OverHttps()
                            .Self()
                            .From(new[] { 
                                cdnUrl, 
                                "data:", 
                                "https://ssl.gstatic.com", 
                                "https://www.gstatic.com", 
                                "https://www.google-analytics.com",
                                "https://www.facebook.com",
                                "https://platform.linkedin.com",
                                "https://www.doubleclick.net",
                                "https://www.googleadservices.com",
                                "https://px.ads.linkedin.com"
                            });

                        var scriptSrc = builder.AddScriptSrc()
                            .Self()
                            .From(new[]
                            {
                                cdnUrl,
                                "https://tagmanager.google.com",
                                "https://www.google-analytics.com/",
                                "https://www.googletagmanager.com",
                                "https://www.googleadservices.com",
                                "https://ssl.google-analytics.com",
                                "https://googleads.g.doubleclick.net",
                                "https://acdn.adnxs.com",
                                "https://www.youtube-nocookie.com",
                                "https://www.youtube.com",
                                "https://snap.licdn.com",
                                "https://analytics.twitter.com",
                                "https://static.ads-twitter.com",
                                "https://connect.facebook.net",
                                "*.qualtrics.com",
                                "*.clarity.ms",
                                "https://td.doubleclick.net",
                                "https://platform.linkedin.com",
                                "https://www.doubleclick.net",
                                "https://scripts.clarity.ms"
                            })
                            .UnsafeEval()
                            .UnsafeInline();

                        builder.AddStyleSrc()
                            .Self()
                            .From(new[]
                            {
                                cdnUrl,
                                "https://www.googletagmanager.com",
                                "https://tagmanager.google.com",
                                "https://fonts.googleapis.com",
                                "https://platform.linkedin.com"
                            })
                            .StrictDynamic()
                            .UnsafeInline();

                        builder.AddMediaSrc()
                            .None();

                        builder.AddFrameAncestors()
                            .None();

                        builder.AddBaseUri()
                            .Self();

                        builder.AddFrameSrc()
                            .From(new[]
                            {
                                "https://www.googletagmanager.com",
                                "https://www.youtube-nocookie.com",
                                "https://2673654.fls.doubleclick.net",
                                "https://www.facebook.com",
                                "*.qualtrics.com",
                                "*.clarity.ms",
                                "https://td.doubleclick.net",
                                "https://platform.linkedin.com"
                            });

                        builder.AddFrameAncestors()
                               .From("https://app.contentful.com");

                        if (env.IsDevelopment())
                        {
                            defaultSrc.From(new[] { "http://localhost:*", "ws://localhost:*" });
                            scriptSrc.From("http://localhost:*");
                            connectSrc.From(new[] { "https://localhost:*", "ws://localhost:*", "wss://localhost:*" });
                        }
                    })
                    */
                    .AddCustomHeader("X-Frame-Options", "ALLOW-FROM https://app.contentful.com/")
                    .AddCustomHeader("X-Permitted-Cross-Domain-Policies", "none")
                    .AddXssProtectionBlock());
#pragma warning restore S1075

            return app;
        }
    }
}