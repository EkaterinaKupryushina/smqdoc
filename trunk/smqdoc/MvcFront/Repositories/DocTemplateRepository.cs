using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Infrastructure;

namespace MvcFront.Repositories
{
    public class DocTemplateRepository : IDocTemplateRepository
    {
        IUnitOfWork _unitOfWork;
        public DocTemplateRepository(IUnitOfWork unitOfWork)
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

        public FieldTemplate GetFieldTemplateById(long id)
        {
            if(id == 0)
            {
                return new FieldTemplate();
            }
            else
            {
                return _unitOfWork.DbModel.FieldTemplates.SingleOrDefault(x => x.fieldteplateid == id);
            }
        }

        public bool SaveFieldTemplate(FieldTemplate entity)
        {
            if (entity.TemplateType != FieldTemplateType.NUMBER)
            {
                entity.Restricted = null;
                entity.MaxVal = null;
                entity.MinVal = null;
            }
            if (entity.fieldteplateid == 0)
            {
                var docTempl = this.GetDocTemplateById(entity.DocTemplate_docteplateid);
                if (docTempl != null)
                {
                    entity.OrderNumber = docTempl.FieldTeplates.Where(x=>x.Status != (int)FieldTemplateStatus.Deleted).Count() + 1;
                    _unitOfWork.DbModel.FieldTemplates.AddObject(entity);
                }
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
            ReoderFields(entity.DocTemplate_docteplateid);
        }
        public void SetFieldTemplateNumber(long TemplateFieldId, int newNumber)
        {
            var enFirst = this.GetFieldTemplateById(TemplateFieldId);
            if (enFirst != null)
            {
                int oldNumber = enFirst.OrderNumber;

                var enSecond = enFirst.DocTemplate.FieldTeplates.FirstOrDefault(x => x.Status != (int)FieldTemplateStatus.Deleted && x.OrderNumber == newNumber);
                if (enSecond == null)
                {
                    enFirst.OrderNumber = newNumber;
                }
                else
                {
                    enSecond.OrderNumber = oldNumber;
                    enFirst.OrderNumber = newNumber;
                }
                _unitOfWork.DbModel.SaveChanges();
                ReoderFields(enFirst.DocTemplate_docteplateid);
            }
            
        }
        private void ReoderFields(long docTemplId)
        {
            var docTempl = this.GetDocTemplateById(docTemplId);
            if (docTempl != null)
            {
                var items = docTempl.FieldTeplates.Where(x => x.Status != (int)FieldTemplateStatus.Deleted).OrderBy(x => x.OrderNumber);
                for (int i = 0; i < items.Count();i++ )
                {
                    items.ElementAt(i).OrderNumber = i+1;
                }
                docTempl.FieldTeplates.Where(x => x.Status == (int)FieldTemplateStatus.Deleted).ToList().ForEach(x => x.OrderNumber = int.MaxValue);

                _unitOfWork.DbModel.SaveChanges();
            }
        }

    }
}