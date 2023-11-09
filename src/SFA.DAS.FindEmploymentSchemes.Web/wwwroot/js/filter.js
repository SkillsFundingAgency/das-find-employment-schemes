// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

const filterSchemesCheckboxSelector = '#scheme-filter-options :checkbox';
const numberOfSchemesSelector = '#number-of-schemes';
const filterParamName = 'filter';

function updateFiltersFromFragmentAndShowResults() {
    const hashParams = getHashParams();
    const filters = getFilters(hashParams);

    showHideSchemes(filters);

    return filters;
}

function NoFilters(filters) {
    return (filters.length === 0 || filters.length === 1 && filters[0] === '');
}

function getFilters(hashParams) {
    const filter = hashParams[filterParamName];
    if (filter == null)
        return [];

    return hashParams[filterParamName].split(',');
}

function showHideSchemes(filters) {

    if (NoFilters(filters)) {
        $('[data-scheme]').show();
        return;
    }

    const filterGroups = filters.reduce((result, filter) => {
            const filterGroup = filter.substr(0, filter.indexOf('--'));

            result[filterGroup] = result[filterGroup] || [];

            result[filterGroup].push(filter);

            return result;
        },
    {});

    var schemes = $('[data-scheme]').hide();

    // we _could_ just create a single selector rather than calling filter repeatedly
    Object.keys(filterGroups).forEach(function (filterGroupName) {

        const showSchemeSelector = filterGroups[filterGroupName].map(function (filter) {
            return '[data-filter-' + filter + ']';
        }).join(',');

        schemes = schemes.filter(showSchemeSelector);
    });

    schemes.show();
}

function initMobileView() {

    const menuButton = document.querySelector('.govuk-js-header-toggle');

    const mobileNav = document.getElementById('mobile-navigation');

    menuButton.addEventListener('click', function () {

        if (mobileNav.style.display === 'block') {

            mobileNav.style.display = 'none';

            menuButton.setAttribute('aria-expanded', 'false');

        }
        else {

            mobileNav.style.display = 'block';

            menuButton.setAttribute('aria-expanded', 'true');

        }

    });

    $("#filter-schemes").click(function () {

        if ($(".app-filter-layout__filter").hasClass("filters_mobile_hidden")) {

            $(".app-filter-layout__filter").removeClass("filters_mobile_hidden").addClass("mobile-filter__layout");

        }

    });

    $("#close-filter").click(function () {

        if (!$(".app-filter-layout__filter").hasClass("filters_mobile_hidden")) {

            $(".app-filter-layout__filter").addClass("filters_mobile_hidden").removeClass("mobile-filter__layout");

        }

    });

    $(".govuk-header__navigation-list-mobile-close-btn").click(function () {

        var headingButton = this;

        var dataSection = headingButton.getAttribute("data-menu-section");

        const subMenuElement = document.querySelector('.govuk-header__navigation-item[data-menu-section="' + dataSection + '"]');

        if (subMenuElement !== null && subMenuElement !== 'undefined') {

            const computedStyle = window.getComputedStyle(subMenuElement);

            const display = computedStyle.getPropertyValue('display');

            const timesSVG = headingButton.querySelector(".das-menu-item__close_section");

            const chevronSVG = headingButton.querySelector(".das-menu-item__open_section");

            if (display !== 'none') {

                subMenuElement.style.display = 'none';

                timesSVG.style.display = 'none';

                chevronSVG.style.display = '';

            }
            else
            {

                subMenuElement.style.display = 'list-item';

                timesSVG.style.display = '';

                chevronSVG.style.display = 'none';

            }

        }

    });

}

function clearFilters() {
    setHashParams([]);
}

function updateFragmentFromCheckboxes() {
    var newFilter = [];
    var newFilterStr = '';
    $(filterSchemesCheckboxSelector).each(function () {
        const $this = $(this);
        var filterId;
        if ($this.prop('checked')) {
            filterId = $this.val();
            newFilter.push(filterId);
        }
    });

    if (newFilter.length > 0) {
        newFilterStr = newFilter.join(",");
    }

    const hashParams = getHashParams();
    hashParams[filterParamName] = newFilterStr;
    setHashParams(hashParams, true);
}

// http://stackoverflow.com/questions/4197591/parsing-url-hash-fragment-identifier-with-javascript
function getHashParams() {

    var hashParams = {};
    var e,
        a = /\+/g,  // Regex for replacing addition symbol with a space
        r = /([^&;=]+)=?([^&;]*)/g,
        d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
        q = window.location.hash.substring(1);

    while (e = r.exec(q))
        hashParams[d(e[1])] = d(e[2]);

    return hashParams;
}

function setHashParams(hashParams, updateResults) {
    var fragment = "";
    $.each(hashParams, function (key, value) {
        fragment += key + '=' + value + '&';
    });

    window.location.hash = fragment.substr(0, fragment.length - 1);
}

function updateNumberOfSchemes() {
    $(numberOfSchemesSelector).html($('[data-scheme]:visible').length);
}

function onFilterBoxOpenClose() {
    if ($('#scheme-filter').hasClass('app-show-hide__section--show')) {
        $(".filter-full-width").removeClass("govuk-grid-column-two-thirds").addClass("govuk-grid-column-full");

        $("#layout-cookie-button-accept").attr("tabIndex", "10");
        $("#layout-cookie-button-reject").attr("tabIndex", "20");
        $("#layout-cookie-link").attr("tabIndex", "30");
        $("#cookie-accept-link").attr("tabIndex", "40");
        $("#cookie-accept-close-button").attr("tabIndex", "50");
        $("#cookie-reject-link").attr("tabIndex", "60");
        $("#cookie-reject-close-button").attr("tabIndex", "70");
        $("#layout-main-content-link").attr("tabIndex", "80");
        $("#header-service-link").attr("tabIndex", "90");
        $("#layout-main-content-banner-link").attr("tabIndex", "100");
        $("#filter-schemes").attr("tabIndex", "110");
    }
    else
    {
        $(".filter-full-width").removeClass("govuk-grid-column-full").addClass("govuk-grid-column-two-thirds");

        $("#layout-cookie-button-accept").attr("tabIndex", "0");
        $("#layout-cookie-button-reject").attr("tabIndex", "0");
        $("#layout-cookie-link").attr("tabIndex", "0");
        $("#cookie-accept-link").attr("tabIndex", "0");
        $("#cookie-accept-close-button").attr("tabIndex", "0");
        $("#cookie-reject-link").attr("tabIndex", "0");
        $("#cookie-reject-close-button").attr("tabIndex", "0");
        $("#layout-main-content-link").attr("tabIndex", "0");
        $("#header-service-link").attr("tabIndex", "0");
        $("#layout-main-content-banner-link").attr("tabIndex", "0");
        $("#filter-schemes").attr("tabIndex", "0");
    }
}
