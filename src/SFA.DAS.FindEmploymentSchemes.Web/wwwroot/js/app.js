function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}

var showHideElements = document.querySelectorAll('[data-module="app-show-hide"]')

nodeListForEach(showHideElements, function (showHideElement) {
    new ShowHideElement(showHideElement).init()
})