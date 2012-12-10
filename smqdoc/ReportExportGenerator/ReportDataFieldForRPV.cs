namespace ReportExportGenerator
{
    //Класс для отображения данных в виде матрицы в  ReportViewer
    public class ReportDataFieldForRPV
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }

        //HAck что бы передавать имя отчета и его описание как датасорс в отчет
        public string ReportName { get; set; }
        public string Legend { get; set; }
    }
}
