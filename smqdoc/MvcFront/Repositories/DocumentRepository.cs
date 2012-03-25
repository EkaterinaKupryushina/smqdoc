using System;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupTemplateRepository _groupTemplateRepository;

        public DocumentRepository(IUnitOfWork unitOfWork,IGroupTemplateRepository groupTemplateRepository)
        {
            _unitOfWork = unitOfWork;
            _groupTemplateRepository = groupTemplateRepository;
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
                var oldEntity = GetDocumentById(entity.documentid);
                if (oldEntity == null)
                {
                    entity.documentid = 0;
                    SaveDocument(entity);
                }
                if (oldEntity != null)
                {                    
                    oldEntity.LastComment = entity.LastComment;
                    oldEntity.LastEditDate = DateTime.Now;
                    entity = oldEntity;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return entity;
        }
        public Document ChangeDocumentStatus(long id, DocumentStatus state)
        {
            var doc = GetDocumentById(id);
            doc.DocStatus = state;
            _unitOfWork.DbModel.SaveChanges();
            return doc;
        }

        public Document DeleteDocument(long id)
        {
            var doc = GetDocumentById(id);
            doc.DocStatus = DocumentStatus.Deleted;
            _unitOfWork.DbModel.SaveChanges();
            return doc;
        }
        public Document CreateDocumentFromGroupDocument(long groupTemplId,int userId)
        {
            var groupTempl = _groupTemplateRepository.GetGroupTemplateById(groupTemplId);
            if(groupTempl != null)
            {
                var doc = new Document();                
                doc.UserAccount_userid = userId;
                doc.GroupTemplate_grouptemplateid = groupTemplId;
                doc = SaveDocument(doc);
                foreach (var fieldTempl in groupTempl.DocTemplate.FieldTeplates)
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