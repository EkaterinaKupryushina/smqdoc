using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;


namespace MvcFront.Repositories
{
    public class UserGroupRepository :IUserGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IUserAccountRepository _userRepository;
        public UserGroupRepository(IUnitOfWork unitOfWork,IUserAccountRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public IQueryable<UserGroup> GetAll()
        {
            return _unitOfWork.DbModel.UserGroups.AsQueryable<UserGroup>();
        }

        public UserGroup GetById(int id)
        {
            if (id == 0)
                return new UserGroup();
            else
                return _unitOfWork.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == id);
        }
        public UserGroup GetByGroupName(string groupName)
        {
            return _unitOfWork.DbModel.UserGroups.SingleOrDefault(x => x.GroupName == groupName.Trim());
        }

        /// <summary>
        /// Сохраняет сущность в базу !! Не использовать при работе с пользователями
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(UserGroup entity)
        {
            if (entity.usergroupid == 0)
            {
                if (this.GetByGroupName(entity.GroupName) == null )
                    _unitOfWork.DbModel.UserGroups.AddObject(entity);
                else
                    return false;
            }
            else
            {
                var item = _unitOfWork.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == entity.usergroupid);
                if (item != null)
                {
                    item.FullGroupName = entity.FullGroupName;
                    item.GroupName = entity.GroupName;
                    item.Managerid = entity.Managerid;
                    item.Status = entity.Status;
                    item.usergroupid = entity.usergroupid;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void Delete(int id)
        {
            var item = _unitOfWork.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == id);
            if (item != null)
            {
                item.GroupStatus = UserGroupStatus.Deleted;
                _unitOfWork.DbModel.SaveChanges();
            }
        }
        public bool AddMember(int groupId, int userId)
        {
            var group = this.GetById(groupId);
            var user = group.Members.FirstOrDefault(x => x.userid == userId);
            if (user == null)
            {
                user = _userRepository.Copy(_unitOfWork,userId);
                if (user == null)
                    return false;
                group.Members.Add(user);
                _unitOfWork.DbModel.SaveChanges();
            }
                
            return true;
        }
        public bool RemoveMember(int groupId, int userId)
        {
            var group = this.GetById(groupId);
            var user = group.Members.FirstOrDefault(x => x.userid == userId);
            if (user != null) 
                group.Members.Remove(user);
            else 
                return false;
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }
        public void ChangeState(int id)
        {
            var item = _unitOfWork.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == id);
            if (item != null)
            {
                if (item.GroupStatus == UserGroupStatus.Active)
                    item.GroupStatus = UserGroupStatus.Unactive;
                else
                    item.GroupStatus = UserGroupStatus.Active;
                _unitOfWork.DbModel.SaveChanges();
            }
        }
        public UserGroup Copy(IUnitOfWork uw, int usergroupid)
        {
            if (usergroupid == 0)
                return new UserGroup();
            else
                return uw.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == usergroupid);
        }
    }
}