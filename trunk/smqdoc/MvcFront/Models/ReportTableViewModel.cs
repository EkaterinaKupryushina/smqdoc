using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.DB;
using MvcFront.Entities;

namespace MvcFront.Models
{
    public class ReportTableViewModel
    {
        public string Name { get { return DocReport != null ? DocReport.Name : string.Empty; } }
        public string Legend { get { return DocReport != null ? DocReport.Legend : string.Empty; } }
        public DocReport DocReport { get; set; }
        public List<ReportDataRow> Rows { get; private set; }
        public ReportDataRow TotalRow { get; set; }

        public ReportTableViewModel()
        {
            Rows = new List<ReportDataRow>();
        }
    }
}