using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUnitOfWork
    {
        smqdocEntities DbModel { get; }
    }
}
