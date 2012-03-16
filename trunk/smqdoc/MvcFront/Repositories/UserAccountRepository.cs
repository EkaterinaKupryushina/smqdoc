using System;
using System.Linq;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<UserAccount> GetAll()
        {
            return _unitOfWork.DbModel.UserAccounts.AsQueryable();
        }

        public UserAccount GetById(Int32 id)
        {
            if (id == 0)
                return new UserAccount();
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

        public UserAccount Login(string userLogin, string password)
        {
            var user = GetByLogin(userLogin);
            if (user == null) return null;
            if (user.Password.Trim() == password.Trim() && user.UserStatus == UserAccountStatus.Active)
            {
                user.LastAccess = DateTime.Now;
                _unitOfWork.DbModel.SaveChanges();
                return user;
            }
            return null;
        }

        public bool Save(UserAccount entity)
        {
            if (entity.userid == 0)
            {
                if (GetByLogin(entity.Login) == null && GetByLogin(entity.Email) == null)
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
                item.UserStatus = item.UserStatus == UserAccountStatus.Active ? UserAccountStatus.Unactive : UserAccountStatus.Active;
                _unitOfWork.DbModel.SaveChanges();
            }
        }
        public UserAccount Copy(IUnitOfWork uw, int userid)
        {
            if (userid == 0)
                return new UserAccount();
            return uw.DbModel.UserAccounts.SingleOrDefault(x => x.userid == userid);
        }
    }
}