using MvcFront.Enums;

namespace MvcFront.DB
{
    public partial class ReportField
    {
        public ReportFieldOperationType ReportFieldOperationType
        {
            get { return (ReportFieldOperationType) ReportOperationType; }
            set { ReportOperationType = (int) value; }
        }
    }
}