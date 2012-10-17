using System;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class DocAppointmentRepository : IDocAppointmentRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public DocAppointmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<DocAppointment> GetAllPersonalDocAppointments(long accountId, int groupId, bool includeNotStarted = false)
        {
            var query =
                _unitOfWork.DbModel.DocAppointments.Where(
                    x =>
                    x.UserGroup_usergroupid == groupId && x.UserAccount_userid == accountId 
                    && x.DocTemplate.Status == (int) DocTemplateStatus.Active &&
                    x.Status == (int) DocAppointmentStatus.Active);
            if (!includeNotStarted)
            {
                query = query.Where(x => x.PlanedStartDate <= DateTime.Now || x.ActualStartDate <= DateTime.Now);
            }
            return query;
        }

        public IQueryable<DocAppointment> GetAllGroupDocAppointments(long groupId, bool includeNotStarted = false)
        {
            var query =
                _unitOfWork.DbModel.DocAppointments.Where(
                    x =>
                    x.UserAccount_userid == null && x.UserGroup_usergroupid == groupId 
                    && x.DocTemplate.Status == (int) DocTemplateStatus.Active &&
                    x.Status == (int) DocAppointmentStatus.Active);
            if (!includeNotStarted)
            {
                query = query.Where(x => x.PlanedStartDate <= DateTime.Now || x.ActualStartDate <= DateTime.Now);
            }
            return query;
        }

        public DocAppointment GetDocAppointmentById(long id)
        {
            if(id == 0)
            {
                return new DocAppointment();
            }
            return _unitOfWork.DbModel.DocAppointments.SingleOrDefault(x => x.docappointmentid == id);
        }

        public void SaveDocAppointment(DocAppointment entity)
        {
            if (entity.docappointmentid== 0)
            {
                _unitOfWork.DbModel.DocAppointments.AddObject(entity);
            }
            else
            {
                _unitOfWork.DbModel.DocAppointments.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void DeleteDocAppointment(long id)
        {
            var entity = GetDocAppointmentById(id);
            if (entity != null)
            {
                entity.DocAppointmentStatus = DocAppointmentStatus.Deleted;
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void ChangeDocAppointmentState(long id)
        {
            var entity = GetDocAppointmentById(id);
            if (entity != null)
            {
                entity.DocAppointmentStatus = entity.DocAppointmentStatus != DocAppointmentStatus.Active ? DocAppointmentStatus.Active : DocAppointmentStatus.Unactive;
            }
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}