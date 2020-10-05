(function (window) {
    'use strict';
    function ManageSystemVariableViewModel(view, data) {

        //Inherit the base view model
        BaseViewModel.call(this, view, data);

    }

    //Export globally
    window.ManageSystemVariableViewModel = ManageSystemVariableViewModel;

}(window));
