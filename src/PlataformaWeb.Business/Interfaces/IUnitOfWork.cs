using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}