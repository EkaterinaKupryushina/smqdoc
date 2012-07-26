using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public IQueryable<DocAppointment> GetAllUserDocAppointments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<DocAppointment> GetAllGroupDocAppointments(long groupId)
        {
           return _unitOfWork.DbModel.DocAppointments.Where(x => x.UserGroup != null && x.UserGroup.usergroupid == groupId);
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
    }
}