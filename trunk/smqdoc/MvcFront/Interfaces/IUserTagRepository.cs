using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserTagRepository
    {
        /// <summary>
        /// Возвращает тэг по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserTags GetUserTagById(long id);

        /// <summary>
        /// Возвращает список всех доступных тэгов
        /// </summary>
        /// <returns></returns>
        IQueryable<UserTags> GetAllUserTags();

        /// <summary>
        /// Сохраняет пользовательский тэг в базу
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool SaveUserTag(UserTags tag);

        /// <summary>
        /// Удаляет пользовательский тэг из базы
        /// </summary>
        /// <param name="id"></param>
        void DeleteUserTag(int id);

        /// <summary>
        /// Добавить пользователю тэг
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        bool AddUserTag(int userId, int tagId);

        //убрать у пользователя тэг
        bool RemoveUserTag(int userId, int tagId);
    }
}