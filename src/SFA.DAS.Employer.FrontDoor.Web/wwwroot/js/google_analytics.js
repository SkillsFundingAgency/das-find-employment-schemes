
(function (w, d, s, l, i) {
    if (getCookie("DASAcceptCookies")) {
        w[l] = w[l] || [];
        w[l].push({'gtm.start':
        new Date().getTime(),event:'gtm.js'});
        var f=d.getElementsByTagName(s)[0], j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';
        j.async=true;
        j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;
        f.parentNode.insertBefore(j,f);
    }
}) (window, document, 'script', 'dataLayer', 'GTM-PLHRMLT');

function getCookie(name) {
    var cookies = '; ' + document.cookie;
    var splitCookie = cookies.split('; ' + name + '=');
    if (splitCookie.length == 2) return splitCookie.pop();
}