(function (window) {
    'use strict';
    function ManageTestVariableViewModel(view, data) {

        //Inherit the base view model
        BaseViewModel.call(this, view, data);

    }

    //Export globally
    window.ManageTestVariableViewModel = ManageTestVariableViewModel;

}(window));
