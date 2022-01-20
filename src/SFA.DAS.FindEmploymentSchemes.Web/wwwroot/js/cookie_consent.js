
$(document).ready(function () {
    if (window.GOVUK.cookie("AnalyticsConsent") == "true" || window.GOVUK.cookie("AnalyticsConsent") == "false") {
        $("div.govuk-cookie-banner").hide();
    } else {
        $("button.cookie-consent-button").click(function () {
            var date = new Date();
            date.setFullYear(date.getFullYear() + 1);
            var expiry = date.toUTCString();
            if ($(this).hasClass("cookies-accept")) {
                document.cookie = "AnalyticsConsent=true; path=/; SameSite=None; secure; expires=" + expiry;
                document.cookie = "MarketingCookieConsent=true; path=/; SameSite=None; secure; expires=" + expiry;
                $("div#cookie-accept-message").show();
            } else {
                document.cookie = "AnalyticsConsent=false; path=/; SameSite=None; secure; expires=" + expiry;
                document.cookie = "MarketingCookieConsent=false; path=/; SameSite=None; secure; expires=" + expiry;
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
