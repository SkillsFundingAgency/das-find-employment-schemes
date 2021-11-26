
function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}

function ShowHideElement (module) {
    this.module = module
    this.id = this.module.id
    this.sectionExpandedClass = 'app-show-hide__section--show'
}

ShowHideElement.prototype.init = function () {
    var that = this
    var sectionExpanded = this.isExpanded()

    if (!this.id) {
        return
    }

    this.showHideLinks = document.querySelectorAll("a[href='#" + this.id + "']")

    if (this.showHideLinks.length === 0) {
        return
    }

    this.module.classList.add('app-show-hide')

    nodeListForEach(this.showHideLinks, function (showHideLink) {
        showHideLink.setAttribute('aria-controls', that.id)
        showHideLink.setAttribute('aria-expanded', sectionExpanded)
        showHideLink.addEventListener('click', that.showHideTarget.bind(that))
    })
}

ShowHideElement.prototype.showHideTarget = function (e) {
    var sectionExpanded = this.isExpanded()
    if (sectionExpanded) {
        this.module.classList.remove(this.sectionExpandedClass)
    } else {
        this.module.classList.add(this.sectionExpandedClass)
        this.module.focus()
    }
    nodeListForEach(this.showHideLinks, function (showHideLink) {
        var showText = showHideLink.getAttribute('data-text-show')
        var hideText = showHideLink.getAttribute('data-text-hide')
        showHideLink.innerHTML = (sectionExpanded ? showText : hideText)
        showHideLink.setAttribute('aria-expanded', !sectionExpanded)
    })
    e.preventDefault()
}

ShowHideElement.prototype.isExpanded = function () {
    return this.module.classList.contains(this.sectionExpandedClass)
}
