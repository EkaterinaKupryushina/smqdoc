using System.Collections.Generic;

namespace MvcFront.Entities
{
    /// <summary>
    /// Строка для отображения в отчете
    /// </summary>
    public class ReportDataRow
    {
        /// <summary>
        /// Название строки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значения строки
        /// </summary>
        public Dictionary<long, string> Values { get; set; }

        public ReportDataRow()
        {
            Name = string.Empty;
            Values = new Dictionary<long, string>();
        }
    }
}