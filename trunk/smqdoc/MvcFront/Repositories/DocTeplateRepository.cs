using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Infrastructure;

namespace MvcFront.Repositories
{
    public class DocTeplateRepository : IDocTeplateRepository
    {
        IUnitOfWork _unitOfWork;
        public DocTeplateRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<DocTemplate> GetAllDocTeplates()
        {
            return _unitOfWork.DbModel.DocTemplates.AsQueryable<DocTemplate>();
        }

        public DocTemplate GetDocTemplateById(long id)
        {
            if (id == 0)
                return new DocTemplate();
            else
                return _unitOfWork.DbModel.DocTemplates.SingleOrDefault(x => x.docteplateid == id);
        }

        public bool SaveDocTemplate(DocTemplate entity)
        {
            if (entity.docteplateid == 0)
            {
                entity.LastEditDate = DateTime.Now;
                _unitOfWork.DbModel.DocTemplates.AddObject(entity);
            }
            else
            {
                var oldEntity = this.GetDocTemplateById(entity.docteplateid);
                if (oldEntity == null)
                {
                    entity.docteplateid = 0;
                    this.SaveDocTemplate(entity);
                }
                oldEntity.Comment = entity.Comment;
                oldEntity.LastEditDate = DateTime.Now;
                oldEntity.Status = entity.Status;
                oldEntity.TemplateName = entity.TemplateName;
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void DeleteDocTemplate(long id)
        {
            var entity = this.GetDocTemplateById(id);
            if (entity != null)
            {
                entity.TemplateStatus = DocTemplateStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void ChangeDocTemplateState(long id)
        {
            var entity = this.GetDocTemplateById(id);
            if (entity != null)
            {
                if (entity.TemplateStatus != DocTemplateStatus.Active)
                {
                    entity.TemplateStatus = DocTemplateStatus.Active;
                }
                else
                {
                    entity.TemplateStatus = DocTemplateStatus.Unactive;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public FieldTeplate GetFieldTemplateById(long id)
        {
            if(id == 0)
            {
                return new FieldTeplate();
            }
            else
            {
                return _unitOfWork.DbModel.FieldTeplates.SingleOrDefault(x => x.fieldteplateid == id);
            }
        }

        public bool SaveFieldTemplate(FieldTeplate entity)
        {
            if (entity.fieldteplateid == 0)
            {
                _unitOfWork.DbModel.FieldTeplates.AddObject(entity);
            }
            else
            {
                var oldEntity = this.GetFieldTemplateById(entity.fieldteplateid);
                if (oldEntity == null)
                {
                    entity.fieldteplateid = 0;
                    this.SaveFieldTemplate(entity);
                }
                oldEntity.DocTemplate_docteplateid = entity.DocTemplate_docteplateid;
                oldEntity.FieldName = entity.FieldName;
                oldEntity.FiledType = entity.FiledType;
                oldEntity.MaxVal = entity.MaxVal;
                oldEntity.MinVal = entity.MinVal;
                oldEntity.OrderNumber = entity.OrderNumber;
                oldEntity.Restricted = entity.Restricted;
                oldEntity.Status = entity.Status;               
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void DeleteFieldTemplate(long TemplateFieldId)
        {
            var entity = this.GetFieldTemplateById(TemplateFieldId);
            if (entity != null)
            {
                entity.TemplateStatus = FieldTemplateStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}