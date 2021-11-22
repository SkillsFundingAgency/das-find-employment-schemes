// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*browsers we need to support: https://www.gov.uk/service-manual/technology/designing-for-different-browsers-and-devices*/

/*todo: we'll check for IE and let them use the non-javascript enhanced version*/

/*todo:use modules, remove globals! ts?*/

var fetchResults = true;

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
