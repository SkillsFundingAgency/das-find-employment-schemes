
$(document).ready(function () {
    if (window.GOVUK.cookie("DASAcceptCookies") == "true" || window.GOVUK.cookie("DASAcceptCookies") == "false") {
        $("div.govuk-cookie-banner").hide();
    } else {
        $("button.cookie-consent-button").click(function () {
            if ($(this).hasClass("cookies-accept")) {
                window.GOVUK.cookie("DASAcceptCookies", "true", { days: 365 });
                $("div#cookie-accept-message").show();
            } else {
                window.GOVUK.cookie("DASAcceptCookies", "false", { days: 365 });
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
