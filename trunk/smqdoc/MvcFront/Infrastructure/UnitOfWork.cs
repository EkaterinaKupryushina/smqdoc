using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly smqdocEntities _dbModel;
        public UnitOfWork()
        {
            _dbModel = new smqdocEntities();
        }
        public smqdocEntities DbModel
        {
            get { return _dbModel; }
        }
    }
}