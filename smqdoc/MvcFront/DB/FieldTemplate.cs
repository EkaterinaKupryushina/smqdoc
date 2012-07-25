using System.ComponentModel.DataAnnotations;
using MvcFront.Enums;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    [MetadataType(typeof(FieldTemplateMetadata))]
    public partial class FieldTemplate
    {
        [Display(Name = "��� ���� �����")]
        public FieldTemplateType TemplateType
        {
            get
            {
                return (FieldTemplateType)FiledType;
            }
            set
            {
                FiledType = (int)value;
            }
        }
        [Display(Name = "��� ���� �����")]
        public string TemplateTypeText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(FieldTemplateType),FiledType);
            }
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
    public class FieldTemplateMetadata
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        [Required]
        [UIHint("Hidden")]
        public long fieldteplateid { get; set; }
        [Required]
        [Display(Name = "�������� ����")]
        public string FieldName { get; set; }
        [Required]
        [Display(Name = "��� ����")]
        public int FiledType { get; set; }
        [Required]
        [Display(Name = "����������?")]
        public bool Restricted { get; set; }
        [Display(Name = "������������ �������� ����")]
        public int MaxVal { get; set; }
        [Display(Name = "����������� �������� ����")]
        public int MinVal { get; set; }
        [Required]
        [Display(Name = "���������� ����� � �����")]
        public int OrderNumber { get; set; }
        [Required]
        [Display(Name = "������ ����")]
        public int Status { get; set; }
        [Display(Name = "������������ �����")]
        public DocTemplate DocTemplate { get; set; }
        [Display(Name = "�������� ��� ���������� ��������")]
        public int OperationType { get; set; }
        [Required]
        [Display(Name = "�����?")]
        public bool Integer { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
}