using System;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocAppointmentRepository _appointmentRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public DocumentRepository(IUnitOfWork unitOfWork, IDocAppointmentRepository appointmentRepository, IUserGroupRepository userGroupRepository)
        {
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
            _userGroupRepository = userGroupRepository;
        }

        public IQueryable<Document> GetAll()
        {
            return _unitOfWork.DbModel.Documents.AsQueryable();
        }

        public Document GetDocumentById(long id)
        {
            if(id == 0)
            {
                return  new Document();
            }
            return _unitOfWork.DbModel.Documents.SingleOrDefault(x=>x.documentid == id);
        }

        public IQueryable<Document> GetPersonalDocumentsByUserId(long id, DocumentStatus? status = null)
        {
            return status.HasValue
                ? _unitOfWork.DbModel.Documents.Where(x => x.UserAccount_userid == id && x.DocAppointment.UserGroup_usergroupid == null && x.Status == (int)status && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted)
                : _unitOfWork.DbModel.Documents.Where(x => x.UserAccount_userid == id && x.DocAppointment.UserGroup_usergroupid == null && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted);
         
        }

        public IQueryable<Document> GetUserDocumentsByUserId(long id, DocumentStatus? status = null)
        {
            return status.HasValue
               ? _unitOfWork.DbModel.Documents.Where(x => x.UserAccount_userid == id && x.Status == (int)status && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted)
               : _unitOfWork.DbModel.Documents.Where(x => x.UserAccount_userid == id && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted);
           }

        public IQueryable<Document> GetGroupDocumentsByGroupId(long id, DocumentStatus? status = null)
        {
            return status.HasValue
                ? _unitOfWork.DbModel.Documents.Where(x => x.DocAppointment.UserGroup_usergroupid == id &&  x.Status == (int)status && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted)
                : _unitOfWork.DbModel.Documents.Where(x => x.DocAppointment.UserGroup_usergroupid == id &&  x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted);
        }

        public IQueryable<Document> GetPersonalDocumentsByGroupId(int id, DocumentStatus? status = null)
        {
            var query = status.HasValue
                ? _unitOfWork.DbModel.Documents.Where(x => x.DocAppointment.UserGroup_usergroupid == null && x.Status == (int)status && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted)
                : _unitOfWork.DbModel.Documents.Where(x => x.DocAppointment.UserGroup_usergroupid == null && x.DocAppointment.Status != (int)DocAppointmentStatus.Deleted && x.DocAppointment.DocTemplate.Status != (int)DocTemplateStatus.Deleted);

            var groupUserIds = _userGroupRepository.GetById(id).Members.Select(x => x.userid);

            return query.Where(x => groupUserIds.Contains(x.UserAccount_userid));
        }

        public Document SaveDocument(Document entity)
        {
            if (entity.documentid == 0)
            {
                entity.CreationDate = DateTime.Now;
                entity.LastEditDate = DateTime.Now;
                _unitOfWork.DbModel.Documents.AddObject(entity);
            }
            else
            {
                entity.LastEditDate = DateTime.Now;
                _unitOfWork.DbModel.Documents.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
            return entity;
        }
        public void ChangeDocumentStatus(long id, DocumentStatus state)
        {
            var doc = GetDocumentById(id);
            doc.DocStatus = state;
            _unitOfWork.DbModel.SaveChanges();
        }

        public Document DeleteDocument(long id)
        {
            var doc = GetDocumentById(id);
            doc.DocStatus = DocumentStatus.Deleted;
            _unitOfWork.DbModel.SaveChanges();
            return doc;
        }

        public Document CreateDocumentFromDocAppointment(long docAppointmentId, int userId)
        {
            var docAppointment = _appointmentRepository.GetDocAppointmentById(docAppointmentId);
            if (docAppointment != null)
            {
                var doc = new Document
                              {
                                  UserAccount_userid = userId, 
                                  DocAppointment_docappointmentid = docAppointmentId,
                                  Status = docAppointment.DocTemplate.IsPlanneble 
                                        ? (int)DocumentStatus.PlanEditing
                                        : (int)DocumentStatus.FactEditing
                              };
                doc = SaveDocument(doc);
                foreach (var fieldTempl in docAppointment.DocTemplate.FieldTeplates)
                {
                    var fld = new DocField
                                  {
                                      Document = doc,
                                      FieldTemplate_fieldteplateid = fieldTempl.fieldteplateid
                                  };
                    _unitOfWork.DbModel.DocFields.AddObject(fld);
                }
                _unitOfWork.DbModel.SaveChanges();
                return doc;
            }
            return null;
        }
    }
}