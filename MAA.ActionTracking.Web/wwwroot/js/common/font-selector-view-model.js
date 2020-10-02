(function (window) {
    'use strict';
    function FontSelectorViewModel(view, data) {

        //Inherit the base view model
        BaseViewModelTrimmed.call(this, view, data);
    }

    //Export globally
    window.FontSelectorViewModel = FontSelectorViewModel;

}(window));
