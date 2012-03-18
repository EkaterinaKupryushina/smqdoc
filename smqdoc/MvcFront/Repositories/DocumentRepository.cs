using System;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    oldEntity.DocumentName = entity.DocumentName;
                    oldEntity.LastComment = entity.LastComment;
                    oldEntity.LastEditDate = DateTime.Now;
                    entity = oldEntity;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return entity;
        }
        public Document ChangeStatus(long id, DocumentStatus state)
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
    }
}