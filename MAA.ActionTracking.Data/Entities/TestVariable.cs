using MAA.ActionTracking.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAA.ActionTracking.Data.Entities
{
    public class TestVariable:EntityBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
