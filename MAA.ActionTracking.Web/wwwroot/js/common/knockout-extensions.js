//Initialize datepicker

ko.bindingHandlers.datepicker = {

    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker
        var options = allBindingsAccessor().datepickerOptions || { autoClose: true, todayHighlight: true};
        $(element).datepicker(options);

        //when a user changes the date, update the view model
        ko.utils.registerEventHandler(element, "dp.change", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(moment(event.date, "MM/DD/YYYY"));
            }
        });
    },
    update: function (element, valueAccessor) {
        var widget = $(element).data("datepicker");
        //when the view model is updated, update the widget
        var value = valueAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        if (valueUnwrapped && widget) {
            setTimeout(function () {
                element.value = moment(valueUnwrapped).format("MM/DD/YYYY");
                widget.update();
            }, 0);        
        }
    }
};

//Initialize Timepicker
ko.bindingHandlers.timepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize timepicker with some optional options
        var options = allBindingsAccessor().timePickerOptions || {};
        $.extend(options, {
            icons: {
                up: "fa fa-chevron-circle-up",
                down: "fa fa-chevron-circle-down"
            }
        });
        var valueOfElement = ko.unwrap(valueAccessor());
        $(element).val(valueOfElement);
        $(element).timepicker(options);

        //when a user changes the date, update the view model
        ko.utils.registerEventHandler(element, "changeTime.timepicker", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(event.time.value);
            }
        });
    },
    update: function (element, valueAccessor) {
        var widget = $(element).data("timepicker");
        //when the view model is updated, update the widget
        if (widget) {
            var time = ko.utils.unwrapObservable(valueAccessor());
            if (time)
                $(element).val(time);
        }

        //var widget = $(element).data("timepicker");
        ////when the view model is updated, update the widget
        //if (widget) {
        //    widget.time = ko.utils.unwrapObservable(valueAccessor());
        //    if (widget.time) {
        //        widget.setValue();
        //    }
        //}
    }
};

//Custom radio button binding
ko.bindingHandlers.radio = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // event handler for the element change event
        var changeHandler = function () {
            var elementValue = $(element).val();
            var observable = valueAccessor();      // set the observable value to the boolean value of the element value
            observable($.parseJSON(elementValue));
        };    // register change handler for element
        ko.utils.registerEventHandler(element,
            "change",
            changeHandler);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var elementValue = $.parseJSON($(element).val());
        var observableValue = ko.utils.unwrapObservable(valueAccessor()); if (elementValue === observableValue) {
            element.checked = true;
        }
        else {
            element.checked = false;
        }
    }
};


ko.observable.fn.updateSilently = function (value) {
    this.notifySubscribers = function () { };
    this(value);
};

//Control CSS Visibility property instead of Displany:none which is handled through 'visible' binding by default
ko.bindingHandlers.hidden = (function () {
    function setVisibility(element, valueAccessor) {
        var hidden = ko.unwrap(valueAccessor());
        $(element).css('visibility', hidden ? 'hidden' : 'visible');
    }
    return { init: setVisibility, update: setVisibility };
})();