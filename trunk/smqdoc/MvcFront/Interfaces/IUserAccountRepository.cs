using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcFront;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserAccountRepository
    {
        IQueryable<UserAccount> GetAll();
        UserAccount GetById(Int32 id);
        UserAccount GetByLogin(string login);
        UserAccount GetByEmail(string email);
        bool Login(string userLogin, string password);
        bool Save(UserAccount entity);
        void Delete(Int32 id);
        void ChangeState(Int32 id);
    }
}
