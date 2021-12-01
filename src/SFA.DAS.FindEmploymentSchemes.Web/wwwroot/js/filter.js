// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

/*todo: we'll check for IE and let them use the non-javascript enhanced version*/

/*todo:use modules, remove globals! ts?*/

/*todo:just filter in the front end according to data attributes on scheme sections - no ajax!*/

/*can we safely change query params, or will we need qp/hash dual scheme?*/

const filterSchemesCheckboxSelector = '#scheme-filter-options :checkbox';
const filterParamName = 'filter';

//todo: create filter from checkbox val array
//$('[data-scheme]').hide().filter('[data-filter-pay-unpaid][data-filter-scheme-length-a-year-or-more]').show();

function initFiltering(options) {
    //setDefaultFilterIfRequired(options.defaultFilter);
    //updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);

    //todo: should we be using pushState instead?
    //$(window).on('hashchange', function () {
    //    updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);
    //});
    initEvents();
}

function initEvents() {
    $(filterSchemesCheckboxSelector).click(function () {
        //todo: sync checkbox?
        updateFragmentFromCheckboxes();
    });
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

var fetchResults = true;
function setHashParams(hashParams, updateResults) {
    var fragment = "";
    $.each(hashParams, function (key, value) {
        fragment += key + '=' + value + '&';
    });

    //todo: only need to update if changed
    fetchResults = updateResults;
    //todo: check fragment.length === 0
    window.location.hash = fragment.substr(0, fragment.length - 1);
}


//var fetchResults = true;
//const filterParamName = 'filter';
//const filterIdDataName = 'filterid';
//const schemeResultsSectionIdSelector = '#scheme-results';
//const throbberIdSelector = '#throbber';
//const numberOfSchemesAjaxIdSelector = '#number-of-schemes';
//const numberOfSchemesIdSelector = '#number-of-schemes';

//function initSearch(options) {
//    setDefaultFilterIfRequired(options.defaultFilter);
//    updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);

//    //todo: should we be using pushState instead?
//    $(window).on('hashchange', function () {
//        updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);
//    });
//    initEvents();
//}

//function setDefaultFilterIfRequired(defaultFilter) {
//    if (window.location.hash.length > 0)
//        return;

//    const hashParams = getHashParams();
//    const filters = getFilters(hashParams);
//    if (filters.length === 0) {
//        hashParams[filterParamName] = defaultFilter;
//        setHashParams(hashParams, true);
//    }
//}

//function updateFiltersFromFragmentAndShowResults(resultsAjaxUrl) {
//    updateCheckboxesFromFragment();
//    if (fetchResults)
//        submitFilters(resultsAjaxUrl);
//    else {
//        updateResults();
//        fetchResults = true;
//    }
//}

//function getFilters(hashParams) {
//    const filter = hashParams[filterParamName];
//    if (filter == null)
//        return [];

//    return hashParams[filterParamName].split(',');
//}

//function submitFilters(resultsAjaxUrl) {
//    const filterData = getFilterAjaxData();
//    showThrobber();
//    $.ajax({
//        url: resultsAjaxUrl,
//        dataType: 'html',
//        type: 'GET',
//        processData: false,
//        data: filterData,
//        success: function (htmlData) {
//            $(schemeResultsSectionIdSelector).html(htmlData);
//            updateNumberOfSchemes($(numberOfSchemesAjaxIdSelector).val());
//            updateResults();
//        }
//    });
//}

//function getFilterAjaxData() {
//    return filterParamName + '=' + getHashParams()[filterParamName];
//}

//function updateNumberOfSchemes(numberOfSchemes) {
//    $(numberOfSchemesIdSelector).html(numberOfSchemes);
//}

//function updateResults() {
//    //todo: if results not already sorted, will have to sort them here
//    hideThrobber();
//}

//function initEvents() {
//    $(filterSchemesCheckboxSelector).click(function () {
//        //todo: sync checkbox?
//        updateFragmentFromCheckboxes();
//    });
//}

//// http://stackoverflow.com/questions/4197591/parsing-url-hash-fragment-identifier-with-javascript
////function getHashParams() {

////    var hashParams = {};
////    var e,
////        a = /\+/g,  // Regex for replacing addition symbol with a space
////        r = /([^&;=]+)=?([^&;]*)/g,
////        d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
////        q = window.location.hash.substring(1);

////    while (e = r.exec(q))
////        hashParams[d(e[1])] = d(e[2]);

////    return hashParams;
////}

//function getHashParams() {
//    const hashParams = new URLSearchParams(window.location.hash.substr(1)); // skip the first char (#)

//    return hashParams;
//}

//function updateFragmentFromCheckboxes() {
//    // checkboxes
//    var newFilter = [];
//    var newFilterStr = '';
//    $(filterSchemesCheckboxSelector).each(function () {
//        const $this = $(this);
//        var filterId;
//        if ($this.prop('checked')) {
//            filterId = $this.data(filterIdDataName);
//            if ($.inArray(filterId, newFilter) === -1) {
//                newFilter.push(filterId);
//            }
//        }
//    });

//    if (newFilter.length > 0) {
//        newFilterStr = newFilter.join(",");
//    }

//    const hashParams = getHashParams();
//    hashParams[filterParamName] = newFilterStr;
//    setHashParams(hashParams, true);
//}

//function updateCheckboxesFromFragment() {
//    var hashParams = getHashParams();
//    var filters = getFilters(hashParams);

//    // checkboxes
//    $(filterSchemesCheckboxSelector).each(function () {
//        const $this = $(this);
//        $this.prop('checked', $.inArray($this.data(filterIdDataName), filters) !== -1);
//    });
//}

//var fetchResults = true;
//function setHashParams(hashParams, updateResults) {
//    var fragment = "";
//    $.each(hashParams, function (key, value) {
//        fragment += key + '=' + value + '&';
//    });

//    //todo: only need to update if changed
//    fetchResults = updateResults;
//    //todo: check fragment.length === 0
//    window.location.hash = fragment.substr(0, fragment.length - 1);
//}

///*todo: use a skeleton screen instead? */
//function showThrobber() {
//    $(throbberIdSelector).show();
//    $(schemeResultsSectionIdSelector).hide();
//}

//function hideThrobber() {
//    $(throbberIdSelector).hide();
//    $(schemeResultsSectionIdSelector).show();
//}