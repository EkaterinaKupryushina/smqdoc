using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.DB;
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
            throw new NotImplementedException();
        }
    }
}