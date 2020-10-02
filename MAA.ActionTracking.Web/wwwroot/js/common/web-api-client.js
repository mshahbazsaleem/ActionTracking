/*********************************************************************************/
//Generic API wrappers to communicate with the backend server
/*********************************************************************************/
var WebApiClient = {
    ApiBaseUri: Globals.Config.BaseAppPath,
    GetAccessToken: function () {
        if (window.sessionStorage.accessToken)
            return window.sessionStorage.accessToken;
        else {

            //Setup syncronous call
            $.ajaxSetup({
                global: false,
                async: false
            });

            $.Get(this.AppBase + '/api/token')
                .done(function (tokenResponse) {
                    window.sessionStorage.accessToken = tokenResponse;
                })
                .fail(function (xhr, status, error) {
                    onError(xhr, status, error);
                });

            //Return the access token when the above "SYNCRONOUS" call is complete.
            if (window.sessionStorage.accessToken)
                return window.sessionStorage.accessToken;
            else
                throw Error("Fail to retreive access token");
        }

    },
    GetEndPointAddress: function (path) {
        return this.ApiBaseUri + path;
    },
    Get: _Get,
    Post: _Post,
    Put: _Put,
    Delete: _Delete
};
function _Get(path, requestdata, successCallback, errorCallback) {

    //if (Helpers.Utility.HasValue(viewModel))
    //    viewModel.StartLoading();

    var jqxhr = $.ajax({
        url: WebApiClient.GetEndPointAddress(path),
        data: requestdata,
        type: 'GET',
        success: function (response) {

            //if (Helpers.Utility.HasValue(viewModel))
            //    viewModel.EndLoading();

            onSuccess(response, successCallback, errorCallback);
        },
        error: function (xhr, status, error) {
            onError(xhr, status, error, errorCallback);
        },
        beforeSend: function (xhr) {
            //TODO: Uncomment when using authentication
            //xhr.setRequestHeader("Authorization", "Bearer " + WebApiClient.GetAccessToken());
        }
    });

    return jqxhr;
}

function _Post(path, requestdata, successCallback, errorCallback, viewModel) {

    if (Helpers.Utility.HasValue(viewModel))
        viewModel.StartProcessing();

    var jqxhr = $.ajax({
        url: WebApiClient.GetEndPointAddress(path),
        data: JSON.stringify(requestdata),
        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (response) {

            if (Helpers.Utility.HasValue(viewModel))
                viewModel.EndProcessing();

            onSuccess(response, successCallback, errorCallback);
        },
        error: function (xhr, status, error) {
            onError(xhr, status, error, errorCallback);
        },
        beforeSend: function (xhr) {
            //TODO: Uncomment when using authentication
            //xhr.setRequestHeader("Authorization", "Bearer " + WebApiClient.GetAccessToken());
        }
    });

    return jqxhr;
}

function _Put(path, requestdata, successCallback, errorCallback) {

    var jqxhr = $.ajax({
        url: WebApiClient.GetEndPointAddress(path),
        data: JSON.stringify(requestdata),
        type: 'PUT',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (response) {

            //if (Helpers.Utility.HasValue(viewModel)) {
            //    viewModel.EndProcessing();
            //    //Try releasing lock
            //    viewModel.ReleaseLock();
            //}

            onSuccess(response, successCallback, errorCallback);
        },
        error: function (xhr, status, error) {
            onError(xhr, status, error, errorCallback);
        },
        beforeSend: function (xhr) {
            //TODO: Uncomment when using authentication
            //xhr.setRequestHeader("Authorization", "Bearer " + WebApiClient.GetAccessToken());
        }
    });

    return jqxhr;
}
function _Delete(path, requestdata, successCallback, errorCallback) {

    var jqxhr = $.ajax({
        url: WebApiClient.GetEndPointAddress(path),
        data: JSON.stringify(requestdata),
        dataType: 'json',
        contentType: 'application/json;',
        type: 'DELETE',

        success: function (response) {
            onSuccess(response, successCallback, errorCallback);
        },
        error: function (xhr, status, error) {
            console.log(xhr, status, error);
            onError(xhr, status, error, errorCallback);
        },
        beforeSend: function (xhr) {
            //TODO: Uncomment when using authentication
            //xhr.setRequestHeader("Authorization", "Bearer " + WebApiClient.GetAccessToken());
        }
    });

    return jqxhr;
}

//function onSuccess(response, successCallback, errorCallback) {
//    if (response.success) {
//        try {
//            if (Helpers.Utility.HasValue(successCallback)) {
//                // return successCallback({ Success: true, Payload: response });
//                return successCallback(response.payload);
//            } else {
//                return true;
//            }
//        } catch (ex) {
//            console.error(ex);
//        }
//    }
//    else if (response.Success === false) {
//        if (Helpers.Utility.HasValue(errorCallback)) {
//            errorCallback(response);
//        } else {
//            console.error(response);
//        }
//    }
//    else {
//        console.error(response);
//    }
//}

function onSuccess(response, successCallback, errorCallback) {
    try {
        if (Helpers.Utility.HasValue(successCallback)) {
            return successCallback(response);
        } else {
            return true;
        }
    } catch (ex) {
        console.error(ex);
    }
}

function onError(xhr, status, error, errorCallback) {

    if (xhr.responseText !== undefined) {

        try {

            responseText = JSON.parse(xhr.responseText);

            let problemDetail = null;

            if (Helpers.Utility.HasValue(responseText.ResponseText)) {
                try {
                    problemDetail = JSON.parse(responseText.ResponseText);
                }
                catch{
                    problemDetail = null;
                }
            }

            if (problemDetail === null) {
                toastr.error(responseText.Message, "Error (" + responseText.StatusCode + ")", {
                    closeButton: true
                });
            }
            else {

                let errorString = "";

                Object.keys(problemDetail.Errors).forEach(function (name) {
                    errorString += "<p>" + problemDetail.Errors[name][0] + "</p>";
                });

                toastr.warning(errorString, problemDetail.Title, {
                    closeButton: true
                });

                console.log(problemDetail);
            }

        }
        catch (e) {
            toastr.error("An error has occured while processing your request, please try later.", "Error", {
                closeButton: true
            });
        }
    }
    else if (error !== undefined) {
        toastr.error("Error Code: " + error.code + "<br>Messaeg: " + error.message, error.name, {
            closeButton: true
        });
    }

    if (Helpers.Utility.HasValue(errorCallback))
        errorCallback(status, error);
}

