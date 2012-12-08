namespace MvcFront.Entities
{
    //Класс для отображения данных в виде матрицы в  ReportViewer
    public class ReportDataFieldForRPV
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
    }
}