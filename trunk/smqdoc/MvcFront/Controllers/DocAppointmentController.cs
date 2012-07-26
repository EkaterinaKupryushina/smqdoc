using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;

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
            var sessData = SessionHelper.GetUserSessionData(Session);
            return View(_appointmentRepository.GetAllGroupDocAppointments(sessData.UserGroupId).Where(x => x.Status != (int)DocAppointmentStatus.Deleted).ToList().ConvertAll(GroupDocAppointmentListViewModel.DocAppointmentToModelConverter));
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

        //
        // POST: /DocAppintment/Delete/5

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
    }
}
