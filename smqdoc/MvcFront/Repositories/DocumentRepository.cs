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

        public DocumentRepository(IUnitOfWork unitOfWork, IDocAppointmentRepository appointmentRepository)
        {
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
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

        public Document CreateDocumentFromGroupDocument(long docAppointmentId,int userId)
        {
            var docAppointment = _appointmentRepository.GetDocAppointmentById(docAppointmentId);
            if (docAppointment != null)
            {
                var doc = new Document();                
                doc.UserAccount_userid = userId;
                doc.DocAppointment_docappointmentid = docAppointmentId;
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