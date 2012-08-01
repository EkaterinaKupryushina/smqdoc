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
        IQueryable<DocAppointment> GetAllUserDocAppointments(long accountId);

        /// <summary>
        /// Возвращает список назначений доступных всем пользвателеям группы
        /// </summary>
        /// <returns></returns>
        IQueryable<DocAppointment> GetAllGroupDocAppointments(long groupId);

        /// <summary>
        /// Возвращает объет назначение по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DocAppointment GetDocAppointmentById(long id);

        /// <summary>
        /// Сохраняем объект назначение в базу
        /// </summary>
        /// <param name="entity"></param>
        void SaveDocAppointment(DocAppointment entity);

        /// <summary>
        /// Сохраняем объект назначение в базу
        /// </summary>
        /// <param name="id"></param>
        void DeleteDocAppointment(long id);

        /// <summary>
        /// Меняет стутус назначение с Active на unactive
        /// </summary>
        /// <param name="id"></param>
        void ChangeDocAppointmentState(long id);
    }
}