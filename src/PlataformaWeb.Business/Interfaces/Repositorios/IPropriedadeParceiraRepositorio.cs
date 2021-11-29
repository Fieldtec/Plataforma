using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IPropriedadeParceiraRepositorio : IRepositorio<PropriedadeParceira>
    {
        Task<List<PropriedadeParceiraDTO>> ObterPaginacao(int? idCliente = null);
    }
}
