using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class UserDocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocAppointmentRepository _appointmentRepository;
        public UserDocumentController(IDocAppointmentRepository appointmentRepository, IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
            _appointmentRepository = appointmentRepository;
        }

        //
        // GET: /Document/

        public ActionResult Index()
        {
            return View();
        }
        [GridAction]
        public ActionResult _UserDocumentsList()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            var data = _documentRepository.GetAll()
                .Where(x => x.Status != (int)DocumentStatus.Deleted && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted 
                    && x.UserAccount_userid == sessData.UserId && x.DocAppointment.UserGroup_usergroupid == sessData.UserGroupId)
                    .ToList().ConvertAll(DocumentListViewModel.DocumentToModelConverter).ToList();

            return View(new GridModel<DocumentListViewModel> { Data = data });
        }
        
        [GridAction]
        public ActionResult _UserGroupTeplatesList()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            var allUserGroupDocs =
                _documentRepository.GetAll().Where(x => x.UserAccount_userid == sessData.UserId && x.Status != (int)DocumentStatus.Deleted 
                    && x.DocAppointment.UserGroup_usergroupid == sessData.UserGroupId)
                    .Select(x => x.DocAppointment_docappointmentid).ToList();

            var data = _appointmentRepository.GetAllGroupDocAppointments(sessData.UserGroupId)
                .Where(x => (x.PlanedStartDate <= DateTime.Now || x.ActualStartDate <= DateTime.Now)
                && x.DocTemplate.Status == (int)DocTemplateStatus.Active && x.Status == (int)DocAppointmentStatus.Active 
                && !allUserGroupDocs.Contains(x.docappointmentid))
                .ToList().ConvertAll(DocAppointmentListViewModel.DocAppointmentToModelConverter).ToList();
            return View(new GridModel<DocAppointmentListViewModel> { Data = data });
        }
        //
        // GET: /Document/Details/5

        public ActionResult Details(long id)
        {
            return View(_documentRepository.GetDocumentById(id));
        }

        //
        // GET: /Document/Create

        public ActionResult Create(long id)
        {
            try
            {

            var sessData = SessionHelper.GetUserSessionData(Session);
            var doc = _documentRepository.CreateDocumentFromGroupDocument(id,sessData.UserId);
            if (doc != null)
                return RedirectToAction("Edit", new {id = doc.documentid});
          }catch{}
            return RedirectToAction("Index");
                 
        } 

 
        public ActionResult Edit(int id)
        {
            return View(new DocumentEditModel(_documentRepository.GetDocumentById(id)));
        }

        //
        // POST: /Document/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentEditModel model)
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
                    _documentRepository.ChangeDocumentStatus(doc.documentid, doc.DocStatus == DocumentStatus.PlanEditing ? DocumentStatus.PlanSended : DocumentStatus.FactSended);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
