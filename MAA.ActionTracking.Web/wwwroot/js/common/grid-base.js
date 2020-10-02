(function (window) {
    'use strict';
    /**
     * /
     * @param {any} gridParams
     *  parentId,
        method, //Remove, method will always be post
        table,
        apiEndpoints,
        viewModelJs,
        popupViewModel,
        popupDialog,
        publishRowClickEvent,
        paramterFunction,
        isServerSide, || false//Remove/default to server side
        isProcessing, || false//Remove/defaual to true
        additionalValuesFunction, || null
        dataSrc
     */

    function GridBase(gridParams) {
        //  $.fn.dataTable.ext.errMode = 'none';

        var vm = this;

        this.MergeParentId = gridParams.mergeParentId || false;//TODO: ParentId is to be set for posted data instead of a query parameter, make this default and remove the extra paramter once all grids are updated.
        this.ParentId = gridParams.parentId || null;
        this.Method = gridParams.method || "POST";
        this.Table = gridParams.table;
        this.Id = gridParams.table.attr("id");
        this.TableWrapper = $("#" + this.Id + "-wrapper");
        //this.SearchPlaceHolderText = this.GetSearchPlaceholderText();
        this.Grid = null;
        this.Columns = null;
        this.ColumnDefinations = null;
        this.ApiEndpoints = gridParams.apiEndpoints;
        this.controller = gridParams.viewModelJs.controller.toLowerCase() || gridParams.table.attr("id").replace("grid-", "");
        this.Pky = gridParams.viewModelJs.idField;
        this.PopupViewModel = gridParams.popupViewModel || null;
        this.PopupDialog = gridParams.popupDialog || null;
        this.DeleteConfirm = $("#confirm-delete");
        this.ActionConfirm = $("#confirm-action");
        this.PublishRowClickEvent = gridParams.publishRowClickEvent;
        this.ReorderColumn = this.Table.data("reorder-column") || "";
        this.HasReorder = this.ReorderColumn !== "";
        this.HasActions = this.Table.data("has-actions") || false;
        this.HasSelect = this.Table.data("has-selection") || false;
        this.HasMultiSelect = this.Table.data("has-multi-select") || false;
        this.HasSelectionControl = this.Table.data("has-selection-control") || false;
        this.HasSearch = this.Table.data("has-search") || false;
        this.HasAdd = this.Table.data("has-add") || false;
        this.HasCopy = this.Table.data("has-copy") || false;
        this.HasEdit = this.Table.data("has-edit") || false;
        this.HasDelete = this.Table.data("has-delete") || false;
        this.HasPrint = this.Table.data("has-print") || false;
        this.HasExports = this.Table.data("has-exports") || false; //For export dropdown
        this.HasExport = this.Table.data("has-export") || false;
        this.HasExportAction = this.Table.data("has-export-action") || false;
        this.ExportText = this.Table.data("export-text") || "Export";
        this.HasColumnVisibility = this.Table.data("has-column-visibility") || false;
        this.AddText = this.Table.data("add-text") || "Add";
        this.CopyText = this.Table.data("copy-text") || "Copy";
        this.DeleteText = this.Table.data("delete-text") || "Delete";
        this.EditText = this.Table.data("edit-text") || "Edit";
        this.YesText = this.Table.data("yes-text") || "Yes";
        this.NoText = this.Table.data("no-text") || "No";
        this.DeleteButton = this.DeleteConfirm.find(".btn-yes");
        this.CancelButton = this.DeleteConfirm.find(".btn-no");
        this.ParameterFunction = gridParams.paramterFunction || null;
        this.Spinner = null;//Displayed on buttons
        this.ButtonText = "";
        this.Icon = "";
        this.DataEvent = this.Table.data("event") || null;
        this.DataParamObject = null;//To send post request instead of get
        this.IsServerSide = gridParams.isServerSide || true;
        this.IsProcessing = gridParams.isProcessing || true;
        //Displayed on table load/refresh
        this.TableLoader = $('<div class="overlay-loader" style="position:absolute!important;background-color:#fff;" id="table-loader"><div class="sk-fading-circle"><div class="sk-circle1 sk-circle"></div><div class="sk-circle2 sk-circle"></div><div class="sk-circle3 sk-circle"></div><div class="sk-circle4 sk-circle"></div><div class="sk-circle5 sk-circle"></div><div class="sk-circle6 sk-circle"></div><div class="sk-circle7 sk-circle"></div><div class="sk-circle8 sk-circle"></div><div class="sk-circle9 sk-circle"></div><div class="sk-circle10 sk-circle"></div><div class="sk-circle11 sk-circle"></div><div class="sk-circle12 sk-circle"></div></div></div>');
        this.AdditionalValuesFunction = gridParams.additionalValuesFunction || null;
        this.SearchTextBox = null;
        this.DataSrc = gridParams.dataSrc || "data";
        this.GridFooterSelector = gridParams.gridFooterSelector || "#grid-footer";
        this.PageLength = gridParams.pageLength || 10;
        this.AllowTextBoxForPaging = gridParams.allowTextBoxForPaging || true;
        this.ExportedFileName = gridParams.exportedFileName || "";
        this.GridTitle = this.Table.data("grid-title");
        this.DefaultSorting = gridParams.defaultSorting || null;
        this.MoveActionsToSubNav = gridParams.moveActionsToSubNav || false;

        this.InitializeColumns = function () {

            if (vm.HasReorder) {

                if (this.Table.find("thead tr th:eq(0)").data("column") === vm.Pky) {
                    this.Table.find("thead tr th:eq(0), tfoot tr th:eq(0)").remove();
                }

                let dragHandle = "<th data-column='" + vm.Pky + "'></th>";

                //Insert drag column into table html
                this.Table.find("thead tr").prepend(dragHandle);

                if (this.Table.find("tfoot") !== null) {
                    this.Table.find("tfoot tr").prepend(dragHandle);
                }
            }

            //Read columns from th data-column attributes
            vm.Columns = _.map(vm.Table.find("thead th"), function (column) {
                return { "data": $(column).data('column') };

            });

            if (vm.HasReorder)
                vm.Columns[0].width = "22px";
        };
        this.GetSearchPlaceholderText = function () {
            if (this.Table.data("search-placeholder")) { //If explicitly provided, show that
                return this.Table.data("search-placeholder");
            }
            else if (this.Table.data("text-search-column")) {//If text based search is configuerd, use the property name to show as placeholder
                return Helpers.Parse.PascalToWords(this.Table.data("text-search-column"));
            }
            else {
                return "Search...";
            }
        };
        this.GetExportedFileName = function () {
            if (vm.ExportedFileName && vm.ExportedFileName !== "") {
                return vm.GridTitle + " - " + vm.ExportedFileName;
            }
            else {
                return $(document).attr('title');
            }
        };
        this.InitializeColumnDefinitions = function () {

            let columns = vm.Table.find("thead th");

            vm.ColumnDefinations =
                _.chain(columns)
                    .map(function (column, index) {

                        let $col = $(column);
                        let isVisible = $col.data("visible");
                        let isSortable = $col.data("sortable");
                        let isActionColumn = $col.data("column-action");
                        let format = $col.data("format") || "";
                        let iconActive = $col.data("icon-active") || "";
                        let iconInActive = $col.data("icon-inactive") || "";
                        let activeClass = $col.data("active-class") || "";
                        let inactiveClass = $col.data("inactive-class") || "";
                        let activeTitle = $col.data("active-title") || "";
                        let inactiveTitle = $col.data("inactive-title") || "";
                        let dataEvent = $col.data("event") || null;

                        return {
                            visible: isVisible,
                            orderable: isActionColumn === true ? false : isSortable,
                            render: function (data, type, row) {
                                return vm.ColumnRenderer(column, data, format, row, isActionColumn, index, iconActive, iconInActive, activeClass, inactiveClass, activeTitle, inactiveTitle, dataEvent);
                            },
                            targets: index
                        };
                    })
                    .reject(function (def) {
                        return def === undefined || def === null;
                    })
                    .value();

            //TODO: simplify the whole selection block
            if (vm.HasSelect && vm.HasSelectionControl && !vm.HasReorder) {
                //DefaultSorting = DefaultSorting ? [[1, "asc"]] : DefaultSorting;

                if (vm.HasMultiSelect) {
                    vm.ColumnDefinations[0] = {
                        orderable: false,
                        checkboxes: {
                            selectRow: true//,
                            //selectAllRender: '<label class="control control-checkbox outlined"><input type="checkbox" name="select-all-recipient" id="select-all-recipient"><div class="control-indicator"></div></label>'
                        },
                        targets: 0
                    };
                }
                else {
                    vm.ColumnDefinations[0] = {
                        orderable: false,
                        render: function (data, type, row) {
                            return '<label class="control control-radio outlined"><input type="radio" name="' + vm.Id + '-select"><div class="control-indicator"></div></label><input type="hidden" value="' + data + '"/>';
                        },
                        targets: 0
                    };
                }

            }
            else {
                //vm.ColumnDefinations[0].orderable = false;
                if (vm.HasSelectionControl || vm.HasReorder) {
                    vm.ColumnDefinations[0].orderable = false;
                }
                if (vm.HasSelect && vm.HasSelectionControl && vm.HasReorder) {

                    vm.ColumnDefinations[1] = {
                        orderable: true,
                        className: '',
                        //render: function (data, type, row) {
                        //    if (vm.HasMultiSelect)
                        //        return '<label class="control control-checkbox outlined"><input type="checkbox" name="' + vm.Id + '-select"><div class="control-indicator"></div></label><input type="hidden" value="' + data + '"/>';
                        //    else
                        //        return '<label class="control control-radio outlined"><input type="radio" name="' + vm.Id + '-select"><div class="control-indicator"></div></label><input type="hidden" value="' + data + '"/>';
                        //},
                        targets: 1
                    };
                }
            }
        };

        this.ColumnRenderer = function (column, data, format, row, isActionColumn, index, iconActive, iconInActive, activeClass, inactiveClass, activeTitle, inactiveTitle, dataEvent) {

            let copyButton, editButton, deleteButton, exportButton, actionButtons = "";

            if (format === "check" && s(data).isBlank())
                data = false;

            if (format === "progress" && s(data).isBlank()) {
                return '<label id="' + row[vm.Pky] + '" class="progress-label"></label><div class="progress"><div class="progress-bar bg-success" style="width:100%"></div></div>';
            }

            if (vm.HasReorder && index === 0) {
                return "<i class='mdi mdi-drag'></i>";
            }
            if (this.HasEdit) {
                editButton = "<li class='dropdown-item'><a href='#' class='edit edit-" + vm.controller + "' data-id='" + row[vm.Pky] + "'><i class='mdi mdi-pencil'></i> " + vm.EditText + "</a></li>" //"<a href='#' class='edit edit-" + vm.controller + "' data-id='" + row[vm.Pky] + "' data-toggle='tooltip' data-placement='top' data-container='body' title='" + vm.EditText + "'><i class='fa fa-w-12 fa-edit'></i></a>";
                actionButtons += editButton;
            }
            if (this.HasExportAction) {
                exportButton = "<li class='dropdown-item'><a href='#' class='export export-" + vm.controller + "' data-id='" + row[vm.Pky] + "'><i class='mdi mdi-export'></i> " + vm.ExportText + "</a></li>";
                actionButtons += exportButton;
            }
            if (this.HasCopy) {
                copyButton = "<li class='dropdown-item'><a href='#' class='copy copy-" + vm.controller + "' data-id='" + row[vm.Pky] + "'><i class='mdi mdi-content-duplicate'></i> " + vm.CopyText + "</a></li>";  //"<a href='#' class='text-secondary copy copy-" + vm.controller + "' data-id='" + row[vm.Pky] + "' data-toggle='tooltip' data-placement='top' data-container='body' title='" + vm.CopyText + "'><i class='fa fa-w-12 fa-copy'></i></a>";
                actionButtons += copyButton;
            }

            if (this.HasDelete) {
                deleteButton = "<li class='dropdown-item'><a href='#' class='delete delete-" + vm.controller + "' data-id='" + row[vm.Pky] + "'><i class='mdi mdi-delete'></i> " + vm.DeleteText + "</a></li>" //"<a href='#' class='delete delete-" + vm.controller + "' data-id='" + row[vm.Pky] + "' data-toggle='tooltip' data-placement='top' data-container='body' title='" + vm.DeleteText + "'><i class='fa fa-w-12 fa-trash'></i></a>";
                actionButtons += deleteButton;
            }

            if (isActionColumn) {
                //return actionButtons;
                return '<div class="dropdown d-inline-block widget-dropdown action-dropdown"><a class="dropdown-toggle icon-burger-mini" href="#" role="button" id="dropdown-units" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-display="static"></a><ul class="dropdown-menu dropdown-menu-custom" aria-labelledby="dropdown-units">' + actionButtons + '</ul></div>';
            }
            else if (data === true || data === false) {
                if (!dataEvent) {
                    let text = data == false ? vm.NoText : vm.YesText;
                    //   return '<label class="control control-checkbox outlined"><input type="checkbox" ' + (data ? "checked=\"checked\"" : "") + ' disabled="disabled"><div class="control-indicator"></div></label>';
                    return '<span>' + text + '</span>';
                }
                else
                    return '<a href="#" class="custom-event-el" data-toggle="tooltip" title="' + (data ? activeTitle : inactiveTitle) + '" data-status="' + data + '" class="on-change" ><i class="mdi mdi-' + (data ? iconActive : iconInActive) + ' ' + (data ? activeClass : inactiveClass) + '"></i></a>';
            }
            else if (!s(format).isBlank() && !s(data).isBlank()) {
                if (format === "date")
                    return Helpers.Format.Date(data);
                else if (format === "datetime")
                    return Helpers.Format.DateTime(data);
                else if (format === "time")
                    return Helpers.Format.Time(data);
                else if (format === "percent")
                    return Helpers.Format.Percent(data);
                else if (format === "check") {
                    if (!dataEvent) {
                        //return '<label class="control control-checkbox outlined"><input type="checkbox" ' + (data ? "checked=\"checked\"" : "") + ' disabled="disabled"><div class="control-indicator"></div></label>';
                        return '<span>' + data == false ? vm.NoText : vm.YesText + '</span>';
                    }
                    else
                        return '<a href="#" class="custom-event-el" data-toggle="tooltip" title="' + (data ? activeTitle : inactiveTitle) + '"  data-status="' + data + '" class="on-change" ><i class="mdi mdi-' + (data ? iconActive : iconInActive) + ' ' + (data ? activeClass : inactiveClass) + '"></i></a>';
                }
                else
                    return data;
            }
            else
                return data;
        };

        this.GetFilters = function () {

            let filters = [];

            vm.Filter = localStorage.getItem("filter") || 0;
            filters.push(vm.Filter);
            return filters;
        };


        this.Load = function (data) {
            if (vm.Grid !== null) {
                vm.Grid.off("draw.dt");
                vm.Grid.destroy();
            }

            let uri = WebApiClient.ApiBaseUri + vm.ApiEndpoints.GetAll;

            if (vm.ParentId !== null && !vm.MergeParentId)
                uri += vm.ParentId;

            //Query paramters
            if (vm.ParameterFunction)
                uri += _.map(vm.ParameterFunction(), function (p) {
                    return "/" + p;
                });

            let gridConfig = {
                ajax: {
                    url: uri,
                    dataSrc: vm.DataSrc,
                    type: vm.Method,
                    contentType: 'application/json;',
                    dataType: 'json',
                    //headers: {
                    //    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                    //},
                    data: function (d) {
                        if (vm.Method === "GET")
                            return d;

                        if (d === null) {
                            d = {};
                        }

                        //If additional data is passed that's most probably employee search model
                        if (data)
                            d.EmployeeFilter = data;

                        d.AdditionalValues = [];
                        d.AdditionalValues = _.union(d.AdditionalValues, vm.AdditionalValuesFunction === null ? [] : vm.AdditionalValuesFunction());

                        if (d.search && d.search.value != "") {
                            d.search.columnName = $(vm.Table).data('text-search-column');
                        }
                        //ToDo: check with Shahbaz why we need to filter these columns as it is impacting the current sort functionality
                        //if (d.columns && d.columns.length > 1) {
                        //    d.columns = _.filter(d.columns, function (val) {
                        //        return val.orderable || val.searchable;

                        //    });
                        //}
                        if (vm.MergeParentId)
                            d.ParentId = parseInt(vm.ParentId);
                        //d = Helpers.Parse.KeysToPascal(d);
                        //console.log(d);
                        return JSON.stringify(d);
                    }

                },
                deferRender: true,
                fixedHeader: {
                    headerOffset: $('.navbar-static-top').innerHeight()
                },
                //fixedHeader: true,
                //scrollX: true, 
                dom: 'Brt<"#grid-footer.row"<"col-md-8"pl><"col-md-4"i>>', //'Brt<"#grid-footer.row"<"col-md-8"p><"col-md-2"l><"col-md-2"i>>',
                columns: vm.Columns,
                columnDefs: vm.ColumnDefinations,
                processing: vm.IsProcessing,
                serverSide: vm.IsServerSide,
                pageLength: vm.PageLength,
                //pagingType: "input",
                language: {
                    processing: "Loading Data...",
                    zeroRecords: "No data available",
                    lengthMenu: "Page size _MENU_",
                    infoFiltered: "(filtered)",
                    sInfo: "_START_ to _END_ of _TOTAL_ records",
                    paginate: {
                        next: '<i class="mdi mdi-arrow-right"></i>',
                        previous: '<i class="mdi mdi-arrow-left"></i>'
                    }
                },
                buttons: [],
                initComplete: function (s, j) {

                    //Raise row click event
                    var event = new CustomEvent('onDataLoaded', { bubbles: true, detail: { settings: s, json: j } });

                    // Dispatch the event
                    vm.Table.get(0).dispatchEvent(event);

                    //Set reorder indicator
                    if (vm.HasReorder)
                        vm.Table.find("tr td:first-child").addClass("reorder");

                    vm.TableWrapper.find('.buttons-colvis').parent().appendTo(vm.TableWrapper.find('.dt-actions'));

                    //vm.Table.colResizable({ liveDrag: true, partialRefresh:true });

                    //Added by UKB
                    if (vm.AllowTextBoxForPaging === true) {
                        let jumpToPageHTML = null;
                        //jumpToPageHTML = '<li><input type="text" class="jump-to-page"/></li>';
                        //$(vm.TableWrapper).find('.dataTables_paginate ul.pagination').append(jumpToPageHTML);
                        //$(vm.TableWrapper).find('.dataTables_paginate ul.pagination input.jump-to-page').off('keyup').on('keyup', function (e) {
                        //    vm.jumpToPage(e);
                        //})
                        jumpToPageHTML = '<div class="jump-wrap"><input type="text" class="jump-to-page form-control form-control-sm" value="1" title="Jump to page"/></div>';
                        $(vm.TableWrapper).find('#grid-footer .dataTables_length').parent().append(jumpToPageHTML);
                        $(vm.TableWrapper).find('#grid-footer input.jump-to-page').off('click').on('click', function (e) {
                            //this.select();
                        });
                        $(vm.TableWrapper).find('#grid-footer input.jump-to-page').off('keyup').on('keyup', function (e) {
                            vm.jumpToPage(e, this);
                        });
                    }

                    $('html, body').animate({
                        scrollTop: $(vm.TableWrapper).offset().top - 70
                    }, 500);

                    //Remove loader
                    vm.HideLoader();
                }
            };

            //Add action buttons
            let actions = null;

            if (vm.HasActions) {
                //Clean actions first
                vm.TableWrapper.find('.dt-actions').remove();
                vm.TableWrapper.find('.section-header-actions').append('<div class="dt-actions"><ul></ul></div>');
                actions = vm.TableWrapper.find(".dt-actions ul");

                if (vm.HasAdd) {

                    actions.append('<li class="add-' + vm.controller + '"><a href="#" title="' + vm.AddText + '" data-toggle="tooltip" class="action-primary"><i class="mdi mdi-plus"></i></a></li>');

                    actions.find('.add-' + vm.controller).click(function (e) {
                        e.preventDefault();
                        //Reset model to default state
                        if (vm.PopupViewModel !== null)
                            vm.PopupViewModel.ResetModel();

                        //Raise add event
                        var event = new CustomEvent('onAdd', { bubbles: true, detail: vm });
                        // Dispatch the event
                        vm.Table.get(0).dispatchEvent(event);
                    });

                    //gridConfig.buttons.push({

                    //    text: '<i class="fa fa-w-12 fa-plus"></i>',
                    //    titleAttr: vm.AddText,
                    //    className: "btn-toolbar btn-add",
                    //    action: function (e, dt, node, config) {

                    //        //Reset model to default state
                    //        if (vm.PopupViewModel !== null)
                    //            vm.PopupViewModel.ResetModel();

                    //        //Raise add event
                    //        var event = new CustomEvent('onAdd', { bubbles: true, detail: dt });
                    //        // Dispatch the event
                    //        vm.Table.get(0).dispatchEvent(event);

                    //    }
                    //});
                }

                if (vm.HasPrint) {
                    actions.append('<li  class="print-' + vm.controller + '"><a href="#"><i class="mdi mdi-printer"></i></a></li>');
                    $('.print-' + vm.controller).click(function (e) {
                        e.preventDefault();
                        vm.TableWrapper.find('.btn-print').click();
                    });
                    gridConfig.buttons.push({
                        extend: 'print',
                        text: '<i class="fa fa-w-12 fa-print"></i>',
                        titleAttr: "Print Data",
                        className: "btn-toolbar btn-print"
                    });
                }
                if (vm.HasColumnVisibility) {
                    actions.append(`<li class="visibility-` + vm.controller + `"><a href="#"><i class="mdi mdi-wrench"></i></a>
                        </li>`);

                    $('.visibility-' + vm.controller).click(function (e) {
                        e.preventDefault();
                        vm.TableWrapper.find('.buttons-colvis').click();

                    });

                    gridConfig.buttons.push({
                        extend: 'colvis',
                        columns: _.filter(vm.Table.find('thead th'), function (v, i) { //Exclude Actions Column from Toggle Colum Visibility List
                            return $(v).data('column-action') != true;
                        }),
                        className: 'colvis-sidebar'
                    });
                }

                if (vm.HasExport) {

                    actions.append(`<li class="visibility-` + vm.controller + `"><div class="dropdown d-inline-block widget-dropdown textual-link">
                                        <a class="dropdown-toggle text-link" href="#" role="button" id="dropdown-units" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" data-display="static"><span>Export</span></a>
                                             <ul class="dropdown-menu dropdown-menu-custom" aria-labelledby="dropdown-units"> <li class="dropdown-item excel"><a href="#"><i class="mdi mdi-file-excel"></i> Excel</a></li>
                                                                      <li class="dropdown-item csv" ><a href="#"><i class="mdi mdi-file-delimited"></i> CSV</a></li>
                                                                      <li class="dropdown-item pdf"><a href="#"><i class="mdi mdi-file-pdf"></i> PDF</a></li>
                                                                      <ul></div></li>`);

                    vm.TableWrapper.find('.csv').click(function (e) {
                        e.preventDefault();
                        vm.TableWrapper.find('.btn-csv').click();
                    });

                    vm.TableWrapper.find('.excel').click(function (e) {
                        e.preventDefault();
                        vm.TableWrapper.find('.btn-excel').click();
                    });

                    vm.TableWrapper.find('.pdf').click(function (e) {
                        e.preventDefault();
                        vm.TableWrapper.find('.btn-pdf').click();
                    });

                    gridConfig.buttons.push(
                        {
                            extend: 'csv',
                            text: '<i class="fa fa-w-12 fa-file-csv"></i>',
                            titleAttr: "Export as Comma Seperated File",
                            className: "btn-toolbar btn-csv",
                            exportOptions: {
                                columns: [":visible:not([data-column-action='true'])"]
                            },
                            filename: function () {
                                return vm.GetExportedFileName();
                            }/*,
                            customize: function (xlsx) {
                                var sheet = xlsx.xl.worksheets['sheet1.xml'];

                                $('c[r=A1] t', sheet).text(vm.GetExportedFileName());
                            }*/
                        });
                    gridConfig.buttons.push({
                        extend: 'excel',
                        text: '<i class="fa fa-w-12 fa-file-excel"></i>',
                        titleAttr: "Export as Excel Workbook",
                        className: "btn-toolbar btn-excel",
                        exportOptions: {
                            columns: [":visible:not([data-column-action='true'])"]
                            //columns: [":visible:not(:contains(Actions))"]

                        },
                        filename: function () {
                            return vm.GetExportedFileName();
                        }/*,
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];

                            $('c[r=A1] t', sheet).text(vm.GetExportedFileName());
                        }*/
                    });
                    gridConfig.buttons.push({
                        extend: 'pdf',
                        text: '<i class="fa fa-w-12 fa-file-pdf"></i>',
                        titleAttr: "Export as PDF",
                        className: "btn-toolbar btn-pdf",
                        // orientation: function () { return 'landscape'; },
                        filename: function () {
                            return vm.GetExportedFileName();
                        },
                        exportOptions: {
                            columns: [":visible:not([data-column-action='true'])"]
                        },
                        customize: function (doc, config) {
                            var tableNode;
                            for (var i = 0; i < doc.content.length; ++i) {
                                if (doc.content[i].table !== undefined) {
                                    tableNode = doc.content[i];
                                    break;
                                }
                            }

                            var rowIndex = 0;
                            var tableColumnCount = tableNode.table.body[rowIndex].length;

                            if (tableColumnCount > 5) {
                                doc.pageOrientation = 'landscape';
                            }

                        }
                        /*,
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];

                            $('c[r=A1] t', sheet).text(vm.GetExportedFileName());
                        }*/
                    });
                }

                if (vm.HasSearch) {
                    actions.append('<li><div class="textbox-with-icon"><i class="fa fa-filter"></i><input type="text" placeholder="' + vm.GetSearchPlaceholderText() + '" class="dt-search-field" /><span class="hidden">x</span></div></li>');
                    //Setup search here
                    vm.TableWrapper.find('.dt-search-field').on('keyup', function (event) {
                        vm.Grid.search(this.value).draw();
                        if (this.value && this.value.length > 0)
                            $(event.target).closest('li').find('span').removeClass("hidden");
                        else
                            $(event.target).closest('li').find('span').addClass("hidden");
                    });
                    vm.TableWrapper.find('.dt-search-field').closest('li').find('span').off('click').on('click', function () {
                        vm.TableWrapper.find('.dt-search-field').val('').trigger('keyup');
                    });

                    //actions.append('<li class="search-field-wrapper"><input type="text" placeholder="' + vm.SearchPlaceHolderText + '" class="dt-search-field grid-search-text" /><i class="mdi mdi-close"></i></li>');
                    //vm.SearchTextBox = $(".dt-search-field");
                    //let searchWrapper = $(".search-field-wrapper");
                    //searchWrapper.find('.mdi-close').hide();
                    ////Grid search
                    //vm.SearchTextBox.on('keydown', function (e) {

                    //    if (this.value.length > 0)
                    //        searchWrapper.find('.mdi-close').show();
                    //    else
                    //        searchWrapper.find('.mdi-close').hide();

                    //    let isEnter = vm.GetKey(e) === "enter";

                    //    if (isEnter)
                    //        vm.GridAPI.search(this.value).draw();
                    //});

                    //searchWrapper.find('.mdi-close').click(function () {
                    //    vm.SearchTextBox.val("");
                    //    vm.GridAPI.search("").draw();
                    //});
                }
            }

            if (vm.HasReorder) {

                gridConfig.rowReorder = {
                    dataSrc: vm.ReorderColumn,
                    update: true
                };
            }

            /*TODO://*/
            if (vm.HasMultiSelect) {
                gridConfig.select = {
                    style: 'multi'
                };
            }
            else if (vm.HasSelect) {
                gridConfig.select = {
                    style: 'single'
                };
            }

            if (vm.DefaultSorting !== null) {
                $.extend(gridConfig, {
                    order: vm.DefaultSorting
                });
            }
            else {
                if (vm.HasSelectionControl || vm.HasReorder) {
                    $.extend(gridConfig, {
                        order: [[1, "asc"]]
                    });

                }
            }

            //Very important, this will automatically destory the Datatable before initializing a new one
            gridConfig.destroy = true;

            vm.Grid = vm.Table.DataTable(gridConfig);

            //TODO: make page size configurable
            vm.Grid.on('xhr', function (e, settings, json) {
                if (json.recordsTotal <= 10) {
                    $(vm.TableWrapper[0]).find(vm.GridFooterSelector).hide();
                } else {
                    $(vm.TableWrapper[0]).find(vm.GridFooterSelector).show();
                }
            });

            if (vm.HasReorder) {
                vm.Grid.off('row-reorder').on('row-reorder', function (e, diff, edit) {
                    let change = _.first(diff);
                    //Raise add event
                    var reorderEvent = new CustomEvent('onReorder', { bubbles: true, detail: diff });
                    // Dispatch the event
                    console.log(e, diff, edit);
                    vm.Table.get(0).dispatchEvent(reorderEvent);
                });

                //vm.Grid.off('row-reorder').on('row-reorder', function (e, settings, details) {
                //    if (details.drop) {
                //        //Raise add event
                //        var reorderEvent = new CustomEvent('onReorder', { bubbles: true, detail: { from: details.from, to: details.to } });
                //        // Dispatch the event
                //        vm.Table.get(0).dispatchEvent(reorderEvent);                       
                //    }

                //});
            }

            //If table-buttons element is present in the mark up move the export buttons to that continer

            let buttonsContainer = vm.TableWrapper.find(".dt-actions");

            if (buttonsContainer.length !== 0) {

                if (buttonsContainer.find("div").length === 0) {

                    vm.Grid.buttons().containers().appendTo(buttonsContainer);
                    vm.TableWrapper.find(".dt-buttons").addClass("pull-right");

                    //Customize column visibility button
                    $(".buttons-colvis span").html("<i class='icon-list-view'></i>");
                }
            }

            //TODO: later
            /*vm.Grid.on('processing.dt', function (e, settings, processing) {
                $('#processingIndicator').css('display', 'block');
                if (processing) {
                    $('#processingIndicator').css('display', '');//$(e.currentTarget).LoadingOverlay("show");
                } else {
                    $('#processingIndicator').css('display', 'block');//$(e.currentTarget).LoadingOverlay("hide", true);
                }
            });*/



            //Action button click event
            //vm.Table.find('tbody')
            //    .off('click', 'div.actions-dropdown')
            //    .on('click', 'div.actions-dropdown', function (e) {
            //        e.preventDefault();
            //        e.stopImmediatePropagation();
            //    });

            //Set edit dialog trigger
            vm.Table.find('tbody')
                .off('click', "a.edit-" + vm.controller + "")
                .on('click', "a.edit-" + vm.controller + "", function (event) {

                    let id = $(this).data('id');
                    //Raise add event
                    var editEvent = new CustomEvent('onEdit', { bubbles: true, detail: id });
                    // Dispatch the event
                    vm.Table.get(0).dispatchEvent(editEvent);

                    if (vm.PopupViewModel)
                        vm.ShowEditScreen(event, id, false);

                });

            //Set copy dialog trigger
            vm.Table.find('tbody')
                .off('click', "a.copy-" + vm.controller + "")
                .on('click', "a.copy-" + vm.controller + "", function (event) {

                    let id = $(this).data('id');
                    //Raise add event
                    var editEvent = new CustomEvent('onCopy', { bubbles: true, detail: id });
                    // Dispatch the event
                    vm.Table.get(0).dispatchEvent(editEvent);

                    vm.ShowEditScreen(event, id, true);

                });

            //Set delete confirmation trigger
            vm.Table.find('tbody')
                .off('click', "a.delete-" + vm.controller + "")
                .on('click', "a.delete-" + vm.controller + "", function (event) {

                    vm.ConfirmDelete(event, $(this).data('id'));

                });

            //Set edit dialog trigger
            vm.Table.find('tbody')
                .off('click', "a.export-" + vm.controller + "")
                .on('click', "a.export-" + vm.controller + "", function (event) {
                    event.preventDefault();
                    event.stopImmediatePropagation();
                    let id = $(this).data('id');
                    // Dispatch the event
                    vm.Table.get(0).dispatchEvent(new CustomEvent('onExport', { bubbles: true, detail: id }));

                    return false;
                });

            //Reister refresh event, reload on save and delete
            if (vm.PopupDialog !== null)
                vm.RegisterReload();

            return vm.Grid;
        };

        this.RegisterReload = function () {

            vm.PopupDialog.off("onRefresh").on("onRefresh", function (event) {
                //TODO: Make it less confusing(DataParamObject)
                //vm.Initialize(vm.DataParamObject);
                vm.Grid.ajax.reload();
            });
        };

        this.Delete = function (id) {

            vm.StartProcessing();

            WebApiClient.Delete(vm.ApiEndpoints.Delete + id, null, function (model) {

                vm.EndProcessing();

                //Raise refresh event
                var event = new CustomEvent('onRefresh', { bubbles: true, detail: { ViewModel: vm } });

                // Dispatch the event
                vm.PopupDialog.get(0).dispatchEvent(event);

            }, function () {
                //Clear processing animation on error
                vm.EndProcessing();
            });
        };

        this.ConfirmDelete = function (event, id) {

            event.preventDefault();
            event.stopImmediatePropagation();

            vm.DeleteConfirm
                .modal("show")
                .off("click", ".btn-yes")
                .on("click", ".btn-yes", function () {
                    vm.Delete(id);
                });

        };

        this.StartProcessing = function () {

            //Add sppinner and loading text
            this.Spinner = $('<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>');

            let loadingText = this.DeleteButton.data("processing-text");
            let iconClass = this.DeleteButton.data("icon");
            this.Icon = $('<i class="' + iconClass + '"></i>');
            this.ButtonText = this.DeleteButton.text();
            this.DeleteButton.text(loadingText);
            this.DeleteButton.append(this.Spinner);
        };

        this.EndProcessing = function () {

            this.DeleteButton.text('');
            this.DeleteButton.append(this.Icon);
            this.DeleteButton.append(this.ButtonText);

            this.Spinner.remove();
            this.DeleteConfirm.modal("hide");
        };

        this.ShowEditScreen = function (event, id, isCopy) {

            event.preventDefault();
            event.stopImmediatePropagation();

            vm.PopupViewModel.IsCopy = isCopy;
            vm.PopupViewModel.Load(id);

            //show pager
            vm.PopupDialog.find(".modal-footer .pager ul").show();
            vm.LoadPager();
            vm.ShowDialog();

            return false;
        };

        this.ShowAddScreen = function () {
            //Hide pager
            vm.PopupDialog.find(".modal-footer .pager ul").hide();
            vm.ShowDialog();
        };

        this.ShowDialog = function () {

            //Hide tooltips
            $("a[data-toggle='tooltip']").tooltip("hide");

            vm.PopupDialog.modal('show');
            //Raise shown event
            var event = new CustomEvent('onDialogShown', { bubbles: true, detail: { ViewModel: vm } });

            // Dispatch the event
            vm.PopupDialog.get(0).dispatchEvent(event);
        };

        this.LoadPager = function () {
            let key = vm.PopupDialog.find(".pager").data("key");
            let value = vm.PopupDialog.find(".pager").data("value");

            vm.PopupViewModel.PagerDataOptions(_.map(vm.Grid.data().toArray(), function (d) {
                return { Text: d[value], Value: d[key] };
            }));

            vm.PopupDialog.find(".pager .record-first").click(function (e) {
                vm.PopupViewModel.PagerData(_.first(vm.Grid.data().toArray())[key]);
                e.preventDefault();
            });

            vm.PopupDialog.find(".pager .record-last").click(function (e) {
                vm.PopupViewModel.PagerData(_.last(vm.Grid.data().toArray())[key]);
                e.preventDefault();
            });

            vm.PopupDialog.find(".pager .record-next").click(function (e) {

                let index = _.findIndex(vm.Grid.data().toArray(), function (d) { return d[key] === vm.PopupViewModel.PagerData(); });
                index++;
                vm.PopupViewModel.PagerData(vm.Grid.data().toArray()[Math.min(index, vm.Grid.data().length - 1)][key]);
                e.preventDefault();
            });

            vm.PopupDialog.find(".pager .record-prev").click(function (e) {

                let index = _.findIndex(vm.Grid.data().toArray(), function (d) { return d[key] === vm.PopupViewModel.PagerData(); });
                index--;
                vm.PopupViewModel.PagerData(vm.Grid.data().toArray()[Math.max(0, index)][key]);
                e.preventDefault();
            });

        };

        this.BindRowClick = function () {

            /*TODO: later*/
            vm.Grid
                .off('select.dt').on('select.dt', function (e, dt, type, indexes) {

                    //   $(e.currentTarget).find('tr td input[type="radio"],tr td input[type="checkbox"]').removeAttr('checked');
                    if (vm.HasMultiSelect) {
                        $.each(indexes, function (i, v) { $(e.currentTarget).find('tbody > tr:eq(' + v + ') td:first input[type="checkbox"]').attr('checked', 'checked').prop('checked', true); })
                    }
                    else {
                        $.each(indexes, function (i, v) { $(e.currentTarget).find('tbody > tr:eq(' + v + ') td:first input[type="radio"]').attr('checked', 'checked').prop('checked', true); })
                    }
                    //if (!$(e.currentTarget).find('tbody > tr:eq(' + indexes[0] + ')').hasClass('selected')) {
                    //    $(e.currentTarget).find('tbody > tr:eq(' + indexes[0] + ')').addClass('selected');
                    //}
                    let dataRows = vm.Grid.rows(indexes).data().toArray();

                    let row = dataRows[0];

                    //Raise row click event
                    let event = new CustomEvent('onRowClick', { bubbles: true, detail: row });

                    // Dispatch the event
                    vm.Table.get(0).dispatchEvent(event);

                    //Raise selection event
                    //vm.Table.get(0).dispatchEvent(new CustomEvent('onDataSelected', { bubbles: true, detail: dataRows }));

                });

            vm.Grid
                .off('deselect.dt').on('deselect.dt', function (e, dt, type, indexes) {
                    if (vm.HasMultiSelect) {
                        $.each(indexes, function (i, v) { $(e.currentTarget).find('tbody > tr:eq(' + v + ') td:first input[type="checkbox"]').removeAttr('checked').prop('checked', false) });

                    }
                    else {
                        $.each(indexes, function (i, v) { $(e.currentTarget).find('tbody > tr:eq(' + v + ') td:first input[type="radio"]').removeAttr('checked').prop('checked', false); });
                    }

                });

        };

        this.SubscribeEvents = function () {

            vm.Grid.off('draw.dt').on('draw.dt', function (settings) {


                setTimeout(function () {  //Setup tooltip
                    $('[data-toggle="tooltip"]').tooltip();
                }, 50);

                $(".action-dropdown").click(function (e) {

                    //Hide all open menus first
                    $(".action-dropdown").find('.dropdown-menu').hide();

                    if ($(e.target).hasClass("icon-burger-mini")) {
                        e.stopImmediatePropagation();
                        e.preventDefault();
                        $(this).find('.dropdown-menu').toggle();
                    }

                });

                $(document).click(function () {
                    $(".action-dropdown .dropdown-menu").hide();
                });

                vm.TableWrapper.find(".dataTables_length select").removeClass("custom-select").removeClass("custom-select-sm");

                // Dispatch the event
                vm.Table.get(0).dispatchEvent(new CustomEvent('onDraw', { bubbles: true, detail: vm.Grid }));

                if (vm.DataEvent) {

                    vm.Table.find(".custom-event-el").off("click").on("click", function (e) {

                        e.preventDefault();

                        //Hide tooltip
                        $('[data-toggle="tooltip"]').tooltip("hide");

                        // Dispatch the event
                        vm.Table.get(0).dispatchEvent(new CustomEvent(vm.DataEvent, { bubbles: true, detail: vm.Grid.row($(this).parents("tr").first()).data() }));

                    });
                }

                //if (vm.Grid.page.info().pages > 1) {
                //    console.log("No need to display pager");
                //    $(vm.TableWrapper[0]).find('#grid-footer').show()
                //}
                //else {
                //    $(vm.TableWrapper[0]).find('#grid-footer').hide()
                //}

            });
            vm.Grid.off('page.dt').on('page.dt', function (settings) {
                $(vm.TableWrapper).find('#grid-footer input.jump-to-page').val(vm.Grid.page.info().page + 1);
            });

        };

        this.ConfirmAction = function (onConfirm, onCancel) {

            vm.ActionConfirm.find(".spinner-grow").remove();

            vm.ActionConfirm
                .modal("show")
                .off("click", ".btn-yes")
                .on("click", ".btn-yes", function () {

                    //Start animation
                    let btn = $(this);

                    //Add sppinner and loading text
                    let spinner = $('<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>');

                    btn.append(spinner);

                    onConfirm();
                })
                .off("click", ".btn-no")
                .on("click", ".btn-no", onCancel);
        };

        this.ShowLoader = function () {
            vm.TableWrapper.append(vm.TableLoader);
        };
        this.HideLoader = function () {
            vm.TableLoader.remove();
        };
        this.jumpToPage = function (e, textBox) {

            let totalPages = vm.Grid.page.info().pages;

            let pageNumber = parseInt($(vm.TableWrapper).find('#grid-footer input.jump-to-page').val()) || 1;

            //backspace
            if (e.keyCode === 8)
                return;
            if (pageNumber < 1) {
                pageNumber = 1;
            }
            if (pageNumber > totalPages) {
                pageNumber = totalPages;
            }

            // 38 = up arrow, 39 = right arrow
            if (e.which === 38 || e.which === 39) {
                if (pageNumber === totalPages) return;

                pageNumber++;
            }
            // 37 = left arrow, 40 = down arrow
            else if ((e.which === 37 || e.which === 40) && e.which > 1) {
                if (pageNumber === 1) return;

                pageNumber--;
            }

            //if (this.value === '' || this.value.match(/[^0-9]/)) {
            //    /* Nothing entered or non-numeric character */
            //    pgNumber = pgNumber.replace(/[^\d]/g, ''); // don't even allow anything but digits

            //    return;
            //}


            vm.Grid.page(pageNumber - 1).draw(false);
            $(vm.TableWrapper).find('#grid-footer input.jump-to-page').val(pageNumber);
            //textBox.select();
        };

        //Call this method in document ready to populate the grid
        this.Initialize = function (data) {

            vm.ShowLoader();

            //Hide action confirmation dialog initially
            vm.ActionConfirm.modal("hide");

            vm.InitializeColumns();
            vm.InitializeColumnDefinitions();

            let grid = vm.Load(data || null);

            if (vm.PublishRowClickEvent) {
                vm.BindRowClick();
            }

            vm.SubscribeEvents();

            return grid;
        };

    }
    //Export globally
    window.GridBase = GridBase;

}(window));
