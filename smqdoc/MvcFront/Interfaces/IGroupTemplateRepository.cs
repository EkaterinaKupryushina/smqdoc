using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IGroupTemplateRepository
    {
        IQueryable<GroupTemplate> GetAllGroupTemplates();
        GroupTemplate GetGroupTemplateById(Int64 id);
        bool SaveGroupTemplate(GroupTemplate entity);
        void DeleteGroupTemplate(Int64 id);
        void ChangeGroupTemplateState(Int64 id, GroupTemplateStatus status);
    }
}
