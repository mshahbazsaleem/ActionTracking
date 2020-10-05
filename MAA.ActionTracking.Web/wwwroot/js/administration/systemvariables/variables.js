/// <reference path="../../common/api-endpoints.js" />
/// <reference path="../../common/helpers.js" />
/// <reference path="../../common/grid-base.js" />
/// <reference path="../../common/base-view-model.js" />

(function (window) {
    'use strict';

    window.SystemVariableTable = $("#grid-system-variable");
    window.SystemVariableDialog = $("#modal-system-variable");
    window.SystemVariableViewModel = new ManageSystemVariableViewModel(SystemVariableDialog.get(0), viewModelJs);

    function SystemVariablesViewController() {
        //Inherit base grid functionality here
     
        GridBase.call(
            this,
            {
                parentId: null,
                table: SystemVariableTable,
                apiEndpoints: ApiEndpoints.SystemVariable,
                viewModelJs: viewModelJs,
                popupViewModel: SystemVariableViewModel,
                popupDialog: SystemVariableDialog,
                publishRowClickEvent: false,
                isProcessing: true,
                isServerSide: true
            });


        let vm = this;

        //Register on add event, this event gives a chance for customization
        SystemVariableTable.off("onAdd").on("onAdd", function (e) {
            vm.ShowAddScreen();
        });
    }

    //Initialize system-variable grid view by calling base class Initialize method
    (new SystemVariablesViewController()).Initialize();

}(window));