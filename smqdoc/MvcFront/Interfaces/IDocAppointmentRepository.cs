using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IDocAppointmentRepository
    {

        /// <summary>
        /// Возвращает список назначений доступных всем пользвателеям
        /// </summary>
        /// <returns></returns>
        IQueryable<DocAppointment> GetAllUserDocAppointments();

        /// <summary>
        /// Возвращает список назначений доступных всем пользвателеям группы
        /// </summary>
        /// <returns></returns>
        IQueryable<DocAppointment> GetAllGroupDocAppointments(long groupId);


    }
}