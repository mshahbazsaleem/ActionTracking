/// <reference path="../../common/api-endpoints.js" />
/// <reference path="../../common/helpers.js" />
/// <reference path="../../common/grid-base.js" />
/// <reference path="../../common/base-view-model.js" />

(function (window) {
    'use strict';

    window.TestVariableTable = $("#grid-test-variable");
    window.TestVariableDialog = $("#modal-test-variable");
    window.TestVariableViewModel = new ManageTestVariableViewModel(TestVariableDialog.get(0), viewModelJs);

    function TestVariablesViewController() {
        //Inherit base grid functionality here
     
        GridBase.call(
            this,
            {
                parentId: null,
                table: TestVariableTable,
                apiEndpoints: ApiEndpoints.TestVariable,
                viewModelJs: viewModelJs,
                popupViewModel: TestVariableViewModel,
                popupDialog: TestVariableDialog,
                publishRowClickEvent: false,
                isProcessing: true,
                isServerSide: true
            });


        let vm = this;

        //Register on add event, this event gives a chance for customization
        TestVariableTable.off("onAdd").on("onAdd", function (e) {
            vm.ShowAddScreen();
        });
    }

    //Initialize system-variable grid view by calling base class Initialize method
    (new TestVariablesViewController()).Initialize();

}(window));