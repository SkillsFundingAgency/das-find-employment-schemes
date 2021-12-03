// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

const filterSchemesCheckboxSelector = '#scheme-filter-options :checkbox';
const numberOfSchemesSelector = '#number-of-schemes';
const filterParamName = 'filter';

function initFiltering(options) {
    // we need to ensure that when the page is displayed:
    //  * from a bookmark/copy & pasted link
    //  * traversing through history
    // that the correct filters are ticked, the schemes are filtered correctly, and the filter panel is displayed if any filters are selected
    const filters = updateFiltersFromFragmentAndShowResults();
    initEvents();

    // if the scheme's are filtered, then ensure the filter panel is displayed
    if (!NoFilters(filters)) {
        $('#scheme-filter').removeClass('app-show-hide__section--show');
        const schemeFilterShowHide = showHideEls['scheme-filter'];
        schemeFilterShowHide.showHideTarget(schemeFilterShowHide);
    }
}

function updateFiltersFromFragmentAndShowResults() {
    const hashParams = getHashParams();
    const filters = getFilters(hashParams);

    updateCheckboxesFromFragment(filters);
    showHideSchemes(filters);
    updateNumberOfSchemes();

    return filters;
}

function NoFilters(filters) {
    return (filters.length === 0 || filters.length === 1 && filters[0] === '');
}

function updateCheckboxesFromFragment(filters) {

    $(filterSchemesCheckboxSelector).each(function () {
        const $this = $(this);
        $this.prop('checked', $.inArray($this.val(), filters) !== -1);
    });
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

function initEvents() {
    window.addEventListener('hashchange', function () {
        updateFiltersFromFragmentAndShowResults();
    });

    $(filterSchemesCheckboxSelector).click(function () {
        updateFragmentFromCheckboxes();
    });

    $('#clear-filters').click(function() {
        clearFilters();
        return false;
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
