using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using MvcFront.Interfaces;
using MvcFront.Repositories;

namespace MvcFront.Infrastructure
{
    /// <summary>
    /// Класс помошник для IoC Ninject
    /// </summary>
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
            Bind<IUserAccountRepository>().To<UserAccountRepository>();
            Bind<IUserGroupRepository>().To<UserGroupRepository>();
            Bind<IDocTemplateRepository>().To<DocTemplateRepository>();
            Bind<IDocumentRepository>().To <DocumentRepository>();
            Bind<IGroupTemplateRepository>().To<GroupTemplateRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}