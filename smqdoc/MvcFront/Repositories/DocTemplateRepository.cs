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
            entity.LastEditDate = DateTime.Now;
            if (entity.docteplateid == 0)
            {
                
                _unitOfWork.DbModel.DocTemplates.AddObject(entity);
            }
            else
            {
                _unitOfWork.DbModel.DocTemplates.ApplyCurrentValues(entity);
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
            if (entity.TemplateType != FieldTemplateType.Number)
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
                    entity.OrderNumber = docTempl.FieldTeplates.Count + 1;
                    _unitOfWork.DbModel.FieldTemplates.AddObject(entity);
                }
            }
            else
            {
                if(entity.TemplateType != FieldTemplateType.Calculated && entity.ComputableFieldTemplateParts != null)
                {
                    while (entity.ComputableFieldTemplateParts.Any())
                    {
                        _unitOfWork.DbModel.ComputableFieldTemplateParts.DeleteObject(entity.ComputableFieldTemplateParts.ElementAt(0));
                    }
                }
                _unitOfWork.DbModel.FieldTemplates.ApplyCurrentValues(entity);
            }            
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void DeleteFieldTemplate(long templateFieldId)
        {
            var entity = GetFieldTemplateById(templateFieldId);
            
            if (entity != null)
            {
                if (entity.PlanFieldTemplates != null)
                {
                    for (var i = 0; i < entity.PlanFieldTemplates.Count; i++ )
                    {
                        _unitOfWork.DbModel.FieldTemplates.DeleteObject(entity.PlanFieldTemplates.ElementAt(i));
                    }
                }
                if (entity.ComputableFieldTemplateParts != null)
                {
                    while (entity.ComputableFieldTemplateParts.Any())
                    {
                        _unitOfWork.DbModel.ComputableFieldTemplateParts.DeleteObject(entity.ComputableFieldTemplateParts.ElementAt(0));
                    }
                }
            }
            _unitOfWork.DbModel.FieldTemplates.DeleteObject(entity);
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

                var enSecond = enFirst.DocTemplate.FieldTeplates.FirstOrDefault(x =>  x.OrderNumber == newNumber);
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
                var items = docTempl.FieldTeplates.OrderBy(x => x.OrderNumber);
                for (int i = 0; i < items.Count();i++ )
                {
                    items.ElementAt(i).OrderNumber = i+1;
                }
                _unitOfWork.DbModel.SaveChanges();
            }
        }

    }
}