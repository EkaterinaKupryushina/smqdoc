using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class AssetFolderEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 AssetFolderId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Корневая папка")]
        public bool IsRootFolder { get; set; }

        [Display(Name = "Папка")]
        public int? ParentAssetFolderId { get; set; }

        public AssetFolderEditModel()
        {
        }

        public AssetFolderEditModel(AssetFolder folder)
        {
            AssetFolderId = folder.assetfolderid;
            Name = folder.Name;
            IsRootFolder = folder.AssetFolder_assetfolderid == null;
            ParentAssetFolderId = folder.AssetFolder_assetfolderid;
        }

        public AssetFolder Update(AssetFolder folder)
        {
            folder.Name = Name;
            if(IsRootFolder || !ParentAssetFolderId.HasValue)
            {
                folder.AssetFolder_assetfolderid = null;
            }
            else
            {
                folder.AssetFolder_assetfolderid = ParentAssetFolderId.Value;
            }
            return folder;
        }
    }
}