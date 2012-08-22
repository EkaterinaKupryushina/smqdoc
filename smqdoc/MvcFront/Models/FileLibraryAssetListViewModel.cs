using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class FileLibraryAssetListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long AssetId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Изменение")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Имя файла")]
        public string FileName { get; set; }
        [Display(Name = "Описание")]
        public string Comment { get; set; }

        public FileLibraryAssetListViewModel()
        {
        }

        public FileLibraryAssetListViewModel(FileLibraryAsset asset)
        {
            if(asset != null)
            {
                AssetId = asset.filelibraryassetid;
                Name = asset.Name;
                FileName = asset.FileName;
                LastEditDate = asset.LastEditDate;
                Comment = asset.Comment;
            }
        }

        public static FileLibraryAssetListViewModel AssetsToModelConverter(FileLibraryAsset templ)
        {
            return new FileLibraryAssetListViewModel(templ);
        }
    }
}