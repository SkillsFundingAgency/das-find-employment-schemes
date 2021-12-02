// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

/*todo: we'll check for IE and let them use the non-javascript enhanced version*/

/*todo:use modules, remove globals! ts?*/

/*can we safely change query params, or will we need qp/hash dual scheme?*/

const filterSchemesCheckboxSelector = '#scheme-filter-options :checkbox';
const numberOfSchemesSelector = '#number-of-schemes';
const filterParamName = 'filter';

function initFiltering(options) {
    updateFiltersFromFragmentAndShowResults();
    initEvents();
}

function updateFiltersFromFragmentAndShowResults() {
    const hashParams = getHashParams();
    const filters = getFilters(hashParams);

    updateCheckboxesFromFragment(filters);
    showHideSchemes(filters);
    updateNumberOfSchemes();
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

    if (filters.length === 0 || filters.length === 1 && filters[0] === '') {
        $('[data-scheme]').show();
        return;
    }

    const showSchemeSelector = filters.map(function (param) {
        return '[data-filter-' + param + ']';
    }).join('');

    $('[data-scheme]').hide().filter(showSchemeSelector).show();
}


function initEvents() {
    window.addEventListener('hashchange', function () {
        updateFiltersFromFragmentAndShowResults();
    });

    $(filterSchemesCheckboxSelector).click(function () {
        //todo: sync checkbox?
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
            //if ($.inArray(filterId, newFilter) === -1) {
                newFilter.push(filterId);
            //}
        }
    });

    if (newFilter.length > 0) {
        newFilterStr = newFilter.join(",");
    }

    const hashParams = getHashParams();
    hashParams[filterParamName] = newFilterStr;
    setHashParams(hashParams, true);
}

//function getHashParams() {
//    return new URLSearchParams(window.location.hash.substr(1)); // skip the first char (#)
//}

 http://stackoverflow.com/questions/4197591/parsing-url-hash-fragment-identifier-with-javascript
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

    //todo: only need to update if changed
    //fetchResults = updateResults;
    //todo: check fragment.length === 0
    window.location.hash = fragment.substr(0, fragment.length - 1);
}

function updateNumberOfSchemes() {
    $(numberOfSchemesSelector).html($('[data-scheme]:visible').length);
}
