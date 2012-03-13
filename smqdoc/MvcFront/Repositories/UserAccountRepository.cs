using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork = null;
        public UserAccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<UserAccount> GetAll()
        {
            return _unitOfWork.DbModel.UserAccounts.AsQueryable<UserAccount>();
        }

        public UserAccount GetById(Int32 id)
        {
            if (id == 0)
                return new UserAccount();
            else
                return _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
        }

        public UserAccount GetByLogin(string login)
        {
            return _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.Login == login.Trim());
        }

        public UserAccount GetByEmail(string email)
        {
            return _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.Email == email.Trim());
        }

        public bool Login(string userLogin, string Password)
        {
            var user = this.GetByLogin(userLogin);
            if (user == null) return false;
            if (user.Password.Trim() == Password.Trim() && user.UserStatus == UserAccountStatus.Active)
            {
                user.LastAccess = DateTime.Now;
                _unitOfWork.DbModel.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Save(UserAccount entity)
        {
            if (entity.userid == 0)
            {
                if (this.GetByLogin(entity.Login) == null && this.GetByLogin(entity.Email) == null)
                    _unitOfWork.DbModel.UserAccounts.AddObject(entity);
                else
                    return false;
            }
            else
            {
                var item = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == entity.userid);
                if (item != null)
                {
                    item.IsAdmin = entity.IsAdmin;
                    item.Email = entity.Email;
                    item.FirstName = entity.FirstName;
                    item.LastAccess = entity.LastAccess;
                    item.LastName = entity.LastName;
                    item.Login = entity.Login;
                    item.Password = entity.Password;
                    item.SecondName = entity.SecondName;
                    item.Status = entity.Status;
                    item.userid = entity.userid;

                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void Delete(Int32 id)
        {
            var item = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
            if (item != null)
            {
                item.UserStatus = UserAccountStatus.Deleted;
                _unitOfWork.DbModel.SaveChanges();
            }
        }
        public void ChangeState(Int32 id)
        {
            var item = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
            if (item != null)
            {
                if(item.UserStatus == UserAccountStatus.Active)
                    item.UserStatus = UserAccountStatus.Unactive;
                else
                    item.UserStatus = UserAccountStatus.Active;
                _unitOfWork.DbModel.SaveChanges();
            }
        }
    }
}