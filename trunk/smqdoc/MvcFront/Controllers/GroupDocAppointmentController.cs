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
        public GroupDocAppointmentController(IDocAppointmentRepository appointmentRepository, IDocTemplateRepository docTemplateRepository)
        {
            _appointmentRepository = appointmentRepository;
            _docTemplateRepository = docTemplateRepository;
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
        public ActionResult Details(long id)
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
                return
                    View(
                        _docTemplateRepository.GetAllDocTeplates().Where(
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
        public ActionResult Edit(long id)
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
        public ActionResult Edit(DocAppointmentEditModel model)
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
        public ActionResult Delete(long id)
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
        public ActionResult Delete(long id, FormCollection collection)
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "GroupDocAppointmentController._DocReportList()", ex);
                return View(new GridModel<GroupDocAppointmentListViewModel> { Data = new List<GroupDocAppointmentListViewModel>() });
            }
        }

        #endregion
    }
}
