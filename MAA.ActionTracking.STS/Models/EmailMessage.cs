using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Models
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
