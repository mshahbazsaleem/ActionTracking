using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Helpers
{
    public class JsonResponseWrapper
    {
        private bool? _success;

        public JsonResponseWrapper()
        {
            AlertMessageType = Alerts.ERROR;
            AlertMessageName = "ERROR";
            AlertMessage = "Operation status not confirmed.";
        }

        public object Payload { get; set; }

        public bool? Success
        {
            get { return _success; }
            set
            {
                _success = value;
                if (_success == true)
                {
                    AlertMessage = string.Empty;
                }
            }
        }

        public string AlertMessageType { get; set; }

        public string AlertMessageName { get; set; }

        public string AlertMessage { get; set; }

        public string TrackingId { get; set; }

        public string TrackingLink
        {
            get { return string.Empty; }
            //get { return !string.IsNullOrEmpty(TrackingId) ? LoggingInfo.Link.Replace("TRACKINGID", TrackingId) : string.Empty; }
        }

        public string NavigateToUrl { get; set; }

        public object RouteValues { get; set; }
    }

    public static class Alerts
    {
        public const string SUCCESS = "success";
        public const string WARNING = "warning";
        public const string ERROR = "danger";
        public const string INFORMATION = "info";

        public static string[] ALL
        {
            get
            {
                return new[]
                {
                    SUCCESS, WARNING, INFORMATION, ERROR
                };
            }
        }
    }
}
