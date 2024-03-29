﻿using System.Linq;
using MvcFront.Enums;
using MvcFront.Interfaces;
using MvcFront.DB;


namespace MvcFront.Repositories
{
    public class UserGroupRepository :IUserGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccountRepository _userRepository;
        public UserGroupRepository(IUnitOfWork unitOfWork,IUserAccountRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public IQueryable<UserGroup> GetAll()
        {
            return _unitOfWork.DbModel.UserGroups.AsQueryable();
        }

        public UserGroup GetById(int id)
        {
            if (id == 0)
                return new UserGroup();
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
                if (GetByGroupName(entity.GroupName) == null )
                    _unitOfWork.DbModel.UserGroups.AddObject(entity);
                else
                    return false;
            }
            else
            {
                _unitOfWork.DbModel.UserGroups.ApplyCurrentValues(entity);
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
            var group = GetById(groupId);
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
            var group = GetById(groupId);
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
                item.GroupStatus = item.GroupStatus == UserGroupStatus.Active ? UserGroupStatus.Unactive : UserGroupStatus.Active;
                _unitOfWork.DbModel.SaveChanges();
            }
        }

        public UserGroup Copy(IUnitOfWork uw, int usergroupid)
        {
            return usergroupid == 0 ? new UserGroup() : uw.DbModel.UserGroups.SingleOrDefault(x => x.usergroupid == usergroupid);
        }
    }
}