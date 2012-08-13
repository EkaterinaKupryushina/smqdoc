using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Infrastructure;
using MvcFront.Interfaces;
using System.Linq;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IAssetRepository _assetRepository;

        public LibraryController(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        /// <summary>
        /// Список всех ассетов
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_assetRepository.GetAllAssetFolders().Where(x => x.AssetFolder_assetfolderid == null));
        }

        /// <summary>
        /// Страница редактирования библиотеки документов
        /// </summary>
        /// <returns></returns>
        public ActionResult EditAssetLibrary()
        {
            return View(_assetRepository.GetAllAssetFolders().Where(x => x.AssetFolder_assetfolderid == null));
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAsset(int id)
        {
            return View(new AssetEditModel{AssetFolderId = id});
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAsset(AssetEditModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    (new AssetService(_assetRepository)).CreateNewAsset(model.Files.ElementAt(0), model.AssetFolderId,model.Comment);
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("EditAssetLibrary");
        }


        /// <summary>
        /// Создание папки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAssetFolder()
        {
            return View(new AssetFolderEditModel { AssetFolderId = 0 });
        }

        /// <summary>
        /// Создание папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAssetFolder(AssetFolderEditModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var assetFolder = model.Update(new AssetFolder());
                    _assetRepository.SaveAssetFolder(assetFolder);
                    return RedirectToAction("EditAssetLibrary");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка",ex);
                return View(model);
            }
            return View(model);
        }

        /// <summary>
        /// Редактирование папки 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAssetFolder(int id)
        {
            return View(new AssetFolderEditModel(_assetRepository.GetAssetFolderById(id)));
        }

        /// <summary>
        /// Редактированеи папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAssetFolder(AssetFolderEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var assetFolder = model.Update(_assetRepository.GetAssetFolderById(model.AssetFolderId));
                    _assetRepository.SaveAssetFolder(assetFolder);
                    return RedirectToAction("EditAssetLibrary");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex);
                return View(model);
            }
            return View(model);
        }

        /// <summary>
        /// Удаление папки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAssetFolder(int id)
        {
            try
            {
                (new AssetService(_assetRepository)).DeleteAssetFolder(_assetRepository.GetAssetFolderById(id));
            }
            catch (Exception)
            {
                return RedirectToAction("EditAssetLibrary");
            }
            return RedirectToAction("EditAssetLibrary");
        }

        /// <summary>
        /// Возвращает Файл ассета
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public FileStreamResult  Download(int assetId)
        {
            var document = _assetRepository.GetAssetById(assetId);
            return File(new FileStream(Path.Combine(SmqSettings.Instance.AssetFolder, document.FileName), FileMode.Open), "application", document.Name);
        }

        #region JSon

        /// <summary>
        /// Удаление ассета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteAsset(int id)
        {
            try
            {
                (new AssetService(_assetRepository)).DeleteAsset(_assetRepository.GetAssetById(id));
            }
            catch (Exception)
            {
            }
            return Json(new { result = true });
        }

        /// <summary>
        /// Возвращает ассеты текущей папки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _AssetsFromFolder(int? id, string text)
        {
            var query = id.HasValue && id.Value != 0
                            ? _assetRepository.GetAssetFolderById(id.Value).Assets.AsQueryable()
                            : _assetRepository.GetAllAssets();
           
            var data = query.ToList().ConvertAll(AssetListViewModel.AssetsToModelConverter).ToList();
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = text.ToLowerInvariant();
                data = data.Where(x => x.Name.ToLowerInvariant().Contains(text) || x.Comment.ToLowerInvariant().Contains(text)).ToList();
            }
            return View(new GridModel<AssetListViewModel> { Data = data });
        }

        /// <summary>
        /// Список всех папок для редактирования
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _AssetFoldersGridList()
        {
            var data = _assetRepository.GetAllAssetFolders().ToList().ConvertAll(AssetFolderListViewModel.AssetFolderToModelConverter);
            return View(new GridModel<AssetFolderListViewModel> { Data = data });
        }

        /// <summary>
        /// Список всех папок для выбора
        /// </summary>
        /// <param name="assetFolderId"> </param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _AssetFoldersList(int assetFolderId)
        {
            var badAssFolderd = new List<int> {assetFolderId};
            
            var folder = _assetRepository.GetAssetFolderById(assetFolderId);
            var listOfChilds = new List<AssetFolder>();
            listOfChilds.AddRange(folder.ChildAssetFolders.ToList());
            while (listOfChilds.Count > 0)
            {
                var current = listOfChilds.ElementAt(0);
                badAssFolderd.Add(current.assetfolderid);
                listOfChilds.AddRange(current.ChildAssetFolders.ToList());
                listOfChilds.Remove(current);
            }
            var data = _assetRepository.GetAllAssetFolders().Where(x => !badAssFolderd.Contains(x.assetfolderid));
            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.assetfolderid, x.Name }), "Id", "Name", assetFolderId) };
        }

        #endregion
    }
}
