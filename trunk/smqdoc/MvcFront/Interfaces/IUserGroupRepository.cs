using System;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUserGroupRepository
    {
        IQueryable<UserGroup> GetAll();
        UserGroup GetById(Int32 id);
        UserGroup GetByGroupName(string groupName);
        bool Save(UserGroup entity);
        void Delete(Int32 id);
        void ChangeState(Int32 id);
        UserGroup Copy(IUnitOfWork uw, int usergroupid);
        bool RemoveMember(int groupId,int userId);
        bool AddMember(int groupId, int userId);
    }
}
