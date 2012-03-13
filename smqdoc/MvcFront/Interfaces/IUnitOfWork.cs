using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IUnitOfWork
    {
        smqdocEntities DbModel { get; }
    }
}
