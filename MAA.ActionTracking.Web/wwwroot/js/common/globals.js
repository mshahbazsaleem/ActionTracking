let Globals = {
    Config: {
        BaseAppPath: window.location.protocol + "//" + window.location.host,
        IsLeftBarCollapsed: false,
        IsLeftBarMinified: false,
        LoggedInUserPky: null,
        TenantId: null,
        Grid:null
    },
    
    Enums: {
        PopupOperationType: {
            Save: 1,
            SaveAndAddNew: 2,
            SaveAndNavigate: 3,
            Cancel: 4
        }
    },
    Labels: {
        //Localized String Lables to be populated from _Layout.cshtml
    }
};
console.log(Globals);