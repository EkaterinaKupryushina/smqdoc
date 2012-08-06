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
        [Display(Name = "��������")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "�����")]
        public int AssetFolderId { get; set; }

        [Required]
        [Display(Name = "����")]
        public HttpPostedFile File { get; set; }

        public AssetEditModel()
        {
        }
    }
}