/// <reference path="../../common/api-endpoints.js" />
/// <reference path="../../common/helpers.js" />
/// <reference path="../../common/grid-base.js" />
/// <reference path="../../common/base-view-model.js" />

(function (window) {
    'use strict';

    window.TenantTable = $("#grid-tenant");
    window.TenantDialog = $("#modal-tenant");
    window.TenantViewModel = new ManageTenantViewModel(TenantDialog.get(0), viewModelJs);

    function TenantsViewController() {
        //Inherit base grid functionality here
     
        GridBase.call(
            this,
            {
                parentId: null,
                table: TenantTable,
                apiEndpoints: ApiEndpoints.Tenant,
                viewModelJs: viewModelJs,
                popupViewModel: TenantViewModel,
                popupDialog: TenantDialog,
                publishRowClickEvent: false,
                isProcessing: true,
                isServerSide: true
            });


        let vm = this;

        //Register on add event, this event gives a chance for customization
        TenantTable.off("onAdd").on("onAdd", function (e) {
            vm.ShowAddScreen();
        });
    }

    //Initialize tenant grid view by calling base class Initialize method
    (new TenantsViewController()).Initialize();

}(window));