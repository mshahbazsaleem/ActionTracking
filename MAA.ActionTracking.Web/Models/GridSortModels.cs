using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAA.ActionTracking.Web.Models
{
    /*
<?xml version="1.0" encoding="UTF-8"?>

<!-- PGL Sort Structure -->

<PGLSort>

   <PGLSortRow>

       <sort_column>geographic_region_3</sort_column>

       <sort_direction>asc</sort_direction>

   </PGLSortRow>

   <PGLSortRow>

       <sort_column>starting_salary_amt</sort_column>

       <sort_direction>desc</sort_direction>

   </PGLSortRow>

</PGLSort>*/
    public class GridSort
    {
        [XmlArrayItem("PGLSort")]
        public List<SortRow> SortRows { get; set; }
    }
    [XmlRoot("PGLSort")]
    [XmlType("PGLSortRow")]
    public class SortRow
    {
        [XmlElement(ElementName = "sort_column")]
        public string SortColumn { get; set; }
        [XmlElement(ElementName = "sort_direction")]
        public string SortDirection { get; set; }
    }
}
