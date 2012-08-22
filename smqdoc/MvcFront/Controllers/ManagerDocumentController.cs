using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Infrastructure;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [GroupManagerAuthorize]
    public class ManagerDocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        public ManagerDocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Список документов отправленных на подтверждение
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///Страница со списоком потдеврженных пользовательских докментов
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitedUserDocuments()
        {
            return View();
        }

        /// <summary>
        /// Просмотр информации о документе
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewDocumentDetails(long docId)
        {
            try
            {
                return View(_documentRepository.GetDocumentById(docId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController.ViewDocumentDetails()", ex);
                return View(new Document());
            }
        }

        /// <summary>
        /// Просмотр информации о документе
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="collection"> </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ViewDocumentDetails(long docId, FormCollection collection)
        {
            try
            {
                var doc = _documentRepository.GetDocumentById(docId);
                if (collection.AllKeys.Contains("LastComment"))
                    doc.LastComment = collection.GetValue("LastComment").AttemptedValue;
                doc = _documentRepository.SaveDocument(doc);
                return View(doc);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController.ViewDocumentDetails()", ex);
                return View(new Document());
            }
        }

        /// <summary>
        /// Принять документ
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public ActionResult Submit(long docId)
        {
            try
            {
                var doc = _documentRepository.GetDocumentById(docId);
                if (doc.DocStatus == DocumentStatus.FactSended)
                {
                    _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.Submited);
                }
                if (doc.DocStatus == DocumentStatus.PlanSended)
                {
                    _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.FactEditing);
                }
                return RedirectToAction("Index");
            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController.Submit()", ex);
                return RedirectToAction("ViewDocumentDetails",new {docId});
            }
        }

        /// <summary>
        /// Вернуть документ на доработку
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public ActionResult Dismiss(long docId)
        {
            try
            {
                var doc = _documentRepository.GetDocumentById(docId);
                if (doc.DocStatus == DocumentStatus.FactSended)
                {
                    _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.FactEditing);
                }
                if (doc.DocStatus == DocumentStatus.PlanSended)
                {
                    _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.PlanEditing);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController.Dismiss()", ex);
                return RedirectToAction("ViewDocumentDetails", new { docId });
            }
        }

        #region GridAction

        /// <summary>
        /// Список групповых документов 
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SendedGroupDocumentsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _documentRepository.GetGroupDocumentsByGroupId(sessData.UserGroupId)
                    .Where(
                        x => x.Status == (int) DocumentStatus.FactSended || x.Status == (int) DocumentStatus.PlanSended)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

                return View(new GridModel<DocumentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController._SendedGroupDocumentsList()", ex);
                return View(new GridModel<DocumentListViewModel> { Data = new List<DocumentListViewModel>() });
            }
        }

        /// <summary>
        /// Список личных документов
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SendedUserDocumentsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _documentRepository.GetPersonalDocumentsByGroupId(sessData.UserGroupId)
                    .Where(
                        x => x.Status == (int) DocumentStatus.FactSended || x.Status == (int) DocumentStatus.PlanSended)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

                return View(new GridModel<DocumentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController._SendedUserDocumentsList()", ex);
                return View(new GridModel<DocumentListViewModel> { Data = new List<DocumentListViewModel>() });
            }
        }

        /// <summary>
        /// Список документов пользователей группы
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SubmittedUserDocumentsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data =
                    _documentRepository.GetGroupDocumentsByGroupId(sessData.UserGroupId, DocumentStatus.Submited).Union(
                        _documentRepository.GetPersonalDocumentsByGroupId(sessData.UserGroupId, DocumentStatus.Submited))
                        .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

                return View(new GridModel<DocumentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerDocumentController._SubmittedUserDocumentsList()", ex);
                return View(new GridModel<DocumentListViewModel> { Data = new List<DocumentListViewModel>() });
            }
        }
        #endregion
    }
}