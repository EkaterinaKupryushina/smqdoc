using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class UserDocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocAppointmentRepository _appointmentRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;
        public UserDocumentController(IDocAppointmentRepository appointmentRepository, IDocumentRepository documentRepository, IDocTemplateRepository docTemplateRepository)
        {
            _documentRepository = documentRepository;
            _appointmentRepository = appointmentRepository;
            _docTemplateRepository = docTemplateRepository;
        }

       /// <summary>
       /// Страница необходимых к заполненеию документов
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
        /// Страница документов пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDocAppointments()
        {
            return View();
        }

       
        /// <summary>
        /// Страница просмотра инфомрации о документе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DocumentDetails(long id)
        {
            try
            {
                return View(_documentRepository.GetDocumentById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.DocumentDetails()", ex);
                return View(new Document());
            }
        }

        /// <summary>
        /// Отключение заполнения формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DisableDocAppointment(long id)
        {
            try
            {
                return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.DisableDocAppointment()", ex);
                return View(new DocAppointmentEditModel());
            }
        }

        /// <summary>
        /// Отключение заполнения формы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"> </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DisableDocAppointment(long id, FormCollection collection)
        {
            try
            {
                _appointmentRepository.ChangeDocAppointmentState(id);

                return RedirectToAction("UserDocAppointments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.DisableDocAppointment()", ex);
                return View(new DocAppointmentEditModel());
            }
        }
        /// <summary>
        /// Выбор шаблона для создания назначения документа
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectDocTemplate()
        {
            try
            {
                return
                    View(
                        _docTemplateRepository.GetAllDocTeplates().Where(
                            x => x.Status == (int) DocTemplateStatus.Active && x.DocTemplatesForUser != null).ToList().
                            ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.SelectDocTemplate()", ex);
                return View(new List<DocTemplateListViewModel>());
            }
        }

        /// <summary>
        /// Создание привязки к группе
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateUserAppointment(long docTemplateId)
        {
            try
            {
                var newAppointment = new DocAppointment
                                         {DocTemplate = _docTemplateRepository.GetDocTemplateById(docTemplateId)};
                return View(new DocAppointmentEditModel(newAppointment));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.CreateUserAppointment()", ex);
                return View(new DocAppointmentEditModel());
            }

        }

        /// <summary>
        /// Создание привязки к группе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUserAppointment(DocAppointmentEditModel model)
        {
            try
            {
                long docAppId;
                if (ModelState.IsValid)
                {
                    var templ = new DocAppointment();
                    templ = model.Update(templ);
                    var sessData = SessionHelper.GetUserSessionData(Session);
                    templ.UserAccount_userid= sessData.UserId;
                    _appointmentRepository.SaveDocAppointment(templ);
                    docAppId = templ.docappointmentid;
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("CreateDocument", new { id = docAppId }); ;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.CreateUserAppointment()", ex);
                return View(model);
            }
        }
        

        /// <summary>
        /// Создание документа на основе назначения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateDocument(long id)
        {
            try
            {

                var sessData = SessionHelper.GetUserSessionData(Session);
                var doc = _documentRepository.CreateDocumentFromDocAppointment(id,sessData.UserId);
                if (doc != null)
                    return RedirectToAction("EditDocument", new { id = doc.documentid });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.CreateDocument()", ex);
            }
            return RedirectToAction("Index");

        }


        /// <summary>
        /// Страница редактированяи документов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditDocument(int id)
        {
            try
            {
                // Удаляем Провайдр валидации типов со стороны клиента, чтобы не появлялось сообщение на англ.
                // The field xxxx must be a number
                foreach (ModelValidatorProvider prov in ModelValidatorProviders.Providers)
                {
                    if (prov.GetType() == typeof (ClientDataTypeModelValidatorProvider))
                    {
                        ModelValidatorProviders.Providers.Remove(prov);
                        break;
                    }
                }
                return View(new DocumentEditModel(_documentRepository.GetDocumentById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.EditDocument()", ex);
                return View(new DocumentEditModel());
            }
        }

        /// <summary>
        /// Страница редактированяи документов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDocument(DocumentEditModel model)
        {
            try
            {
                var doc = _documentRepository.GetDocumentById(model.DocumentId);
                foreach (var field in model.Fields.Where(x => x.FieldType != (int)FieldTemplateType.Calculated))
                {
                    {
                        field.Update(doc.DocFields.Single(x => x.docfieldid == field.FieldId));
                    } 
                }
                foreach (var field in model.Fields.Where(x => x.FieldType == (int)FieldTemplateType.Calculated))
                {
                    {
                        field.Update(doc.DocFields.Single(x => x.docfieldid == field.FieldId), doc);
                    }
                }

                if (Request.Form["calculate"] != null)
                {

                    return View(new DocumentEditModel(doc));
                }
                _documentRepository.SaveDocument(doc);
                if (Request.Form["send"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _documentRepository.ChangeDocumentStatus(doc.documentid, doc.DocStatus ==
                                                                            DocumentStatus.PlanEditing
                                                                                ? DocumentStatus.PlanSended
                                                                                : DocumentStatus.FactSended);
                    }
                    else
                    {
                        ModelState.AddModelError("Документ не дозаполнен","Заполните все поля");
                        return View(model);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController.EditDocument()", ex);
                return View(model);
            }
        }

        #region GridActions
       
        /// <summary>
        /// Список документов пользователя
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _UserDocumentsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _documentRepository.GetUserDocumentsByUserId(sessData.UserId)
                    .Where(x => x.Status != (int) DocumentStatus.Deleted)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

                return View(new GridModel<DocumentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController._UserDocumentsList()", ex);
                return View(new GridModel<DocumentListViewModel> {Data = new List<DocumentListViewModel>()});
            }
        }

        /// <summary>
        /// Список необхоидимых к заполнению документов
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _GroupDocAppointList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var allUserGroupDocs =
                    _documentRepository.GetUserDocumentsByUserId(sessData.UserId).Where(
                        x => x.Status != (int) DocumentStatus.Deleted
                             && x.DocAppointment.UserGroup_usergroupid == sessData.UserGroupId)
                        .Select(x => x.DocAppointment_docappointmentid).ToList();

                var data = _appointmentRepository.GetAllGroupDocAppointments(sessData.UserGroupId)
                    .Where(x => !allUserGroupDocs.Contains(x.docappointmentid))
                    .ToList().ConvertAll(DocAppointmentListViewModel.DocAppointmentToModelConverter).ToList();
                return View(new GridModel<DocAppointmentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController._GroupDocAppointList()", ex);
                return View(new GridModel<DocAppointmentListViewModel> { Data = new List<DocAppointmentListViewModel>() });
            }
        }

        /// <summary>
        /// Список необхоидимых к заполнению документов
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _UserDocAppointmentList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var allUserGroupDocs =
                    _documentRepository.GetUserDocumentsByUserId(sessData.UserId).Where(
                        x => x.Status != (int) DocumentStatus.Deleted
                             && x.DocAppointment.UserAccount_userid == sessData.UserId)
                        .Select(x => x.DocAppointment_docappointmentid).ToList();

                var data = _appointmentRepository.GetAllUserDocAppointments(sessData.UserId)
                    .Where(
                        x =>
                        ((!allUserGroupDocs.Contains(x.docappointmentid) &&
                          !x.DocTemplate.DocTemplatesForUser.AllowManyInstances) ||
                         x.DocTemplate.DocTemplatesForUser.AllowManyInstances))
                    .ToList().ConvertAll(DocAppointmentListViewModel.DocAppointmentToModelConverter).ToList();
                return View(new GridModel<DocAppointmentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController._UserDocAppointmentList()", ex);
                return View(new GridModel<DocAppointmentListViewModel> { Data = new List<DocAppointmentListViewModel>() });
            }
        }

        /// <summary>
        /// Список документов утвержденных
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _SubmittedUserDocumentsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _documentRepository.GetUserDocumentsByUserId(sessData.UserId, DocumentStatus.Submited)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

                return View(new GridModel<DocumentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserDocumentController._SubmittedUserDocumentsList()", ex);
                return View(new GridModel<DocumentListViewModel> { Data = new List<DocumentListViewModel>() });
            }
        }
        #endregion
    }
}
