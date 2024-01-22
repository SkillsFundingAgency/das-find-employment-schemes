document.addEventListener("DOMContentLoaded", function () {

    InitializeInterimContentLinks();

});

function InitializeInterimContentLinks() {

    $("[data-type='interim-content-section'] ul li").click(function () {

        var anchor = $(this).find("a");

            if (anchor.length > 0) {

                var targetId = anchor.attr("href");

                console.log(targetId);

                scrollToElement(targetId);

            }

        }

    );

}

function scrollToElement(targetId) {

    var targetElement = $("[data-id='" + targetId + "']");

    if (targetElement.length > 0) {

        $('html, body').animate({

            scrollTop: targetElement.offset().top

        }, 1000); // Adjust the duration as needed

    }

}