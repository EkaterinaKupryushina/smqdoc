using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Infrastructure
{
    /// <summary>
    /// Помошник для IoC
    /// </summary>
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