function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}

var showHideElements = document.querySelectorAll('[data-module="app-show-hide"]')

var showHideEls = {};

nodeListForEach(showHideElements, function (showHideElement) {
    var showHideEl = new ShowHideElement(showHideElement);
    showHideEl.init();

    if (showHideElement.hasAttribute('id')) {
        showHideEls[showHideElement.id] = showHideEl;
    }
})

var tag = document.createElement('script');
tag.id = 'iframe-demo';
tag.src = 'https://www.youtube.com/iframe_api';
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var player;
function onYouTubeIframeAPIReady() {
    player = new YT.Player('existing-iframe-example', {
        events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange
        }
    });
}
function onPlayerReady(event) {
    $("#existing-iframe-example").contents().find(".ytp-title-channel-logo").attr('aria-label', 'proof');
    var xxx = $("#existing-iframe-example").contents().find(".ytp-title-channel-logo");
    var yyy = $("#existing-iframe-example").contents();
    yyy.html('The new body.');
    //$(".ytp-title-channel-logo").attr('aria-label', 'proof');
    document.getElementById('existing-iframe-example').style.borderColor = '#FF6D00';
}
function changeBorderColor(playerStatus) {
    var color;
    if (playerStatus == -1) {
        color = "#37474F"; // unstarted = gray
    } else if (playerStatus == 0) {
        color = "#FFFF00"; // ended = yellow
    } else if (playerStatus == 1) {
        color = "#33691E"; // playing = green
    } else if (playerStatus == 2) {
        color = "#DD2C00"; // paused = red
    } else if (playerStatus == 3) {
        color = "#AA00FF"; // buffering = purple
    } else if (playerStatus == 5) {
        color = "#FF6DOO"; // video cued = orange
    }
    if (color) {
        document.getElementById('existing-iframe-example').style.borderColor = color;
    }
}
function onPlayerStateChange(event) {
    changeBorderColor(event.data);
}
