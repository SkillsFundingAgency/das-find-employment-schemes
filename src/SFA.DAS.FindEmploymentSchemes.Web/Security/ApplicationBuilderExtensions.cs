﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.FindEmploymentSchemes.Web.Security
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
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
        ///
        /// Note: we _may_ need the other google domains from the das ga doc,
        /// but there were no violations reported without them, so we leave them out for now
        ///
        /// Allowing unsafe-inline scripts
        /// ------------------------------
        /// Google's nonce-aware tag manager code has an issue with custom html tags (which we use).
        /// https://stackoverflow.com/questions/65100704/gtm-not-propagating-nonce-to-custom-html-tags
        /// https://dev.to/matijamrkaic/using-google-tag-manager-with-a-content-security-policy-9ai
        ///
        /// We tried the given solution (above), but the last piece of the puzzle to make it work,
        /// would involve self hosting a modified version of google's gtm.js script.
        ///
        /// In gtm.js, where it's creating customScripts, we'd have to change...
        /// var n = C.createElement("script");
        /// to
        /// var n=C.createElement("script");n.nonce=[get nonce from containing script block];
        ///
        /// The problems with self hosting a modified gtm.js are (from https://stackoverflow.com/questions/45615612/is-it-possible-to-host-google-tag-managers-script-locally)
        /// * we wouldn't automatically pick up any new tags or triggers that Steve added
        /// * we would need a version of the script that worked across all browsers and versions (and wouldn't have a browser optimised version)
        /// * we wouldn't pick up new versions of the script
        /// For these reasons, the only way to get the campaign tracking working, is to open up the CSP to allow unsafe-inline scripts.
        /// This will make our site less secure, but is a trade-off between security and tracking functionality.
        /// </summary>
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
                                /* application insights*/ "https://dc.services.visualstudio.com/v2/track", "rt.services.visualstudio.com/v2/track",
                                "cdn.linkedin.oribi.io",
                                "*.clarity.ms",
                                "https://td.doubleclick.net",
                                "https://px.ads.linkedin.com/wa/",
                                "https://px.ads.linkedin.com/attribution_trigger",
                                "https://ib.adnxs.com/pixie/up",
                                "https://www.google.com/ccm/collect"
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
                                "https://td.doubleclick.net"
                            });

                        builder.AddImgSrc()
                            .OverHttps()
                            .Self()
                            .From(new[] { cdnUrl, "data:", "https://ssl.gstatic.com", "https://www.gstatic.com", "https://www.google-analytics.com" });

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
                                "https://td.doubleclick.net"
                            })
                            // this is needed for GTM and YouTube embedding
                            .UnsafeEval()
                            .UnsafeInline();
                        // if we wanted the nonce back, we'd add `.WithNonce();` here

                        builder.AddStyleSrc()
                            .Self()
                            .From(new[]
                            {
                                cdnUrl,
                                "https://www.googletagmanager.com",
                                "https://tagmanager.google.com",
                                "https://fonts.googleapis.com"
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
                                "https://td.doubleclick.net"
                            });

                        // Add frame-ancestors directive allowing embedding from specific domain(s)
                        builder.AddFrameAncestors()
                               .From("https://app.contentful.com");

                        if (env.IsDevelopment())
                        {
                            // open up for browserlink
                            defaultSrc.From(new[] { "http://localhost:*", "ws://localhost:*" });

                            scriptSrc.From("http://localhost:*");

                            connectSrc.From(new[] { "https://localhost:*", "ws://localhost:*", "wss://localhost:*" });
                        }
                    })
                    .AddCustomHeader("X-Frame-Options", "ALLOW-FROM https://app.contentful.com/")
                    .AddCustomHeader("X-Permitted-Cross-Domain-Policies", "none")

                    // this is called in AddDefaultSecurityHeaders(), but without this, we get AddXssProtectionDisabled() instead
                    .AddXssProtectionBlock());
#pragma warning restore S1075

            return app;
        }
    }
}
