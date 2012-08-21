using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Infrastructure;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using System.Linq;
using MvcFront.Models;
using MvcFront.Services;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IFileLibaryRepository _fileLibaryRepository;
        private readonly IAssetRepository _assetRepository;

        public LibraryController(IAssetRepository assetRepository, IFileLibaryRepository fileLibaryRepository)
        {
            _assetRepository = assetRepository;
            _fileLibaryRepository = fileLibaryRepository;
        }

        /// <summary>
        /// Список всех ассетов
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                return View(_fileLibaryRepository.GetAllAssetFolders().Where(x => x.FileLibraryFolder_filelibraryfolderid == null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.Index()", ex);
                return View(new List<FileLibraryFolder>());
            }
        }

        /// <summary>
        /// Страница редактирования библиотеки документов
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult EditAssetLibrary()
        {
            try
            {
                return View(_fileLibaryRepository.GetAllAssetFolders().Where(x => x.FileLibraryFolder_filelibraryfolderid == null));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.EditAssetLibrary()", ex);
                return View(new List<FileLibraryFolder>());
            }
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminAuthorize]
        public ActionResult CreateAsset(int id)
        {
            try
            {
                return View(new FileLibraryAssetEditModel { AssetFolderId = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAsset()", ex);
                return View(new FileLibraryAssetEditModel());
            }
        }

        /// <summary>
        /// Создание ассета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize]
        public ActionResult CreateAsset(FileLibraryAssetEditModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    (new FileLibraryService(_assetRepository, _fileLibaryRepository)).CreateNewAsset(model.Files.ElementAt(0), model.AssetFolderId, model.Comment);
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
        [AdminAuthorize]
        public ActionResult CreateAssetFolder()
        {
            try
            {
                return View(new FileLibraryFolderEditModel { AssetFolderId = 0 });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.CreateAssetFolder()", ex);
                return View(new FileLibraryFolderEditModel());
            }
        }

        /// <summary>
        /// Создание папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize]
        public ActionResult CreateAssetFolder(FileLibraryFolderEditModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var assetFolder = model.Update(new FileLibraryFolder());
                    _fileLibaryRepository.SaveAssetFolder(assetFolder);
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
        [AdminAuthorize]
        public ActionResult EditAssetFolder(int id)
        {
            try
            {
                return View(new FileLibraryFolderEditModel(_fileLibaryRepository.GetAssetFolderById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController.EditAssetFolder()", ex);
                return View(new FileLibraryFolderEditModel());
            }
        }

        /// <summary>
        /// Редактированеи папки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize]
        public ActionResult EditAssetFolder(FileLibraryFolderEditModel model)
        {
            if (ModelState.IsValid)
                {
                    try
                    {
                        var assetFolder = model.Update(_fileLibaryRepository.GetAssetFolderById(model.AssetFolderId));
                        _fileLibaryRepository.SaveAssetFolder(assetFolder);
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
        [AdminAuthorize]
        public ActionResult DeleteAssetFolder(int id)
        {
            try
            {
                (new FileLibraryService(_assetRepository, _fileLibaryRepository)).DeleteFileAssetFolder(id);
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
        [AdminAuthorize]
        public FileStreamResult Download(int assetId)
        {
            try
            {
                var document = _fileLibaryRepository.GetAssetById(assetId);
                return
                    File(
                        new FileStream(Path.Combine(SmqSettings.Instance.AssetFolder, document.Asset.FileName), FileMode.Open),
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
                //TODO Fix
                //(new FileLibraryService(_assetRepository, _fileLibaryRepository)).DeleteFileAssetFolder(_assetRepository.GetAssetById(id)););
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

                var folder = _fileLibaryRepository.GetAssetFolderById(assetFolderId);
                var listOfChilds = new List<FileLibraryFolder>();
                listOfChilds.AddRange(folder.ChildFileLibraryFolders.ToList());
                while (listOfChilds.Count > 0)
                {
                    var current = listOfChilds.ElementAt(0);
                    badAssFolderd.Add(current.filelibraryfolderid);
                    listOfChilds.AddRange(current.ChildFileLibraryFolders.ToList());
                    listOfChilds.Remove(current);
                }
                var data = _fileLibaryRepository.GetAllAssetFolders().Where(x => !badAssFolderd.Contains(x.filelibraryfolderid));
                return new JsonResult
                           {
                               Data =
                                   new SelectList(data.ToList().Select(x => new {Id = x.filelibraryfolderid, x.Name}), "Id",
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
                                ? _fileLibaryRepository.GetAssetFolderById(id.Value).FileLibraryAssets.AsQueryable()
                                : _fileLibaryRepository.GetAllAssets();

                var data = query.ToList().ConvertAll(FileLibraryAssetListViewModel.AssetsToModelConverter).ToList();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    text = text.ToLowerInvariant();
                    data =
                        data.Where(
                            x => x.Name.ToLowerInvariant().Contains(text) || x.Comment.ToLowerInvariant().Contains(text))
                            .ToList();
                }
                return View(new GridModel<FileLibraryAssetListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController._AssetsFromFolder()", ex);
                return View(new GridModel<FileLibraryAssetListViewModel> { Data = new List<FileLibraryAssetListViewModel>() });
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
                    _fileLibaryRepository.GetAllAssetFolders().ToList().ConvertAll(
                        FileLibraryFolderListViewModel.AssetFolderToModelConverter);
                return View(new GridModel<FileLibraryFolderListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "LibraryController._AssetFoldersGridList()", ex);
                return View(new GridModel<FileLibraryFolderListViewModel> { Data = new List<FileLibraryFolderListViewModel>() });
            }
        }

        #endregion
    }
}
