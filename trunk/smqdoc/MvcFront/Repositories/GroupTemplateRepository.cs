using System;
using System.Collections.Generic;
using System.Linq;
using MvcFront.Enums;
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

        public IEnumerable<GroupTemplate> GetGroupTemplateByGroupId(Int64 id)
        {
            return GetAllGroupTemplates().Where(x => x.UserGroup_usergroupid == id).ToList();
        }
        public IQueryable<GroupTemplate> GetAllGroupTemplates()
        {
            return _unitOfWork.DbModel.GroupTemplates.AsQueryable();            
        }

        public GroupTemplate GetGroupTemplateById(long id)
        {
            if (id == 0)
                return new GroupTemplate { ActualStartDate = DateTime.Now, PlanedStartDate = DateTime.Now, ActualEndDate = DateTime.Now, PlanedEndDate = DateTime.Now };
            return _unitOfWork.DbModel.GroupTemplates.SingleOrDefault(x=> x.grouptemplateid == id);
        }

        public bool SaveGroupTemplate(GroupTemplate entity)
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
                    oldEntity.DocAppointment = entity.DocAppointment;
                    oldEntity.ActualStartDate= entity.ActualStartDate;
                    oldEntity.ActualEndDate= entity.ActualEndDate;
                    oldEntity.PlanedStartDate = entity.PlanedStartDate;
                    oldEntity.PlanedEndDate = entity.PlanedEndDate;
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
                entity.DocAppointment.DocAppointmentStatus = DocAppointmentStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();            
        }

        public void ChangeGroupTemplateState(long id, DocAppointmentStatus status)
        {
            var entity = GetGroupTemplateById(id);
            if (entity != null)
            {
                entity.DocAppointment.DocAppointmentStatus = status;
            }
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}