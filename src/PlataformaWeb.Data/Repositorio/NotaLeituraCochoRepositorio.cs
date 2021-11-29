using Dapper;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlataformaWeb.Data.Repositorio
{
    public class NotaLeituraCochoRepositorio : Repositorio<NotaLeituraCocho>, INotaLeituraCochoRepositorio
    {
        public NotaLeituraCochoRepositorio(PlataformaFieldContext context, IUser appUser) 
            : base(context, appUser)
        {
        }

        public async override Task<List<NotaLeituraCocho>> ObterTodos()
        {
            var where = base.ObterWhere().And(x => x.IdCliente == AppUser.ObterIdCliente());

            return await DbSet.AsNoTracking()
                              .Where(where)
                              .OrderBy(x => x.AjustePorcentagem)
                              .ToListAsync();
        }

        public async override Task<NotaLeituraCocho> ObterPorId(int id)
        {
            var where = base.ObterWhere().And(x => x.IdCliente == AppUser.ObterIdCliente());
            return await DbSet.AsNoTracking().Where(where).FirstOrDefaultAsync();
        }

        public async Task AdicionarLog(List<NotaLeituraCocho> notas)
        {

            foreach (var nota in notas)
            {
                LogNotaLeituraCocho log = new LogNotaLeituraCocho
                {
                    AjustePorcentagem = nota.AjustePorcentagem,
                    DataRegistro = nota.DataRegistro,
                    DataAlteracao = DateTime.Now,
                    IdCliente = nota.IdCliente,
                    IdNota = nota.Id,
                    IdUsuario = AppUser.ObterId(),
                    Nome = nota.Nome
                };

                await DbConnection.ExecuteAsync("INSERT INTO lognotasleituracocho " +
                    "(idnota, nome, ajusteporcentagem, dataregistro, idusuario, dataalteracao, idcliente) " +
                    "VALUES(@IdNota, @Nome, @AjustePorcentagem, CURRENT_TIMESTAMP, @IdUsuario, @DataAlteracao, @IdCliente) ", log);

            }

        }

    }
}
