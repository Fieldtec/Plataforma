using Dapper;
using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class FuncoesRepositorio : IFuncoesRepositorio
    {

        private readonly PlataformaFieldContext _context;
        private readonly IDbConnection _connection;

        public FuncoesRepositorio(PlataformaFieldContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
        }

        public async Task AtualizaGmd(int idCliente)
        {
            await _connection.ExecuteAsync("select atualiza_gmd(@idCliente)", new { idCliente });
        }

        public async Task AtualizaLote(int idCliente, DateTime dataBase)
        {
            await _connection.ExecuteAsync("select atualiza_lote(@idCliente, to_date(cast(@dataBase as TEXT),'YYYY-MM-DD'))", new { idCliente, dataBase = dataBase.ToString("yyyy-MM-dd") });
        }

        public async Task AtualizaLote(int idCliente, int idLote, DateTime dataBase)
        {
            await _connection.ExecuteAsync("select atualiza_lote(@idCliente, @idLote, to_date(cast(@dataBase as TEXT),'YYYY-MM-DD'))", new { idCliente, idLote, dataBase = dataBase.ToString("yyyy-MM-dd") });
        }
    }
}
