using System;
using System.Collections.Generic;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Interfaces
{
    /// <summary>
    /// Интерфейс относистя только к Групповым документам
    /// </summary>
    public interface IGroupTemplateRepository
    {
        /// <summary>
        /// Возвращает список всех групповых  назначений документов
        /// </summary>
        /// <returns></returns>
        IQueryable<GroupTemplate> GetAllGroupTemplates();

        /// <summary>
        /// Возвращает список групповых  назначений по id группы (групповых  назначений принадлежат группе)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<GroupTemplate> GetGroupTemplateByGroupId(Int64 id);

        /// <summary>
        /// Возвращает групповое  назначение по ID шаблона
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GroupTemplate GetGroupTemplateById(Int64 id);

        /// <summary>
        /// Сохраняет групповое  назначение в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveGroupTemplate(GroupTemplate entity);

        /// <summary>
        /// Удаляет групповое назначение из базы
        /// </summary>
        /// <param name="id"></param>
        void DeleteGroupTemplate(Int64 id);
        
        /// <summary>
        /// Меняет статус группового  назначения на новый статус
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void ChangeGroupTemplateState(Int64 id, GroupTemplateStatus status);
    }
}
