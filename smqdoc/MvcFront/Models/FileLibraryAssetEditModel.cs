using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class FileLibraryAssetEditModel :IValidatableObject
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long AssetId { get; set; }

        [Required]
        [Display(Name = "��������")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "�����")]
        public int AssetFolderId { get; set; }

        [Required]
        [Display(Name = "����")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public FileLibraryAssetEditModel()
        {
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Files == null || Files.Count() != 1 )
            {
                    yield return new ValidationResult("�������� ����");
            }
        }
    }
}