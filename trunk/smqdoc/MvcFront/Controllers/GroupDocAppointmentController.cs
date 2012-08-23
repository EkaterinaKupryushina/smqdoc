using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [GroupManagerAuthorize]
    public class GroupDocAppointmentController : Controller
    {
        private readonly IDocAppointmentRepository _appointmentRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;
        private readonly IPersonalDocTemplateRepository _personalDocTemplateRepository;

        public GroupDocAppointmentController(IDocAppointmentRepository appointmentRepository, IDocTemplateRepository docTemplateRepository,IPersonalDocTemplateRepository personalDocTemplateRepository)
        {
            _appointmentRepository = appointmentRepository;
            _docTemplateRepository = docTemplateRepository;
            _personalDocTemplateRepository = personalDocTemplateRepository;
        }

        /// <summary>
        /// Список всех назначений текущей группы
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отображает инфомрацию о привязке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailsGroupDocAppointment(long id)
        {
            try
            {
                return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.Details()", ex);
                return View(new DocAppointmentEditModel());
            }
        }

        /// <summary>
        /// Показывает страницу выбора формы для привязки
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectDocTemplate()
        {
            try
            {
                    return View(_docTemplateRepository.GetAllDocTeplates().Where(
                            x => x.Status != (int) DocTemplateStatus.Deleted).ToList().ConvertAll(
                                DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.SelectDocTemplate()", ex);
                return View(new List<DocTemplateListViewModel>());
            }
        }

        /// <summary>
        /// Показывает страницу выбора формы для привязки
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectDocTemplateForPersonal()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var persDocTemplsIds =
                    _personalDocTemplateRepository.GetAllGroupPersonalDocTemplates(sessData.UserGroupId).Select(x => x.DocTemplate_docteplateid)
                    .Distinct().ToList();
                return View(_docTemplateRepository.GetAllDocTeplates().Where(
                        x => x.Status != (int)DocTemplateStatus.Deleted && !persDocTemplsIds.Contains(x.docteplateid)).ToList().ConvertAll(
                            DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.SelectDocTemplate()", ex);
                return View(new List<DocTemplateListViewModel>());
            }
        }

        /// <summary>
        /// Создание привязки к группе (песрональные дкоументы)
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatePersonalDocTemplate(long docTemplateId)
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var newPersDocTempl = new PersonalDocTemplate
                    {
                        DocTemplate_docteplateid = docTemplateId, 
                        UserGroup_usergroupid = sessData.UserGroupId,
                        personaldoctemplateid = 0
                    };
                _personalDocTemplateRepository.SavePersonalDocTemplate(newPersDocTempl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.CreateGroupAppointment()", ex);
                
            }
            return RedirectToAction("Index");
        } 

        /// <summary>
        /// Создание привязки к группе
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateGroupAppointment(long docTemplateId)
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.CreateGroupAppointment()", ex);
                return View(new  DocAppointmentEditModel());
            }
        } 

       /// <summary>
       /// Создание привязки к группе
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult CreateGroupAppointment(DocAppointmentEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = new DocAppointment();
                    templ = model.Update(templ);
                    var sessData = SessionHelper.GetUserSessionData(Session);
                    templ.UserGroup_usergroupid = sessData.UserGroupId;
                    _appointmentRepository.SaveDocAppointment(templ);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.CreateGroupAppointment()", ex);
                return View(model);
            }
        }
        
        /// <summary>
        /// Редактирование инфы о назначении
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditGroupDocAppointment(long id)
        {
            try
            {
                return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.Edit()", ex);
                return View(new DocAppointmentEditModel());
            }
        }

        /// <summary>
        /// Редактированеи инфы о назначении
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditGroupDocAppointment(DocAppointmentEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _appointmentRepository.GetDocAppointmentById(model.DocAppointmentId);
                    templ = model.Update(templ);
                    _appointmentRepository.SaveDocAppointment(templ);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
 
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.Edit()", ex);
                return View(model);
            }
        }

        /// <summary>
        /// Удаление Назначения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteGroupDocAppointment(long id)
        {
            try
            {
                return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.Delete()", ex);
                return View(new DocAppointmentEditModel());
            }
        }

       /// <summary>
       /// Удаление назначения
       /// </summary>
       /// <param name="id"></param>
       /// <param name="collection"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteGroupDocAppointment(long id, FormCollection collection)
        {
            try
            {
                _appointmentRepository.DeleteDocAppointment(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.Delete()", ex);
                return View(new DocAppointmentEditModel());
            }
        }

        #region JSon

        /// <summary>
        /// Меняет стутс формы 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeState(long id)
        {
            try
            {
                _appointmentRepository.ChangeDocAppointmentState(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.ChangeState()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Меняет стутс формы 
        /// </summary>
        /// <param name="personalDocTemplateId"></param>
        /// <returns></returns>
        public ActionResult DeletePersonalDocTemplate(int personalDocTemplateId)
        {
            try
            {
                _personalDocTemplateRepository.DeletePersonalDocTemplate(personalDocTemplateId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController.DeletePersonalDocTemplate()", ex);
                return new JsonResult { Data = false };
            }
        }

        #endregion

        #region GridActions

        /// <summary>
        /// Возвращает список  назначений
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _GroupDocAppointmentList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _appointmentRepository.GetAllGroupDocAppointments(sessData.UserGroupId, true).Where(
                    x => x.Status != (int) DocAppointmentStatus.Deleted).ToList()
                    .ConvertAll(GroupDocAppointmentListViewModel.DocAppointmentToModelConverter);
                return View(new GridModel<GroupDocAppointmentListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController._GroupDocAppointmentList()", ex);
                return View(new GridModel<GroupDocAppointmentListViewModel> { Data = new List<GroupDocAppointmentListViewModel>() });
            }
        }

        [GridAction]
        public ActionResult _PersonalDocTemplateList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _personalDocTemplateRepository.GetAllGroupPersonalDocTemplates(sessData.UserGroupId).ToList()
                    .ConvertAll(PersonalDocTemplateListViewModel.PersonalDocTemplateListViewModelToModelConverter);
                return View(new GridModel<PersonalDocTemplateListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController._PersonalDocTemplateList()", ex);
                return View(new GridModel<PersonalDocTemplateListViewModel> { Data = new List<PersonalDocTemplateListViewModel>() });
            }
        }
        #endregion
    }
}
