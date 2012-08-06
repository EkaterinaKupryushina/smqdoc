using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class AssetListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public int AssetId { get; set; }
        [Display(Name = "��������")]
        public string Name { get; set; }
        [Display(Name = "��������� ���������")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "��� �����")]
        public string FileName { get; set; }
        [Display(Name = "��������")]
        public string Comment { get; set; }

        public AssetListViewModel()
        {
        }

        public AssetListViewModel(Asset asset)
        {
            if(asset != null)
            {
                AssetId = asset.assetid;
                Name = asset.Name;
                FileName = asset.FileName;
                LastEditDate = asset.LastEditDate;
                Comment = asset.Comment;
            }
        }

        public static AssetListViewModel AssetsToModelConverter(Asset templ)
        {
            return new AssetListViewModel(templ);
        }
    }
}