using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class AssetEditModel
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
        public HttpPostedFile File { get; set; }

        public AssetEditModel()
        {
        }
    }
}