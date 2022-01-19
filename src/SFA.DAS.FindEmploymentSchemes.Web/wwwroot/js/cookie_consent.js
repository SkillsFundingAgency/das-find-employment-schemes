
$(document).ready(function () {
    if (window.GOVUK.cookie("AnalyticsConsent") == "true" || window.GOVUK.cookie("AnalyticsConsent") == "false") {
        $("div.govuk-cookie-banner").hide();
    } else {
        $("button.cookie-consent-button").click(function () {
            if ($(this).hasClass("cookies-accept")) {
                window.GOVUK.cookie("AnalyticsConsent", "true", { days: 365 });
                window.GOVUK.cookie("MarketingCookieConsent", "true", { days: 365 });
                $("div#cookie-accept-message").show();
            } else {
                window.GOVUK.cookie("AnalyticsConsent", "false", { days: 365 });
                window.GOVUK.cookie("MarketingCookieConsent", "false", { days: 365 });
                $("div#cookie-reject-message").show();
            }

            $("div#cookie-message").hide();
            window.scrollTo(0, 0);
        });

        $(".cookies-close").click(function () {
            $("div.govuk-cookie-banner").hide();
        });
    }
});
