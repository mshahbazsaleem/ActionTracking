(function (window) {
    'use strict';

    /****************************************************************************/
    //                          SingalR Clinet
    /****************************************************************************/

    function SignalRClientController(endpoint, callbackEvents, callbacks) {

        let vm = this;

        vm.SignalRConnection = null;

        vm.Initialize = function () {

            if (!endpoint) {
                toastr.error("Endpoint missing", "SignalR Connection Error", {
                    closeButton: true
                });
                return console.error("Endpoint missing");
            }

            if (!callbackEvents) {
                toastr.error("Callback event missing", "SignalR Connection Error", {
                    closeButton: true
                });
                return console.error("Callback event missing");
            }

            if (!callbacks) {
                toastr.error("Callback method missing", "SignalR Connection Error", {
                    closeButton: true
                });
                return console.error("Callback method missing");
            }

            vm.SignalRConnection = new signalR.HubConnectionBuilder().withUrl(endpoint).build();

            vm.SignalRConnection.start().catch(function (err) {
                toastr.error(err.toString(), "SignalR Connection Error", {
                    closeButton: true
                });
                return console.error(err.toString());
            });

            //If we have come this far everything's good.
            //Time to register callback
            for (var i = 0; i < callbackEvents.length; i++)
                vm.SignalRConnection.on(callbackEvents[i], callbacks[i]);

            return vm.SignalRConnection;
        };
    }

    //Export globally
    window.SignalRClientController = SignalRClientController;

})(window);
