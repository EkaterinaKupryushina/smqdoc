using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private smqdocEntities _dbModel;
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