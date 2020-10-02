(function (window) {
    'use strict';

    //nestedObjectKey is used to get property for nested object
    //added for validation messages compilation
    function BaseViewModel(view, model, nestedObjectKey) {
        this.Id = 0;
        this.Modal = $(view);
        this.CloseOnSuccess = this.Modal.get(0).hasAttribute("data-close-on-success");
        this.IsAsync = false;
        this.IsCopy = false;
        this.SaveButton = this.Modal.find(".btn-save");
        this.SaveNewButton = this.Modal.find(".btn-save-new");
        this.CancelButton = this.Modal.find(".btn-cancel");
        this.Spinner = null;
        this.ButtonText = "";
        this.Icon = "";
        this.OverlayLoader = $('<div class="overlay-loader" id="overlay-loader"><div class="sk-fading-circle"><div class="sk-circle1 sk-circle"></div><div class="sk-circle2 sk-circle"></div><div class="sk-circle3 sk-circle"></div><div class="sk-circle4 sk-circle"></div><div class="sk-circle5 sk-circle"></div><div class="sk-circle6 sk-circle"></div><div class="sk-circle7 sk-circle"></div><div class="sk-circle8 sk-circle"></div><div class="sk-circle9 sk-circle"></div><div class="sk-circle10 sk-circle"></div><div class="sk-circle11 sk-circle"></div><div class="sk-circle12 sk-circle"></div></div></div>');

        this.ApiEndpoints = {
            Save: "",
            GetById: ""
        };

        this.LoadCallback = null;

        let vm = this;

        this.MappingsToIgnore = function () {

            let mappingIgnore = vm.mappingIgnore() || [];

            mappingIgnore.push("controller");
            mappingIgnore.push("idField");
            mappingIgnore.push("mappingIgnore");
            mappingIgnore.push("resetIgnore");

            let options =
                _.chain(vm)
                    .keys()//array of object keys
                    .filter(function (v) {
                        return ko.isWritableObservable(vm[v]);
                    })
                    .filter(function (v) {
                        return s(v).endsWith("Options");
                    })
                    .union(mappingIgnore)
                    .value();

            return options;
        };

        this.ResetModel = function () {

            let resetIgnore = vm.resetIgnore() || [];

            resetIgnore.push("controller");
            resetIgnore.push("idField");
            resetIgnore.push("PagerData");//Reseting pager data will cause endless loop, each change causes reload from the server.
            resetIgnore.push("mappingIgnore");

            vm.Id = 0;

            _.chain(vm)
                .keys()//array of object keys
                .filter(function (v) {
                    return ko.isWritableObservable(vm[v]);
                })
                .reject(function (v) {
                    let r = _.contains(resetIgnore, v);
                    return r;
                })
                .reject(function (v) {
                    let r = s(v).endsWith("Options");
                    return r;
                })
                .reject(function (v) {
                    return v === "resetIgnore" || v === "mappingIgnore";
                })
                .each(function (name) {
                    if (typeof vm[name]() === "number")
                        vm[name](0);
                    else if (typeof vm[name]() === "boolean")
                        vm[name](false);
                    else
                        vm[name](null);
                });

            //Reset dropdown selection
            vm.Modal.find("select").each(function () {
                if (!this.hasAttribute("data-reset-ignore"))
                    this.selectedIndex = 0;
            });

            //Reset checkboxes
            vm.Modal.find("input[type=checkbox]").each(function () {
                this.checked = false;
            });

            //Hide all errors initially
            vm.errors.showAllMessages(false);
        };

        this.CompileValidation = function (view) {

            //TODO: Improve code
            //[UKB] We need to validate only those fields which are currently visible to user( consider scenario of tabs/popups/side panels etc)
            $(view).find("[required]").each(function () {
                let elem = $(this),
                    msg = elem.data("required-message"),
                    minLength = parseInt(elem.data("min-length")),
                    maxLength = parseInt(elem.data("max-length"));
                if (this.hasAttribute('data-bind')) {
                    let bindings = $(this).data("bind").split(',');
                    let valueBinding = _.find(bindings, function (b) {
                        return s(b).trim().startsWith("value:") || s(b).trim().startsWith("textInput:");
                    });


                    let extendedProperties = {
                        required: {
                            message: msg
                        }
                    };
                    if (minLength !== null && minLength !== 0 && !isNaN(minLength)) {
                        extendedProperties.minLength = minLength;
                    }
                    if (maxLength !== null && maxLength !== 0 && !isNaN(maxLength)) {
                        extendedProperties.maxLength = maxLength;
                    }
                    let property = s(valueBinding.split(':')[1]).trim().value();

                    if (s(property).startsWith("moment(")) {
                        property = s(property).split("moment(")[1].split("(")[0];
                    }

                    if (nestedObjectKey) {
                        if (typeof vm[nestedObjectKey][property].extend == 'function')
                            vm[nestedObjectKey][property].extend(extendedProperties);
                    }
                    else {
                        if (typeof vm[property].extend === 'function')
                            vm[property].extend(extendedProperties);
                    }
                }
            });
            $(view).find("[number]").each(function () {
                let elem = $(this),
                    msg = elem.data("number-message"),
                    minLength = parseInt(elem.data("min-length")),
                    maxLength = parseInt(elem.data("max-length"));

                if (this.hasAttribute('data-bind')) {
                    let bindings = $(this).data("bind").split(',');
                    let valueBinding = _.find(bindings, function (b) {
                        return s(b).trim().startsWith("value:") || s(b).trim().startsWith("textInput:");
                    });

                    let extendedProperties = {
                        number: {
                            message: msg
                        }
                    };
                    if (minLength !== null && minLength !== 0 && !isNaN(minLength)) {
                        extendedProperties.minLength = minLength;
                    }
                    if (maxLength !== null && maxLength !== 0 && !isNaN(maxLength)) {
                        extendedProperties.maxLength = maxLength;
                    }

                    let property = s(valueBinding.split(':')[1]).trim().value();

                    if (nestedObjectKey) {
                        if (typeof vm[nestedObjectKey][property].extend == 'function')
                            vm[nestedObjectKey][property].extend(extendedProperties);
                    }
                    else {
                        if (typeof vm[property].extend == 'function')
                            vm[property].extend(extendedProperties);
                    }
                }
            });

        };

        this.Bind = function (view, model) {

            //Setup validation
            ko.validation.init({
                registerExtenders: true,
                messagesOnModified: true,
                insertMessages: true,
                parseInputAttributes: true,
                messageTemplate: null,
                decorateInputElement: true,
                errorMessageClass: 'error',
                errorElementClass: 'error'
            }, true);

            //Extend from viewModelJS
            $.extend(model, { PagerDataOptions: [], PagerData: null });
            $.extend(vm, ko.mapping.fromJS(model));

            //Api endpoints
            $.extend(vm.ApiEndpoints, ApiEndpoints[vm.controller()]);

            vm.CompileValidation(view);

            //Aply validation
            vm.errors = ko.validation.group(vm, { deep: true, live: true });

            //Apply the bindings
            ko.applyBindings(vm, view);

            //Hide all errors initially
            vm.errors.showAllMessages(false);

            //Subscribe to pager change 
            vm.PagerData.subscribe(function (selected) {
                if (selected && selected !== vm.Id) { //ToDo: Implement Generic ViewModel Dirty Check and save data only once changed/view model is dirty
                    vm.OnSave(Globals.Enums.PopupOperationType.SaveAndNavigate).done(function (data) {
                        console.log('data from promise ', data)
                        if (selected && selected !== vm.Id) {
                            vm.Load(selected);
                        }
                    });
                    
                }
            });

        };

        this.StartProcessing = function (popupOperationType) {
            //Add sppinner and loading text
            this.Spinner = $('<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>');

            if (popupOperationType == Globals.Enums.PopupOperationType.SaveAndAddNew) {
                let loadingText = this.SaveNewButton.data("processing-text");
                let iconClass = this.SaveNewButton.data("icon");
                this.Icon = $('<i class="' + iconClass + '"></i>');
                this.ButtonText = this.SaveNewButton.text();
                this.SaveNewButton.text(loadingText);
                this.SaveNewButton.append(this.Spinner);
            }
            else if (popupOperationType == Globals.Enums.PopupOperationType.Save) {
                let loadingText = this.SaveButton.data("processing-text");
                let iconClass = this.SaveButton.data("icon");
                this.Icon = $('<i class="' + iconClass + '"></i>');
                this.ButtonText = this.SaveButton.text();
                this.SaveButton.text(loadingText);
                this.SaveButton.append(this.Spinner);
            }

            else if (popupOperationType == Globals.Enums.PopupOperationType.Cancel) {
                let loadingText = this.CancelButton.data("processing-text");
                let iconClass = this.CancelButton.data("icon");
                this.Icon = $('<i class="' + iconClass + '"></i>');
                this.ButtonText = this.CancelButton.text();
                this.CancelButton.text(loadingText);
                this.CancelButton.append(this.Spinner);
            }
            else if (popupOperationType == Globals.Enums.PopupOperationType.SaveAndNavigate) {
                vm.StartLoading();
            }
        };

        this.EndProcessing = function (popupOperationType) {

            if (popupOperationType == Globals.Enums.PopupOperationType.SaveAndAddNew) {
                this.SaveNewButton.text('');
                this.SaveNewButton.append(this.Icon);
                this.SaveNewButton.append(this.ButtonText);
            }
            else if (popupOperationType == Globals.Enums.PopupOperationType.Save) {
                this.SaveButton.text('');
                this.SaveButton.append(this.Icon);
                this.SaveButton.append(this.ButtonText);
            }
            else if (popupOperationType == Globals.Enums.PopupOperationType.Cancel) {
                this.CancelButton.text('');
                this.CancelButton.append(this.Icon);
                this.CancelButton.append(this.ButtonText);
            }

            this.Spinner.remove();

            if (this.CloseOnSuccess)
                this.Modal.modal("hide");
        };

        this.StartLoading = function () {

            vm.IsAsync = vm.Modal.get(0).hasAttribute("data-async");

            if (vm.IsAsync) {
                //Hide dialog content
                vm.Modal.find(".modal-body form").addClass("invisible");
                vm.Modal.find(".modal-body").prepend(vm.OverlayLoader);
            }
        };

        this.EndLoading = function () {
            if (vm.IsAsync) {
                //vm.OverlayLoader.remove();
                vm.Modal.find(".overlay-loader").remove();
                //Show dialog content
                vm.Modal.find(".modal-body form").removeClass("invisible");
            }
        };

        this.Load = function (id) {

            vm.Id = id;

            if (id > 0)
                vm.StartLoading();

            WebApiClient.Get(vm.ApiEndpoints.GetById + vm.Id, null, function (model) {

                vm.ResetModel();

                //This is required since Id is reset in the resest model method
                vm.Id = id;
                vm.PagerData(id);

                //Hide all errors initially
                vm.errors.showAllMessages(false);

                ko.mapping.fromJS(model, vm);

                //Raise add event
                var editEvent = new CustomEvent('onModelLoaded', { bubbles: true, detail: model, data: model });
                // Dispatch the event
                window.dispatchEvent(editEvent);


                vm.EndLoading();

                if (Helpers.Utility.HasValue(vm.LoadCallback))
                    vm.LoadCallback();

            }, null);
        };

        this.OnSave = function (popupOperationType) {

            //Don't close if Save & New button OR Next/Previous Button is cicked
            vm.CloseOnSuccess = !(popupOperationType == Globals.Enums.PopupOperationType.SaveAndAddNew || popupOperationType == Globals.Enums.PopupOperationType.SaveAndNavigate);

            if (vm.errors().length > 0) {
                vm.FocusOnError();
                vm.errors.showAllMessages();

                //Raise validation failed event
                vm.Modal.get(0).dispatchEvent(new CustomEvent('onValidationFailed', { bubbles: true, detail: { Errors: vm.errors() } }));

                return;
            }

            vm.StartProcessing(popupOperationType);

            let mapping = {
                'ignore': vm.MappingsToIgnore()
            };

            let model = ko.mapping.toJS(vm, mapping);

            let endpoint = vm.ApiEndpoints.Save + (vm.IsCopy ? 0 : vm.Id);

            model.CreatedDate = "2020-10-02T13:10:14.605Z";//Dummy date
            model.LastUpdated = "2020-10-02T13:10:14.605Z";//Dummy date

            return WebApiClient.Put(endpoint, model, function (response) {

                vm.EndProcessing(popupOperationType);

                //Clear model if Save & New clicked
                if (popupOperationType == Globals.Enums.PopupOperationType.SaveAndAddNew)
                    vm.ResetModel();

                //Raise onRefresh event
                var event = new CustomEvent('onRefresh', { bubbles: true, detail: { ViewModel: vm } });

                // Dispatch the event
                vm.Modal.get(0).dispatchEvent(event);

                //Dispatch the event
                vm.Modal.get(0).dispatchEvent(new CustomEvent('onSaved', { bubbles: true, detail: { Result: response } }));

            }, function (error) {
                vm.EndProcessing(popupOperationType);
            });
        };

        //Not used so far
        this.OnCancel = function () {

            if (vm.Id > 0) {

                //vm.StartProcessing(false, false);
                vm.errors.showAllMessages(false);

                //let model = { key: vm.Id, name: vm.controller() };

                //WebApiClient.Post(ApiEndpoints.Parameter.ReleaseLock, model, function (response) {
                //    vm.EndProcessing(false, false);
                //}, null);
            }
            else {
                //vm.Close();
            }

            vm.Close();

        };

        this.Save = function (model) {

            //let mapping = {
            //    'ignore': vm.MappingsToIgnore()
            //};

            //let model = ko.mapping.toJS(vm, mapping);

            let endpoint = vm.ApiEndpoints.Save + vm.Id;

            WebApiClient.Put(endpoint, model, function (response) {

                //TODO:Use standard naming convention for events

                //Raise onRefresh event
                var event = new CustomEvent('onRefresh', { bubbles: true, detail: { ViewModel: vm } });

                // Dispatch the event
                vm.Modal.get(0).dispatchEvent(event);

                //Dispatch the event
                vm.Modal.get(0).dispatchEvent(new CustomEvent('OnSaved', { bubbles: true, detail: { ViewModel: vm } }));

            }, function (error) {
                //vm.EndProcessing(true, isSaveAndNew);
            });
        };

        this.Cancel = function () {

            if (vm.Id > 0) {

                //vm.StartProcessing(false, false);
                vm.errors.showAllMessages(false);

                //let model = { key: vm.Id, name: vm.controller() };

                //WebApiClient.Post(ApiEndpoints.Parameter.ReleaseLock, model, function (response) {
                //    vm.EndProcessing(false, false);
                //}, null);
            }
            else {
                //vm.Close();
            }

            vm.Close();
        };

        this.ShowError = function (title, error) {

            toastr.error(error, title, {
                closeButton: true
            });
        };

        this.FocusOnError = function () {
            let toElement = $(view).find('.error:eq(0)'),
                speed = 500;

            $('html, body, .modal-body').animate({
                scrollTop: $(toElement).offset().top - 60
            }, speed);

        };

        this.Close = function () {
            vm.Modal.modal("hide");
        };

        this.CancelButton.on("click", vm.Cancel);
        this.SaveButton.on("click", function () { vm.OnSave(Globals.Enums.PopupOperationType.Save); });
        this.SaveNewButton.on("click", function () { vm.OnSave(Globals.Enums.PopupOperationType.SaveAndAddNew); });

        //Ko binding
        vm.Bind(view, model);
    }

    //Export globally
    window.BaseViewModel = BaseViewModel;
}(window));