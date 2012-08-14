using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
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
            return View(_documentRepository.GetDocumentById(docId));
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
            var doc = _documentRepository.GetDocumentById(docId);
            if (collection.AllKeys.Contains("LastComment"))
                doc.LastComment = collection.GetValue("LastComment").AttemptedValue;
            doc = _documentRepository.SaveDocument(doc);
            return View(doc);
        }

        /// <summary>
        /// Принять документ
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public ActionResult Submit(long docId)
        {
            var doc = _documentRepository.GetDocumentById(docId);
            if(doc.DocStatus == DocumentStatus.FactSended)
            {
                _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.Submited);
            }
            if (doc.DocStatus == DocumentStatus.PlanSended)
            {
                _documentRepository.ChangeDocumentStatus(docId, DocumentStatus.FactEditing);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Вернуть документ на доработку
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public ActionResult Dismiss(long docId)
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

        #region GridAction

        /// <summary>
        /// Список групповых документов 
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SendedGroupDocumentsList()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            var data = _documentRepository.GetGroupDocumentsByGroupId(sessData.UserGroupId)
                .Where(x => x.Status == (int)DocumentStatus.FactSended || x.Status == (int)DocumentStatus.PlanSended)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

            return View(new GridModel<DocumentListViewModel> { Data = data });
        }

        /// <summary>
        /// Список личных документов
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SendedUserDocumentsList()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            var data = _documentRepository.GetPersonalDocumentsByGroupId(sessData.UserGroupId)
                .Where(x => x.Status == (int)DocumentStatus.FactSended || x.Status == (int)DocumentStatus.PlanSended)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

            return View(new GridModel<DocumentListViewModel> { Data = data });
        }

        /// <summary>
        /// Список документов пользователей группы
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SubmittedUserDocumentsList()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            var data = _documentRepository.GetGroupDocumentsByGroupId(sessData.UserGroupId, DocumentStatus.Submited).Union(_documentRepository.GetPersonalDocumentsByGroupId(sessData.UserGroupId, DocumentStatus.Submited))
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

            return View(new GridModel<DocumentListViewModel> { Data = data });
        }
        #endregion
    }
}