(function (window) {
    'use strict';
    function ManageTenantViewModel(view, data) {

        //Inherit the base view model
        BaseViewModel.call(this, view, data);
    }

    //Export globally
    window.ManageTenantViewModel = ManageTenantViewModel;

}(window));
