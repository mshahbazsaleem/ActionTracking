using Newtonsoft.Json;
using System;

namespace MAA.ActionTracking.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string ResponseText { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}