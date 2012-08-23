using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IPersonalDocTemplateRepository
    {

        /// <summary>
        /// Возвращает список назначений доступных всем пользвателеям группы
        /// </summary>
        /// <returns></returns>
        IQueryable<PersonalDocTemplate> GetAllGroupPersonalDocTemplates(int groupId);

        /// <summary>
        /// Возвращает объет назначение по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PersonalDocTemplate GetPersonalDocTemplateById(int id);

        /// <summary>
        /// Сохраняем объект назначение в базу
        /// </summary>
        /// <param name="entity"></param>
        void SavePersonalDocTemplate(PersonalDocTemplate entity);

        /// <summary>
        /// Сохраняем объект назначение в базу
        /// </summary>
        /// <param name="id"></param>
        void DeletePersonalDocTemplate(int id);

    }
}