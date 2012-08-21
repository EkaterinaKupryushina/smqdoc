using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IAssetRepository
    {
        /// <summary>
        /// Возвращает asset по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Asset GetAssetById(long id);

        /// <summary>
        /// Возвращает список всех доступных asset
        /// </summary>
        /// <returns></returns>
        IQueryable<Asset> GetAllAssets();

        /// <summary>
        /// Создать или сохранить asses
        /// </summary>
        /// <param name="asset"></param>
        void SaveAsset(Asset asset);

        /// <summary>
        /// Удалить asset
        /// </summary>
        /// <param name="assetId"></param>
        void DeleteAsset(long assetId);

    }
}