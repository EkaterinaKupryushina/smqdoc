using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class DocAppointmentController : Controller
    {
        private readonly IDocAppointmentRepository _appointmentRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;
        public DocAppointmentController(IDocAppointmentRepository appointmentRepository, IDocTemplateRepository docTemplateRepository)
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
            return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
        }

        /// <summary>
        /// Показывает страницу выбора формы для привязки
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectDocTemplate()
        {
            return View(_docTemplateRepository.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
        }

        /// <summary>
        /// Создание привязки к группе
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateGroupAppointment(long docTemplateId)
        {
            var newAppointment = new DocAppointment
                                     {DocTemplate = _docTemplateRepository.GetDocTemplateById(docTemplateId)};
            return View(new DocAppointmentEditModel(newAppointment));
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
            catch
            {
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
            return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
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
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Удаление Назначения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(long id)
        {
            return View(new DocAppointmentEditModel(_appointmentRepository.GetDocAppointmentById(id)));
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
            catch
            {
                return View();
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex.Message);
            }
            return Json(new { result = true });
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
            var sessData = SessionHelper.GetUserSessionData(Session);
            var data = _appointmentRepository.GetAllGroupDocAppointments(sessData.UserGroupId).Where(x => x.Status != (int)DocAppointmentStatus.Deleted).ToList()
                .ConvertAll(GroupDocAppointmentListViewModel.DocAppointmentToModelConverter);
            return View(new GridModel<GroupDocAppointmentListViewModel> { Data = data });
        }

        #endregion
    }
}
