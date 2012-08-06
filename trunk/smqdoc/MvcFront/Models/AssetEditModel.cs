using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class AssetEditModel :IValidatableObject
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 AssetId { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Папка")]
        public int AssetFolderId { get; set; }

        [Required]
        [Display(Name = "Файл")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public AssetEditModel()
        {
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Files == null || Files.Count() != 1 )
            {
                    yield return new ValidationResult("Выберите файл");
            }
        }
    }
}