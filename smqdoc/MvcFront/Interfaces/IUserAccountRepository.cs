using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Возвращает всех пользователей системы
        /// </summary>
        /// <param name="refreshFromDb">Обновить возвращаемое значенеи из базы</param>
        /// <returns></returns>
        IQueryable<UserAccount> GetAll(bool refreshFromDb = false);
        
        /// <summary>
        /// Возаращает пользователя по ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="refreshFromDb"></param>
        /// <returns></returns>
        UserAccount GetById(Int32 id, bool refreshFromDb = false);

        /// <summary>
        /// Возвращаетп ользователя по имени пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="refreshFromDb"></param>
        /// <returns></returns>
        UserAccount GetByLogin(string login, bool refreshFromDb = false);

        /// <summary>
        /// Попытка Логина
        /// </summary>
        /// <param name="userLogin">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        UserAccount Login(string userLogin, string password);
        
        /// <summary>
        /// Сохраняет пользователя в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Save(UserAccount entity);
        
        /// <summary>
        /// Удаляет пользователя из базы
        /// </summary>
        /// <param name="id"></param>
        void Delete(Int32 id);

        /// <summary>
        /// Меняет стутус пользователя с Active на unactive и наоборот
        /// </summary>
        /// <param name="id"></param>
        void ChangeState(Int32 id);

        /// <summary>
        /// Создает копию Instance объект ользователя
        /// </summary>
        /// <param name="uw"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        UserAccount Copy(IUnitOfWork uw, int userid);

                 
    }
}
