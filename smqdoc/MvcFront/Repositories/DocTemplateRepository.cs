using System;
using System.Linq;
using MvcFront.Enums;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class DocTemplateRepository : IDocTemplateRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public DocTemplateRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<DocTemplate> GetAllDocTeplates()
        {
            return _unitOfWork.DbModel.DocTemplates.AsQueryable();
        }

        public DocTemplate GetDocTemplateById(long id)
        {
            if (id == 0)
                return new DocTemplate();
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
                var oldEntity = GetDocTemplateById(entity.docteplateid);
                if (oldEntity == null)
                {
                    entity.docteplateid = 0;
                    SaveDocTemplate(entity);
                }
                if (oldEntity != null)
                {
                    oldEntity.Comment = entity.Comment;
                    oldEntity.LastEditDate = DateTime.Now;
                    oldEntity.Status = entity.Status;
                    oldEntity.TemplateName = entity.TemplateName;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }
        
        public void DeleteDocTemplate(long id)
        {
            var entity = GetDocTemplateById(id);
            if (entity != null)
            {
                entity.TemplateStatus = DocTemplateStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void ChangeDocTemplateState(long id)
        {
            var entity = GetDocTemplateById(id);
            if (entity != null)
            {
                entity.TemplateStatus = entity.TemplateStatus != DocTemplateStatus.Active ? DocTemplateStatus.Active : DocTemplateStatus.Unactive;
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public FieldTemplate GetFieldTemplateById(long id)
        {
            if(id == 0)
            {
                return new FieldTemplate();
            }
            return _unitOfWork.DbModel.FieldTemplates.SingleOrDefault(x => x.fieldteplateid == id);
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
                var docTempl = GetDocTemplateById(entity.DocTemplate_docteplateid);
                if (docTempl != null)
                {
                    entity.OrderNumber = docTempl.FieldTeplates.Count(x => x.Status != (int)FieldTemplateStatus.Deleted) + 1;
                    _unitOfWork.DbModel.FieldTemplates.AddObject(entity);
                }
            }
            else
            {
                var oldEntity = GetFieldTemplateById(entity.fieldteplateid);
                if (oldEntity == null)
                {
                    entity.fieldteplateid = 0;
                    SaveFieldTemplate(entity);
                }
                if (oldEntity != null)
                {
                    oldEntity.DocTemplate_docteplateid = entity.DocTemplate_docteplateid;
                    oldEntity.FieldName = entity.FieldName;
                    oldEntity.FiledType = entity.FiledType;
                    oldEntity.MaxVal = entity.MaxVal;
                    oldEntity.MinVal = entity.MinVal;
                    oldEntity.OrderNumber = entity.OrderNumber;
                    oldEntity.Restricted = entity.Restricted;
                    oldEntity.Status = entity.Status;
                }
            }            
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        private void SaveCalculatedFieldTemplateParts(FieldTemplate entity)
        {
            var lstOld = _unitOfWork.DbModel.ComputableFieldTemplateParts.Where(x => x.FieldTemplate.fieldteplateid == entity.fieldteplateid).ToList();
            foreach (ComputableFieldTemplateParts oldItem in lstOld)
            {
                _unitOfWork.DbModel.ComputableFieldTemplateParts.DeleteObject(oldItem);
            }

            foreach (ComputableFieldTemplateParts newItem in entity.ComputableFieldTemplateParts)
            {
                _unitOfWork.DbModel.ComputableFieldTemplateParts.AddObject(newItem);
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        //public IQueryable<DocTemplate> GetAllDocTeplates()
        //{
        //    return _unitOfWork.DbModel.DocTemplates.AsQueryable();
        //}
        public void DeleteFieldTemplate(long templateFieldId)
        {
            var entity = GetFieldTemplateById(templateFieldId);
            if (entity != null)
            {
                entity.TemplateStatus = FieldTemplateStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();
            if (entity != null) ReoderFields(entity.DocTemplate_docteplateid);
        }

        public IQueryable<ComputableFieldTemplateParts> GetAllComputableFieldTempalteParts()
        {
            return _unitOfWork.DbModel.ComputableFieldTemplateParts.AsQueryable();
        }

        public void SetFieldTemplateNumber(long templateFieldId, int newNumber)
        {
            var enFirst = GetFieldTemplateById(templateFieldId);
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
            var docTempl = GetDocTemplateById(docTemplId);
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