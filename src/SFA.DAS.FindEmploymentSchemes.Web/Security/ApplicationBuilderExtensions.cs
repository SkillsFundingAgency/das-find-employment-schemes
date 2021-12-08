using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.FindEmploymentSchemes.Web.Security
{
    public static class ApplicationBuilderExtensions
    {
        //todo: see if we need the other google domains from the das ga doc

        /// <summary>
        /// nuget documentation
        /// https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
        /// csp introduction
        /// https://scotthelme.co.uk/content-security-policy-an-introduction/
        /// google analytics tag manager required csp
        /// https://developers.google.com/tag-platform/tag-manager/web/csp
        /// jquery csp
        /// https://content-security-policy.com/examples/jquery/
        /// das ga
        /// https://skillsfundingagency.atlassian.net/wiki/spaces/DAS/pages/3249700873/Adding+Google+Analytics
        /// </summary>
        public static IApplicationBuilder UseAppSecurityHeaders(
            this IApplicationBuilder app,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            string cdnUrl = configuration["cdn:url"];

#pragma warning disable S1075
            app.UseSecurityHeaders(policies =>
                policies.AddDefaultSecurityHeaders()
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddUpgradeInsecureRequests();
                        builder.AddBlockAllMixedContent();

                        var defaultSrc = builder.AddDefaultSrc()
                            .Self()
                            .From(cdnUrl);

                        var connectSrc = builder.AddConnectSrc()
                            .Self()
                            .From(new []
                            {
                                "https://www.google-analytics.com",
                                /* application insights*/ "https://dc.services.visualstudio.com/v2/track", "rt.services.visualstudio.com/v2/track"
                            });

                        builder.AddFontSrc()
                            .Self()
                            .From(new[] { cdnUrl, "https://fonts.gstatic.com"});

                        builder.AddObjectSrc()
                            .None();

                        builder.AddFormAction()
                            .Self();

                        builder.AddImgSrc()
                            .OverHttps()
                            .Self()
                            .From(new[] {cdnUrl, "https://ssl.gstatic.com", "https://www.gstatic.com"});

                        var scriptSrc = builder.AddScriptSrc()
                            .Self()
                            .From(new[] {cdnUrl, "https://tagmanager.google.com"})
                            // this is needed for gtm
                            .UnsafeEval()
                            .WithNonce();

                        var styleSrc = builder.AddStyleSrc()
                            .Self()
                            .From(new[] { cdnUrl, "https://tagmanager.google.com", "https://fonts.googleapis.com"})
                            .StrictDynamic();

                        builder.AddMediaSrc()
                            .None();

                        builder.AddFrameAncestors()
                            .None();

                        builder.AddBaseUri()
                            .Self();

                        builder.AddFrameSrc()
                            .From("https://www.googletagmanager.com");

                        if (env.IsDevelopment())
                        {
                            // open up for browserlink
                            defaultSrc.From(new[] {"http://localhost:*", "ws://localhost:*"});

                            scriptSrc.From("http://localhost:*");

                            styleSrc.UnsafeInline();

                            connectSrc.From(new [] { "https://localhost:*", "ws://localhost:*", "wss://localhost:*"});
                        }
                    })
                    .AddCustomHeader("X-Permitted-Cross-Domain-Policies", "none"));
#pragma warning restore S1075

            return app;
        }
    }
}