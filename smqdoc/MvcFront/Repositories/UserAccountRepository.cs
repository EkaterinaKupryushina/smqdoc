using System;
using System.Data.Objects;
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
            _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, _unitOfWork.DbModel.UserAccounts.AsQueryable());
            return _unitOfWork.DbModel.UserAccounts.AsQueryable();
        }

        public UserAccount GetById(Int32 id)
        {
            if (id == 0)
                return new UserAccount();
            var user = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
            _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, user);
            return user;
        }

        public UserAccount GetByLogin(string login)
        {
            var user = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.Login == login.Trim());
            _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, user);
            return user;
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
                {
                    _unitOfWork.DbModel.UserAccounts.AddObject(entity);
                    _unitOfWork.DbModel.SaveChanges();
                    _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, entity);
                }
                    
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
                    item.LastAccessProfileCode = entity.LastAccessProfileCode;
                    _unitOfWork.DbModel.SaveChanges();
                    _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, item);
                }
            }
            
            return true;
        }

        public void Delete(Int32 id)
        {
            var item = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
            if (item != null)
            {
                item.UserStatus = UserAccountStatus.Deleted;
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, item);
            }
        }
        public void ChangeState(Int32 id)
        {
            var item = _unitOfWork.DbModel.UserAccounts.SingleOrDefault(x => x.userid == id);
            if (item != null)
            {
                item.UserStatus = item.UserStatus == UserAccountStatus.Active ? UserAccountStatus.Unactive : UserAccountStatus.Active;
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, item);
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