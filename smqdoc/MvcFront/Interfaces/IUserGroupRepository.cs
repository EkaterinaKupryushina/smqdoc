using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserGroupRepository
    {
        /// <summary>
        /// Получить список всех групп
        /// </summary>
        /// <returns></returns>
        IQueryable<UserGroup> GetAll();
        /// <summary>
        /// Получить группу по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserGroup GetById(Int32 id);
        
        /// <summary>
        /// Получить группу по имени группы
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        UserGroup GetByGroupName(string groupName);
        
        /// <summary>
        /// Сохранить обект в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Save(UserGroup entity);
        
        /// <summary>
        /// Удалить группу
        /// </summary>
        /// <param name="id"></param>
        void Delete(Int32 id);
        
        /// <summary>
        /// Включает или отключает группу (Status == Active To Unactive)
        /// </summary>
        /// <param name="id"></param>
        void ChangeState(Int32 id);

        /// <summary>
        /// Копируем группу (все ее параметры, и возвращаем новую сущность
        /// </summary>
        /// <param name="uw"></param>
        /// <param name="usergroupid"></param>
        /// <returns></returns>
        UserGroup Copy(IUnitOfWork uw, int usergroupid);

        /// <summary>
        /// Удалить пользователя из группы
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool RemoveMember(int groupId,int userId);

        /// <summary>
        /// Добавить пользователя как члена группы
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool AddMember(int groupId, int userId);
    }
}
