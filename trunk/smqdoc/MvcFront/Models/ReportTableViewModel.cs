﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.DB;
using MvcFront.Entities;

namespace MvcFront.Models
{
    public class ReportTableViewModel
    {
        public string Name { get; set; }
        public DocReport DocReport { get; set; }
        public List<ReportDataRow> Rows { get; set; }
    }
}