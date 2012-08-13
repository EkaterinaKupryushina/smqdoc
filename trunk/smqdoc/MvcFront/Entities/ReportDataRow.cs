using System.Collections.Generic;

namespace MvcFront.Entities
{
    /// <summary>
    /// ������ ��� ����������� � ������
    /// </summary>
    public class ReportDataRow
    {
        /// <summary>
        /// �������� ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �������� ������
        /// </summary>
        public Dictionary<long, string> Values { get; set; }

        public ReportDataRow()
        {
            Name = string.Empty;
            Values = new Dictionary<long, string>();
        }
    }
}