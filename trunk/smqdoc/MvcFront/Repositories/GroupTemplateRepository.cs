using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class GroupTemplateRepository : IGroupTemplateRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public GroupTemplateRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<GroupTemplate> GetGroupTemplateByGroupId(Int64 id)
        {
            return GetAllGroupTemplates().Where(x => x.UserGroup_usergroupid == id).ToList();
        }
        public IQueryable<DB.GroupTemplate> GetAllGroupTemplates()
        {
            return _unitOfWork.DbModel.GroupTemplates.AsQueryable();            
        }

        public DB.GroupTemplate GetGroupTemplateById(long id)
        {
            if (id == 0)
                return new GroupTemplate() { DateStart = DateTime.Now, DateEnd = DateTime.Now };
            return _unitOfWork.DbModel.GroupTemplates.SingleOrDefault(x=> x.grouptemplateid == id);
        }

        public bool SaveGroupTemplate(DB.GroupTemplate entity)
        {
            if (entity.grouptemplateid == 0)
            {                                
                _unitOfWork.DbModel.GroupTemplates.AddObject(entity);
            }
            else
            {
                var oldEntity = GetGroupTemplateById(entity.grouptemplateid);
                if (oldEntity == null)
                {
                    entity.grouptemplateid = 0;
                    SaveGroupTemplate(entity);
                }
                if (oldEntity != null)
                {
                    oldEntity.UserGroup_usergroupid = entity.UserGroup_usergroupid;
                    oldEntity.DocTemplate_docteplateid = entity.DocTemplate_docteplateid;
                    oldEntity.Name = entity.Name;
                    oldEntity.DateStart = entity.DateStart;
                    oldEntity.DateEnd = entity.DateEnd;
                    oldEntity.Status = entity.Status;
                }
            }
            _unitOfWork.DbModel.SaveChanges();
            return true;
        }

        public void DeleteGroupTemplate(long id)
        {
            var entity = GetGroupTemplateById(id);
            if (entity != null)
            {
                entity.GroupTemplateStatus = GroupTemplateStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();            
        }

        public void ChangeGroupTemplateState(long id, GroupTemplateStatus status)
        {
            var entity = GetGroupTemplateById(id);
            if (entity != null)
            {
                entity.GroupTemplateStatus = status;
            }
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}