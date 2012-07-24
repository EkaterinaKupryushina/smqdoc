using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IDocTemplateRepository
    {
        /// <summary>
        /// Получает список всех шаблонов
        /// </summary>
        /// <returns></returns>
        IQueryable<DocTemplate> GetAllDocTeplates();

        /// <summary>
        /// Поулчает шаблон по его Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DocTemplate GetDocTemplateById(Int64 id);

        /// <summary>
        /// Сохраняет шаблон в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveDocTemplate(DocTemplate entity);
        
        /// <summary>
        /// Ставит шаблону сттус удален
        /// </summary>
        /// <param name="id"></param>
        void DeleteDocTemplate(Int64 id);

        /// <summary>
        /// Меняет стутус Формы с Active на unactive
        /// </summary>
        /// <param name="id"></param>
        void ChangeDocTemplateState(Int64 id);

        /// <summary>
        /// Получает список всех 
        /// </summary>
        /// <returns></returns>
        IQueryable<ComputableFieldTemplateParts> GetAllComputableFieldTempalteParts();

        /// <summary>
        /// Поулчить шаблон поля по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FieldTemplate GetFieldTemplateById(Int64 id);

        /// <summary>
        /// сохранить шаблон поля в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveFieldTemplate(FieldTemplate entity);

        /// <summary>
        /// Удалить поставить шаблону поля статус удален
        /// </summary>
        /// <param name="templateFieldId"></param>
        void DeleteFieldTemplate(Int64 templateFieldId);

        /// <summary>
        /// Изменить номер Формы поля 
        /// </summary>
        /// <param name="templateFieldId"></param>
        /// <param name="newNumber"></param>
        void SetFieldTemplateNumber(Int64 templateFieldId, int newNumber);
    }
}
