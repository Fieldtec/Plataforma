using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class TecnicoRepositorio : Repositorio<Tecnico>, ITecnicoRepositorio
    {
        public TecnicoRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
            
        }

        public async Task<int> BuscaLicencasDisponiveis(int tecnicoId)
        {
            var licencasAtivas = await Context.Cliente.CountAsync(x => x.IdTecnico == tecnicoId && x.Status == Business.Enums.Status.Ativado);
            var quantidadeLicencas = await BuscaQtdLicencas(tecnicoId);

            return quantidadeLicencas - licencasAtivas;
        }       

        public async Task<int> BuscaQtdLicencas(int tecnicoId)
        {
            return await DbSet.Where(x => x.Id == tecnicoId)
                              .Select(x => x.QtdeLicenca)
                              .FirstOrDefaultAsync();
        }
    }
}
