using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using MvcFront.Interfaces;
using MvcFront.Repositories;

namespace MvcFront.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        #endregion

        public IBindingToSyntax<T> Bind<T>()
        {
            return _kernel.Bind<T>();
        }

        private void AddBindings()
        {
            //Bind<IUnitOfWork>().To<UnitOfWork>().InTransientScope().OnDeactivation(u => u.Dispose());
            Bind<IUserAccountRepository>().To<UserAccountRepository>();
            Bind<IUserGroupRepository>().To<UserGroupRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}