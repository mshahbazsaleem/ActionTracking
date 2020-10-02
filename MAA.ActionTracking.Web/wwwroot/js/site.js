

$(document).ready(function () {
    "use strict";

    /*======== SCROLLBAR SIDEBAR ========*/
    $(".sidebar-scrollbar")
        .slimScroll({
            opacity: 0,
            height: "100%",
            color: "#808080",
            size: "5px",
            wheelStep: 10
        })
        .mouseover(function () {
            $(this)
                .next(".slimScrollBar")
                .css("opacity", 0.5);
        });

    /*======== BACKDROP ========*/
    if ($(window).width() < 768) {
        var shadowClass = $(".mobile-sticky-body-overlay");
        $(".sidebar-toggle").on("click", function () {
            shadowClass.addClass("active");
            $("body").css("overflow", "hidden");
        });

        $(".mobile-sticky-body-overlay").on("click", function (e) {
            $(this).removeClass("active");
            $("#body").removeClass("sidebar-minified").addClass("sidebar-minified-out");
            $("body").css("overflow", "auto");
        });
    }

    /*======== SIDEBAR MENU ========*/
    $(".sidebar .nav > .has-sub > a").click(function () {
        $(this).parent().siblings().removeClass('expand');
        $(this).parent().toggleClass('expand');
    });

    $(".sidebar .nav > .has-sub .has-sub > a").click(function () {
        $(this).parent().toggleClass('expand');
    });


    /*======== SIDEBAR TOGGLE FOR MOBILE ========*/
    if ($(window).width() < 768) {
        $(document).on("click", ".sidebar-toggle", function (e) {
            e.preventDefault();
            var min = "sidebar-minified",
                min_out = "sidebar-minified-out",
                body = "#body";
            $(body).hasClass(min)
                ? $(body)
                    .removeClass(min)
                    .addClass(min_out)
                : $(body)
                    .addClass(min)
                    .removeClass(min_out);
        });
    }

    /*======== SIDEBAR TOGGLE FOR VARIOUS SIDEBAR LAYOUT ========*/
    var body = $("#body");
    if ($(window).width() >= 768) {
        //window.isMinified = false;
        //window.isCollapsed = false;

        $("#sidebar-toggler").on("click", function () {
            if (
                body.hasClass("sidebar-fixed-offcanvas") ||
                body.hasClass("sidebar-static-offcanvas")
            ) {
                $(this)
                    .addClass("sidebar-offcanvas-toggle")
                    .removeClass("sidebar-toggle");
                if (Globals.Config.IsLeftBarCollapsed === false) {
                    body.addClass("sidebar-collapse");
                    Globals.Config.IsLeftBarCollapsed = true;
                    Globals.Config.IsLeftBarMinified = false;
                    //window.isCollapsed = true;
                    //window.isMinified = false;
                } else {
                    body.removeClass("sidebar-collapse");
                    body.addClass("sidebar-collapse-out");
                    setTimeout(function () {
                        body.removeClass("sidebar-collapse-out");
                    }, 300);
                    Globals.Config.IsLeftBarCollapsed = false;
                }
            }

            if (
                body.hasClass("sidebar-fixed") ||
                body.hasClass("sidebar-static")
            ) {
                $(this)
                    .addClass("sidebar-toggle")
                    .removeClass("sidebar-offcanvas-toggle");
                if (Globals.Config.IsLeftBarMinified === false) {
                    body
                        .removeClass("sidebar-collapse sidebar-minified-out")
                        .addClass("sidebar-minified");
                    Globals.Config.IsLeftBarMinified = true;
                    Globals.Config.IsLeftBarCollapsed = false;
                } else {
                    body.removeClass("sidebar-minified");
                    body.addClass("sidebar-minified-out");
                    Globals.Config.IsLeftBarMinified = false;
                }
            }
        });
    }

    if ($(window).width() >= 768 && $(window).width() < 992) {
        if (
            body.hasClass("sidebar-fixed") ||
            body.hasClass("sidebar-static")
        ) {
            body
                .removeClass("sidebar-collapse sidebar-minified-out")
                .addClass("sidebar-minified");
            Globals.Config.IsLeftBarMinified = true;
        }
    }


    /*======== TOOLTIPS AND POPOVER ========*/
    $('[data-toggle="tooltip"]').tooltip({
        container: "body",
        template:
            '<div class="tooltip" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
    });
    $('[data-toggle="popover"]').popover();

});
var cUser = document.getElementById("currentUser");
if (cUser !== null) {
    var myUChart = new Chart(cUser, {
        type: "bar",
        data: {
            labels: [
                "1h",
                "10 h",
                "50 m",
                "30 m",
                "40 m",
                "11 h",
                "30 m",
                "25 m",
                "50 m",
                "5 h",
                "40 m"
            ],
            datasets: [
                {
                    label: "Users",
                    data: [15, 13, 7, 3, 9, 5, 12, 5, 13, 8, 9],
                    backgroundColor: "#4c84ff"
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                xAxes: [
                    {
                        gridLines: {
                            drawBorder: true,
                            display: false,
                        },
                        ticks: {
                            fontColor: "#8a909d",
                            fontFamily: "Roboto, sans-serif",
                            display: false, // hide main x-axis line
                            beginAtZero: true,
                            callback: function (tick, index, array) {
                                return index % 2 ? "" : tick;
                            }
                        },
                        barPercentage: 1.8,
                        categoryPercentage: 0.2
                    }
                ],
                yAxes: [
                    {
                        gridLines: {
                            drawBorder: true,
                            display: true,
                            color: "#eee",
                            zeroLineColor: "#eee"
                        },
                        ticks: {
                            fontColor: "#8a909d",
                            fontFamily: "Roboto, sans-serif",
                            display: true,
                            beginAtZero: true
                        }
                    }
                ]
            },

            tooltips: {
                mode: "index",
                titleFontColor: "#888",
                bodyFontColor: "#555",
                titleFontSize: 12,
                bodyFontSize: 15,
                backgroundColor: "rgba(256,256,256,0.95)",
                displayColors: true,
                xPadding: 10,
                yPadding: 7,
                borderColor: "rgba(220, 220, 220, 0.9)",
                borderWidth: 2,
                caretSize: 6,
                caretPadding: 5
            }
        }
    });
}


var AsyncLoaderViewModel = {
    GetLoader: function () {
        return $("#loader");
    }
};

window.SetupColorPicker = function () {
    //Setup color picker
    $("input[data-color-picker]").colorpicker();
};

window.ResetLocalStorage = function () {
    localStorage.clear();
};

$(document).ready(function () {

    AsyncLoaderViewModel.GetLoader().show();

    SetupColorPicker();
    //Setup tooltip
    $('[data-toggle="tooltip"]').tooltip();

    $('#sidebar-menu li').each(function () {
        let el = $(this);
        let content = el.data("title");
        el.tooltipster({
            animation: 'fade',
            delay: 200,
            theme: 'tooltipster-borderless',
            side: 'right',
            maxWidth: 350,
            content: content
        });
    });

    $("#roles").click(function (e) {
        e.stopImmediatePropagation();
    });

    $("input[type=checkbox]").focus(function () {
        $(this).parent().find(".control-indicator").addClass("focus");
    }).blur(function () {
        $(this).parent().find(".control-indicator").removeClass("focus");
    });

    SetupLogoutConfirmation();

    SetupCalendarClickable();

    $(".sidebar-right .sidepanel-close").off("click").on("click", window.HideRightPanel);

});

function SetupCalendarClickable() {
    $(".input-group-append").on("click", function () {
        let inputElement = $(this).parent().find("input");
        inputElement.focus();
        inputElement.click();
    });
}

function SetupLogoutConfirmation() {

    let confirmModal = $("#confirm-logout").modal("hide");

    confirmModal.find(".btn-yes").click(function (e) {
        confirmModal.modal("hide");
        AsyncLoaderViewModel.GetLoader().show();
        ResetLocalStorage();
        WebApiClient.Get(ApiEndpoints.Users.Logout, {}, function (data) {
            window.location.href = $(".btn-logout").attr("href");
        });
    });

    $(".btn-logout").click(function (e) {
        e.preventDefault();
        confirmModal.modal("show");
    });
}

$(window).on('load', function () {
    let loader = AsyncLoaderViewModel.GetLoader();
    setTimeout(function () { loader.hide(); }, 50);

});
// Add Horizontal Tabs to jquery
(function ($) {

    $.fn.horizontalTabs = function () {
        // Variable creation
        var $elem = $(this),
            widthOfReducedList = $elem.find('.nav-tabs-horizontal').width(),
            widthOfList = 0,
            currentPos = 0,
            adjustScroll = function () {
                widthOfList = 0;
                $elem.find('.nav-tabs-horizontal li').each(function (index, item) {
                    widthOfList += $(item).width();
                });

                widthAvailale = $elem.width();

                if (widthOfList > widthAvailale) {
                    $elem.find('.scroller').show();
                    updateArrowStyle(currentPos);
                    widthOfReducedList = $elem.find('.nav-tabs-horizontal').width();
                } else {
                    $elem.find('.scroller').hide();
                }
            },
            scrollLeft = function () {
                $elem.find('.nav-tabs-horizontal').animate({
                    scrollLeft: currentPos - widthOfReducedList
                }, 500);

                if (currentPos - widthOfReducedList > 0) {
                    currentPos -= widthOfReducedList;
                } else {
                    currentPos = 0;
                }
            },
            scrollRight = function () {
                $elem.find('.nav-tabs-horizontal').animate({
                    scrollLeft: currentPos + widthOfReducedList
                }, 500);

                if ((currentPos + widthOfReducedList) < (widthOfList - widthOfReducedList)) {
                    currentPos += widthOfReducedList;
                } else {
                    currentPos = (widthOfList - widthOfReducedList);
                }
            },
            manualScroll = function () {
                currentPos = $elem.find('.nav-tabs-horizontal').scrollLeft();

                updateArrowStyle(currentPos);
            },
            updateArrowStyle = function (position) {
                if (position >= (widthOfList - widthOfReducedList)) {
                    $elem.find('.arrow-right').addClass('disabled');
                } else {
                    $elem.find('.arrow-right').removeClass('disabled');
                }

                if (position <= 0) {
                    $elem.find('.arrow-left').addClass('disabled');
                } else {
                    $elem.find('.arrow-left').removeClass('disabled');
                };
            };

        // Event binding
        $(window).resize(function () {
            adjustScroll();
        });

        $elem.find('.arrow-left').on('click.horizontalTabs', function () {
            scrollLeft();
        });

        $elem.find('.arrow-right').on('click.horizontalTabs', function () {
            scrollRight();
        });

        $elem.find('.nav-tabs-horizontal').scroll(function () {
            manualScroll();

        });

        // Initial Call
        adjustScroll();

        return this;
    };

}(window.jQuery));


function GetKey(e) {
    e = e || window.event;
    let isEnter = false;
    let isEscape = false;
    let isBackspace = false;
    let isDelete = false;

    if ("key" in e) {
        isBackspace = e.key === "Backspace";
    } else {
        isBackspace = e.keyCode === 8;
    }

    if ("key" in e) {
        isEnter = e.key === "Enter" || e.key === "Ent";
    } else {
        isEnter = e.keyCode === 13;
    }

    if ("key" in e) {
        isEscape = e.key === "Escape" || e.key === "Esc";
    } else {
        isEscape = e.keyCode === 27;
    }

    if ("key" in e) {
        isDelete = e.key === "Delete";
    } else {
        isDelete = e.keyCode === 46;
    }

    if (isEnter) return "enter";
    if (isEscape) return "escape";
    if (isBackspace) return "backspace";
    if (isDelete) return "delete";

    return "";
}






