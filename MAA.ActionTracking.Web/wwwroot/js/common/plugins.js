let Plugins = {
    ColorPicker: function () {
        $("input[data-color-picker]").colorpicker();
    },
    ToolTip: function () {   //Setup tooltip
        $('[data-toggle="tooltip"]').tooltip();
    },
    NiceSelect: function ($view) {

        //Setup niceselect
        $view.find("select[data-nice-select]").niceSelect();

    },

    SearchableDropdown: function ($view) {

        //Setup select2
        $view.find("select[data-select]").select2();

    },

    NiceFontSelector: function ($view) {
        //take data-value and build something nice
        $view.find(".nice-select ul.list li").each(function () {
            var self = $(this);
            self.css("font-family", self.data("value"));
        });
    }
};