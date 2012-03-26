using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;

namespace MvcFront.Controllers
{
    public class DocumentAppointmentController : Controller
    {
         private readonly IGroupTemplateRepository _groupTemplateRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocumentAppointmentController(IGroupTemplateRepository groupTemplateRepository, IDocTemplateRepository docTemplateRepository, IDocumentRepository documentRepository)
         {
            _groupTemplateRepository = groupTemplateRepository;
            _docTemplateRepository = docTemplateRepository;
            _documentRepository = documentRepository;
         }
        //
        // GET: /DocumentAppointment/

        public ActionResult Index()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            return View(_groupTemplateRepository.GetGroupTemplateByGroupId(sessData.UserGroupId).Where(x => x.Status != (int)GroupTemplateStatus.Deleted && x.DocTemplate.Status != (int)DocTemplateStatus.Deleted)
                .ToList().ConvertAll(DocumentAppointmentListViewModel.GroupTemplateToModelConverter).ToList());
        }

        //
        // GET: /DocumentAppointment/Details/5

        public ActionResult Details(int id)
        {
            return View(_groupTemplateRepository.GetGroupTemplateById(id).Documents.Where(x => x.Status != (int)DocumentStatus.Deleted).ToList()
                .ConvertAll(UserDocumentListViewModel.DocumentToModelConverter));
        }


        //
        // POST: /DocumentAppointment/Create

        public ActionResult Create(long id = 0)
        {
            if(id == 0)
                return View(_docTemplateRepository.GetAllDocTeplates().Where(x => x.Status == (int)DocTemplateStatus.Active).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter));
            try
            {
                var sesData = SessionHelper.GetUserSessionData(Session);
               var templ = _docTemplateRepository.GetDocTemplateById(id);
                var groupTempl = new GroupTemplate
                                     {
                                         DocTemplate_docteplateid = templ.docteplateid,
                                         Name = templ.TemplateName,
                                         Status = (int) GroupTemplateStatus.Unactive,
                                         DateStart = DateTime.Now,
                                         DateEnd = DateTime.Now + new TimeSpan(7, 0, 0, 0),
                                         UserGroup_usergroupid = sesData.UserGroupId
                                     };
                _groupTemplateRepository.SaveGroupTemplate(groupTempl);

                return RedirectToAction("Edit",new {id = groupTempl.grouptemplateid});
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        //
        // GET: /DocumentAppointment/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(new GroupTemplateEditViewModel(_groupTemplateRepository.GetGroupTemplateById(id)));
        }

        //
        // POST: /DocumentAppointment/Edit/5

        [HttpPost]
        public ActionResult Edit(long id, GroupTemplateEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var groupTemplate = _groupTemplateRepository.GetGroupTemplateById(id);
                    groupTemplate = model.Update(groupTemplate);
                    if (!_groupTemplateRepository.SaveGroupTemplate(groupTemplate))
                    {
                        throw new Exception("Ошибка сохранения связи");
                    }
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

        //
        // GET: /DocumentAppointment/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(_groupTemplateRepository.GetGroupTemplateById(id));
        }

        //
        // POST: /DocumentAppointment/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _groupTemplateRepository.DeleteGroupTemplate(id);
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult ChangeState(int id)
        {
            return View(_groupTemplateRepository.GetGroupTemplateById(id));
        }



        [HttpPost]
        public ActionResult ChangeState(int id, FormCollection collection)
        {
            try
            {
                var grTempl = _groupTemplateRepository.GetGroupTemplateById(id);
                var newStatus = GroupTemplateStatus.Active;
                if(grTempl.Status == (int)GroupTemplateStatus.Active)
                    newStatus = GroupTemplateStatus.Unactive;
         
                _groupTemplateRepository.ChangeGroupTemplateState(id,newStatus);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DocumentDetails(long id)
        {
            return View(_documentRepository.GetDocumentById(id));
        }
        [HttpPost]
        public ActionResult DocumentDetails(long id, FormCollection collection)
        {
            var doc = _documentRepository.GetDocumentById(id);
            if(collection.AllKeys.Contains("LastComment"))
            doc.LastComment = collection.GetValue("LastComment").AttemptedValue;
            doc = _documentRepository.SaveDocument(doc);
            return View(doc);
        }


        public ActionResult Submit(long id)
        {            
            _documentRepository.ChangeDocumentStatus(id, DocumentStatus.Submited);
            return RedirectToAction("DocumentDetails", new { id = id });
        }

        public ActionResult Dismiss(long id)
        {
            _documentRepository.ChangeDocumentStatus(id, DocumentStatus.Editing);
            return RedirectToAction("DocumentDetails", new { id = id });
        }

        public ActionResult GroupDocumentsList()
        {
            var sesData =  SessionHelper.GetUserSessionData(Session);
            return View(_documentRepository.GetAll().Where(x => x.GroupTemplate.UserGroup_usergroupid == sesData.UserGroupId)
                        .ToList().ConvertAll(GroupDocumentListViewModel.DocumentToModelConverter));
        }
    }
}
