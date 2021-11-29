using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;

namespace PlataformaWeb.Data.Repositorio
{
    public class InsumoAlimentoRepositorio : Repositorio<InsumoAlimento>, IInsumoAlimentoRepositorio
    {
        public InsumoAlimentoRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }

        public override Expression<Func<InsumoAlimento, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
            where = where.And(x => x.FornecedorInsumo.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task Atualizar(InsumoAlimento entity)
        {
            await base.Atualizar(entity);

            //Retirando o campo Materia Seca do Tracker para não adicionar via formulário
            Context.ChangeTracker.Entries()
                    .FirstOrDefault(entry => entry.Entity.GetType() == typeof(InsumoAlimento)
                            && (int)entry.Property("Id").CurrentValue == entity.Id).Property("MateriaSeca").IsModified = false;            
        }

        public async override Task<InsumoAlimento> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.FornecedorInsumo)
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<InsumoAlimentoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.FornecedorInsumo)
                                    .ThenInclude(c => c.Cliente)
                                    .ThenInclude(t => t.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new InsumoAlimentoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  ValorKg = x.ValorKg,
                                  MateriaSeca = x.MateriaSeca,
                                  Proprietario = x.FornecedorInsumo.Cliente.Nome,
                                  NomeFornecedorInsumo = x.FornecedorInsumo.Nome,
                                  Tecnico = x.FornecedorInsumo.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<IEnumerable<InsumoAlimentoDTO>> BuscarQuery(Expression<Func<InsumoAlimento, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                                .Include(x => x.FornecedorInsumo)
                                .Where(ObterWhere().And(predicate))
                                .Select(x => new InsumoAlimentoDTO
                                {
                                    Id = x.Id,
                                    Nome = x.Nome,
                                    ValorKg = x.ValorKg,
                                    MateriaSeca = x.MateriaSeca,
                                    NomeFornecedorInsumo = x.FornecedorInsumo.Nome
                                }).ToListAsync();
        }

        public async Task AtualizarMateriaSeca(InsumoAlimento insumo, decimal materiaSecaAnterior)
        {
            await base.Atualizar(insumo);

            AtualizacaoMateriaSeca logAtualizacao = new AtualizacaoMateriaSeca
            {
                DataAtualizacao = DateTime.Now,
                IdInsumo = insumo.Id,
                IdUsuario = AppUser.ObterId(),
                MateriaSecaAnterior = materiaSecaAnterior,
                MateriaSecaAtual = insumo.MateriaSeca
            };
            
            await DbConnection.ExecuteAsync("INSERT INTO atualizacaoms " +
                "   (idinsumo, msanterior, msatual, dataatualizacao, idusuario) " +
                " VALUES (@IdInsumo, @MateriaSecaAnterior, @MateriaSecaAtual, @DataAtualizacao, @IdUsuario) ", logAtualizacao);
        }

        public async Task<List<LogAtualizacaoMaterialSecaDTO>> ObterLogsAlteracao(int id)
        {
            var resultado = await DbConnection.QueryAsync<LogAtualizacaoMaterialSecaDTO>(" SELECT " +
                "  a.Id, i2.nome Insumo, p.nome Usuario, a.dataatualizacao DataAlteracao, a.msanterior MateriaSecaAnterior, a.msatual MateriaSecaAtual " +
                "FROM atualizacaoms a " +
                "INNER JOIN insumosalimento i2 on i2.id = a.idinsumo " +
                "LEFT JOIN pessoas p on p.id = a.idusuario " +
                "WHERE a.idinsumo = @id", new { id });

            return resultado.ToList();
        }
    }
}
