using System.Data.Objects;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Asset GetAssetById(long id)
        {
            if (id == 0)
                return new Asset();
            return _unitOfWork.DbModel.Assets.SingleOrDefault(x => x.assetid == id);
        }

        public IQueryable<Asset> GetAllAssets()
        {
            return _unitOfWork.DbModel.Assets;
        }

        public void SaveAsset(Asset asset)
        {
            if (asset.assetid == 0)
            {
                _unitOfWork.DbModel.Assets.AddObject(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
            else
            {
                _unitOfWork.DbModel.Assets.ApplyCurrentValues(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
        }

        public void DeleteAsset(long assetId)
        {
            var asset = GetAssetById(assetId);
            _unitOfWork.DbModel.Assets.DeleteObject(asset);
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}