using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Infrastructure;
using MvcFront.Interfaces;
using System.Linq;
using MvcFront.Models;
using NLog;
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
            try
            {
                return View(_assetRepository.GetAllAssetFolders().Where(x => x.AssetFolder_assetfolderid == null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.Index()", ex);
                return View(new List<AssetFolder>());
            }
        }

        /// <summary>
        /// Страница редактирования библиотеки документов
        /// </summary>
        /// <returns></returns>
        public ActionResult EditAssetLibrary()
        {
            try
            {
                return View(_assetRepository.GetAllAssetFolders().Where(x => x.AssetFolder_assetfolderid == null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.EditAssetLibrary()", ex);
                return View(new List<AssetFolder>());
            }
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAsset(int id)
        {
            try
            {
                return View(new AssetEditModel { AssetFolderId = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAsset()", ex);
                return View(new AssetEditModel());
            }
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAsset(AssetEditModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    (new AssetService(_assetRepository)).CreateNewAsset(model.Files.ElementAt(0), model.AssetFolderId, model.Comment);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка");
                    LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAsset()", ex);
                    return View(model);
                }
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
            try
            {
                return View(new AssetFolderEditModel { AssetFolderId = 0 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAssetFolder()", ex);
                return View(new AssetFolderEditModel());
            }
        }

        /// <summary>
        /// Создание папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAssetFolder(AssetFolderEditModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var assetFolder = model.Update(new AssetFolder());
                    _assetRepository.SaveAssetFolder(assetFolder);
                    return RedirectToAction("EditAssetLibrary");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка");
                    LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAssetFolder()", ex);
                    return View(model);
                }
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
            try
            {
                return View(new AssetFolderEditModel(_assetRepository.GetAssetFolderById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.EditAssetFolder()", ex);
                return View(new AssetFolderEditModel());
            }
        }

        /// <summary>
        /// Редактированеи папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAssetFolder(AssetFolderEditModel model)
        {
            if (ModelState.IsValid)
                {
                    try
                    {
                        var assetFolder = model.Update(_assetRepository.GetAssetFolderById(model.AssetFolderId));
                        _assetRepository.SaveAssetFolder(assetFolder);
                        return RedirectToAction("EditAssetLibrary");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Произошла ошибка");
                        LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.EditAssetFolder()", ex);
                        return View(model);
                    }
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.DeleteAssetFolder()", ex);
            }
            return RedirectToAction("EditAssetLibrary");
        }

        /// <summary>
        /// Возвращает Файл ассета
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public FileStreamResult Download(int assetId)
        {
            try
            {
                var document = _assetRepository.GetAssetById(assetId);
                return
                    File(
                        new FileStream(Path.Combine(SmqSettings.Instance.AssetFolder, document.FileName), FileMode.Open),
                        "application", document.Name);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.Download()", ex);
                return null;
            }
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
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.DeleteAsset()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Список всех папок для выбора
        /// </summary>
        /// <param name="assetFolderId"> </param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _AssetFoldersList(int assetFolderId)
        {
            try
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
                return new JsonResult
                           {
                               Data =
                                   new SelectList(data.ToList().Select(x => new {Id = x.assetfolderid, x.Name}), "Id",
                                                  "Name", assetFolderId)
                           };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController._AssetFoldersList()", ex);
                return new JsonResult { Data = false };
            }
        }

        #endregion

        #region GridActions

        /// <summary>
        /// Возвращает ассеты текущей папки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _AssetsFromFolder(int? id, string text)
        {
            try
            {
                var query = id.HasValue && id.Value != 0
                                ? _assetRepository.GetAssetFolderById(id.Value).Assets.AsQueryable()
                                : _assetRepository.GetAllAssets();

                var data = query.ToList().ConvertAll(AssetListViewModel.AssetsToModelConverter).ToList();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    text = text.ToLowerInvariant();
                    data =
                        data.Where(
                            x => x.Name.ToLowerInvariant().Contains(text) || x.Comment.ToLowerInvariant().Contains(text))
                            .ToList();
                }
                return View(new GridModel<AssetListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController._AssetsFromFolder()", ex);
                return View(new GridModel<AssetListViewModel> { Data = new List<AssetListViewModel>() });
            }
        }

        /// <summary>
        /// Список всех папок для редактирования
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _AssetFoldersGridList()
        {
            try
            {
                var data =
                    _assetRepository.GetAllAssetFolders().ToList().ConvertAll(
                        AssetFolderListViewModel.AssetFolderToModelConverter);
                return View(new GridModel<AssetFolderListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController._AssetFoldersGridList()", ex);
                return View(new GridModel<AssetFolderListViewModel> { Data = new List<AssetFolderListViewModel>() });
            }
        }

        #endregion
    }
}
