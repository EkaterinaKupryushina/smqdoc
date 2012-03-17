﻿using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserAccountRepository
    {
        IQueryable<UserAccount> GetAll();
        UserAccount GetById(Int32 id);
        UserAccount GetByLogin(string login);
        UserAccount GetByEmail(string email);
        UserAccount Login(string userLogin, string password);
        bool Save(UserAccount entity);
        void Delete(Int32 id);
        void ChangeState(Int32 id);
        UserAccount Copy(IUnitOfWork uw, int userid);
    }
}