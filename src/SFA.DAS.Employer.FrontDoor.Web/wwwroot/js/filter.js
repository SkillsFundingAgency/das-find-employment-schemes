// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

/*todo: we'll check for IE and let them use the non-javascript enhanced version*/

/*todo:use modules, remove globals! ts?*/
/*todo: filter-schemes as id rather than class*/
/*todo: ids and class names as constants?*/

var fetchResults = true;

function initSearch(options) {
    setDefaultFilterIfRequired(options.defaultFilter);
    updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);

    $(window).hashchange(function () {
        updateFiltersFromFragmentAndShowResults(options.resultsAjaxUrl);
    });
    initEvents();
}

function setDefaultFilterIfRequired(defaultFilter) {
    if (window.location.hash.length > 0)
        return;

    const hashParams = getHashParams();
    const filters = getFilters(hashParams);
    if (filters.length === 0) {
        hashParams['filter'] = defaultFilter;
        setHashParams(hashParams, true);
    }
}

function updateFiltersFromFragmentAndShowResults(resultsAjaxUrl) {
    updateCheckboxesFromFragment();
    if (fetchResults)
        submitFilters(resultsAjaxUrl);
    else {
        updateResults();
        fetchResults = true;
    }
}

function getFilters(hashParams) {
    const filter = hashParams['filter'];
    if (filter == null)
        return [];

    return hashParams['filter'].split(',');
}

function submitFilters(resultsAjaxUrl) {
    const filterData = getFilterAjaxData();
    showThrobber();
    $.ajax({
        url: resultsAjaxUrl,
        dataType: "html",
        type: 'GET',
        processData: false,
        data: filterData,
        success: function (htmlData) {
            $('#scheme-results').html(htmlData);
            updateNumberOfSchemes($("#number-of-schemes").val());
            updateResults();
        }
    });
}

function getFilterAjaxData() {
    return 'filter=' + getHashParams()['filter'];
}

function updateNumberOfSchemes(numberOfSchemes) {
    $("#number-of-schemes").html(numberOfSchemes);
}

function updateResults() {
    //todo: if results not already sorted, will have to sort them here
    hideThrobber();
}

function initEvents() {
    $('.filter-schemes :checkbox').click(function () {
        //todo: sync checkbox?
        updateFragmentFromCheckboxes();
    });
}

// http://stackoverflow.com/questions/4197591/parsing-url-hash-fragment-identifier-with-javascript
//function getHashParams() {

//    var hashParams = {};
//    var e,
//        a = /\+/g,  // Regex for replacing addition symbol with a space
//        r = /([^&;=]+)=?([^&;]*)/g,
//        d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
//        q = window.location.hash.substring(1);

//    while (e = r.exec(q))
//        hashParams[d(e[1])] = d(e[2]);

//    return hashParams;
//}

function getHashParams() {
    const hashParams = new URLSearchParams(window.location.hash.substr(1)); // skip the first char (#)

    return hashParams;
}

function updateFragmentFromCheckboxes() {
    // checkboxes
    var newFilter = [];
    var newFilterStr = '';
    $('.filter-schemes :checkbox').each(function () {
        const $this = $(this);
        var filterId;
        if ($this.prop('checked')) {
            filterId = $this.data('filterid');
            if ($.inArray(filterId, newFilter) === -1) {
                newFilter.push(filterId);
            }
        }
    });

    if (newFilter.length > 0) {
        newFilterStr = newFilter.join(",");
    }

    const hashParams = getHashParams();
    hashParams['filter'] = newFilterStr;
    setHashParams(hashParams, true);
}

function updateCheckboxesFromFragment() {
    var hashParams = getHashParams();
    var filters = getFilters(hashParams);

    // checkboxes
    $('.filter-schemes :checkbox').each(function () {
        const $this = $(this);
        $this.prop('checked', $.inArray($this.data('filterid'), filters) !== -1);
    });
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

/*todo: use a skeleton screen instead? */
function showThrobber() {
    $("#throbber").show();
    $("#scheme-results").hide();
}

function hideThrobber() {
    $("#throbber").hide();
    $("#scheme-results").show();
}