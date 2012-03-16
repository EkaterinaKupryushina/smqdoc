using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IDocTemplateRepository
    {
        IQueryable<DocTemplate> GetAllDocTeplates();
        DocTemplate GetDocTemplateById(Int64 id);
        bool SaveDocTemplate(DocTemplate entity);
        void DeleteDocTemplate(Int64 id);
        void ChangeDocTemplateState(Int64 id);

        
        FieldTemplate GetFieldTemplateById(Int64 id);
        bool SaveFieldTemplate(FieldTemplate entity);
        void DeleteFieldTemplate(Int64 TemplateFieldId);
        void SetFieldTemplateNumber(Int64 TemplateFieldId, int newNumber);
    }
}
