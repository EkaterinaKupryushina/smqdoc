using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ELW.Library.Math.Tools;
using MvcFront.DB;
using MvcFront.Helpers;
using MvcFront.Infrastructure;

namespace MvcFront.Models
{
    public class DocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        //[Display(Name = "Название документа")]
        //public string DocumentName { get; set; }
        [Display(Name = "Название документа")]
        public string GroupTemplateName { get; set; }
        [Display(Name = "Статус документа")]
        public string DocumentStatusText { get; set; }
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Окончание заполенния")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Status")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }
        [Display(Name = "Coloring")]
        [UIHint("Hidden")]
        public bool IsRed { get; set; }
        public DocumentListViewModel()
        {
        }
        public DocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;
            //DocumentName = templ.DocumentName;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            IsReadOnly = templ.DocStatus != DocumentStatus.Editing;
            DateEnd = templ.GroupTemplate.DateEnd;
            IsRed = templ.GroupTemplate.DateEnd < DateTime.Now.AddDays(SmqSettings.Instance.DocumentsDedlineWarning) &&
                    templ.Status == (int)DocumentStatus.Editing;

            GroupTemplateName = templ.GroupTemplate.Name;

        }
        public static DocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new DocumentListViewModel(templ);
        }
    }

    public class DocumentEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        [Display(Name = "Название документа")]
        public string GroupTemplateName { get; set; }
        [Display(Name = "Статус документа")]
        public string DocumentStatusText { get; set; }
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }

        public List<DocFieldEditModel> Fields { get; set; }
        
        public DocumentEditModel()
        {
            Fields = new List<DocFieldEditModel>();
        }
        public DocumentEditModel(Document templ)
            : this()
        {
            DocumentId = templ.documentid;
            GroupTemplateName = templ.GroupTemplate.Name;
            //DocumentName = templ.DocumentName;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            Fields = templ.DocFields.Where(x => x.FieldTemplate.TemplateStatus == FieldTemplateStatus.Active)
                .OrderBy(x => x.FieldTemplate.OrderNumber).ToList().ConvertAll(DocFieldEditModel.FieldToModelConverter).ToList();
        }
        public static DocumentEditModel DocumentToModelConverter(Document templ)
        {
            return new DocumentEditModel(templ);
        }

    }
    public class DocFieldEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long FieldId { get; set; }
        [Display(Name = "Название поля")]
        public string FieldName { get; set; }
        [Display(Name = "Порядковый номер")]
        public int OrderNumber { get; set; }
        [Display(Name = "Тип поля")]
        public int FieldType { get; set; }
        [Display(Name = "Значение")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string StringValue { get; set; }
        [Display(Name = "Значение")]
        [DataType("Number")]
        [Required]
        public double? DoubleValue { get; set; }
        [Display(Name = "Значение")]
        [DataType("Number")]
        [Required]
        public int? IntegerValue { get; set; }
        [Display(Name = "Поле ограничено?")]
        public bool IsRestricted { get; set; }
        [Display(Name = "Поле Целое?")]
        public bool IsInteger { get; set; }
        [Display(Name = "Максимальное значение")]
        [DataType("Number")]
        public double? MaxVal { get; set; }
        [DataType("Number")]
        [Display(Name = "Минимальное значение")]
        public double? MinVal { get; set; }
        [Display(Name = "Значение")]
        [Required]
        public bool BoolValue { get; set; }
        //[Display(Name = "Операция вычисления")]
        //public int? CalcOperationType { get; set; }
        //[UIHint("Hidden")]
        //public List<long> ComputebleFieldIds { get; set; }

        public DocFieldEditModel()
        {
            StringValue = "";
            BoolValue = false;
            DoubleValue = 0;
        }
        public DocFieldEditModel(DocField item)
        {
            FieldId = item.docfieldid;
            FieldName = item.FieldTemplate.FieldName;
            FieldType = item.FieldTemplate.FiledType;
            IsRestricted = item.FieldTemplate.Restricted.HasValue && item.FieldTemplate.Restricted.Value;
            MaxVal = item.FieldTemplate.MaxVal;
            MinVal = item.FieldTemplate.MinVal;
            StringValue = item.StringValue;
            BoolValue = item.BoolValue ?? false;
            DoubleValue = item.DoubleValue;
            OrderNumber = item.FieldTemplate.OrderNumber;
            IsInteger = item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value;
            if(IsInteger && item.DoubleValue.HasValue)
            {
                IntegerValue = (int) item.DoubleValue.Value;
            }
            //if(item.FieldTemplate.TemplateType == FieldTemplateType.CALCULATED)
            //{
            //    ComputebleFieldIds = new List<long>();
            //    foreach (var part in item.FieldTemplate.ComputableFieldTemplateParts)
            //    {
            //        var field = item.Document.DocFields.FirstOrDefault(x => x.FieldTemplate_fieldteplateid == part.fkCalculatedFieldTemplateID);
            //        if (field != null && field.FieldTemplate.TemplateType == FieldTemplateType.NUMBER)
            //            ComputebleFieldIds.Add(field.docfieldid);
            //    }
            //}
        }
        public DocField Update(DocField item, Document doc = null)
        {
            switch ((FieldTemplateType)FieldType)
            {
                case FieldTemplateType.BOOL:
                    item.BoolValue = BoolValue;
                    item.StringValue = null;
                    item.DoubleValue = null;
                    break;
                case FieldTemplateType.NUMBER:
                    item.BoolValue = null;
                    item.StringValue = null;
                    if (item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value)
                    {
                        item.DoubleValue = IntegerValue;
                    }
                    else
                    {
                        item.DoubleValue = DoubleValue;
                    }
                    break;
                case FieldTemplateType.STRING:
                    item.BoolValue = null;
                    item.StringValue = StringValue;
                    item.DoubleValue = null;
                    break;
                case FieldTemplateType.CALCULATED:
                    if (doc != null)
                        DoubleValue = CalculateValue(doc, item);

                    item.BoolValue = null;
                    item.StringValue = null;
                    item.DoubleValue = DoubleValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return item;
        }

        private Double? CalculateValue(Document doc, DocField field)
        {
            Double? res = 0;
            List<long> lstFieldTemplateIDs = field.FieldTemplate.ComputableFieldTemplateParts
                                                                .Where(x => x.FieldTemplate_fieldteplateid == field.FieldTemplate_fieldteplateid)
                                                                .Select(x => x.fkCalculatedFieldTemplateID).ToList();

            var lstValues = doc.DocFields
                                          .Where(x => lstFieldTemplateIDs.Contains(x.FieldTemplate.fieldteplateid) && x.DoubleValue.HasValue)
                                          .Select(x => new VariableValue ( x.DoubleValue.Value,string.Format("{{{0}}}",x.FieldTemplate.fieldteplateid) )).ToList();
            try
            {

                res = CalculationHelper.CalculateExpression(field.FieldTemplate.OperationExpression,lstValues);
            }
            catch(Exception)
            {
            }
            return res;
        }

        public static DocFieldEditModel FieldToModelConverter(DocField templ)
        {
            return new DocFieldEditModel(templ);
        }
    }
}