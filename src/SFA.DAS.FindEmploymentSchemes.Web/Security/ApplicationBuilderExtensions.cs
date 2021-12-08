using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
        public static IApplicationBuilder UseAppSecurityHeaders(this IApplicationBuilder app,
            IConfiguration configuration)
        {
            string cdnUrl = configuration["cdn:url"];

#pragma warning disable S1075
            app.UseSecurityHeaders(policies =>
                policies.AddDefaultSecurityHeaders()
                    //todo: don't leave as report only!
                    .AddContentSecurityPolicyReportOnly(builder =>
                    {
                        builder.AddUpgradeInsecureRequests();
                        builder.AddBlockAllMixedContent();

                        builder.AddDefaultSrc()
                            .Self()
                            .From(cdnUrl);

                        builder.AddConnectSrc()
                            .Self();

                        builder.AddFontSrc()
                            .Self()
                            .From("https://fonts.gstatic.com")
                            .Data();

                        builder.AddObjectSrc()
                            .None();

                        builder.AddFormAction()
                            .Self();

                        builder.AddImgSrc()
                            .OverHttps()
                            .Self()
                            .From(new[] {cdnUrl, "https://ssl.gstatic.com", "https://www.gstatic.com"});

                        builder.AddScriptSrc()
                            .Self()
                            .From(new[] {cdnUrl, "https://tagmanager.google.com"})
                            .UnsafeInline()
                            // think we need this for ga tm
                            .UnsafeEval()
                            .ReportSample()
                            .WithNonce();

                        builder.AddStyleSrc()
                            .Self()
                            .From(new[] {"https://tagmanager.google.com", "https://fonts.googleapis.com"})
                            .StrictDynamic();

                        builder.AddMediaSrc()
                            .None();
                        //.OverHttps();

                        // required for ga?
                        builder.AddFrameAncestors()
                            .None();

                        builder.AddBaseUri()
                            .Self();

                        //builder.AddFrameSrc()
                        //    .From("http://testUrl.com");

                    }));
#pragma warning restore S1075

            return app;
        }
    }
}