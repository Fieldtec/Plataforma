using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class FaseDoAnoService : Service, IFaseDoAnoService
    {
        private readonly IFaseDoAnoRepositorio _faseDoAnoRepositorio;

        public FaseDoAnoService(INotificador notificador, 
                                IUser appUser, 
                                IFaseDoAnoRepositorio faseDoAnoRepositorio) : base(notificador, appUser)
        {
            _faseDoAnoRepositorio = faseDoAnoRepositorio;
        }

        public async Task Adicionar(FaseDoAno entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FaseDoAnoValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaDataDaFase(entity)) return;

            await _faseDoAnoRepositorio.Adicionar(entity);

            await _faseDoAnoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(FaseDoAno entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FaseDoAnoValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaDataDaFase(entity)) return;

            await _faseDoAnoRepositorio.Atualizar(entity);

            await _faseDoAnoRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<FaseDoAnoDTO>> Buscar(Expression<Func<FaseDoAno, bool>> predicate)
        {
            return await _faseDoAnoRepositorio.BuscarQuery(predicate);
        }

        public async Task<FaseDoAno> ObterPorId(int id)
        {
            return await _faseDoAnoRepositorio.ObterPorId(id);
        }

        public async Task<List<FaseDoAnoDTO>> ObterPaginacao(int? id = null)
        {
            return await _faseDoAnoRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Fase do Ano é inválida");
                return;
            }

            var faseDoAno = await _faseDoAnoRepositorio.ObterPorId(id);

            if (faseDoAno is null)
            {
                Notificar("Fase do ano não existe");
                return;
            }

            await _faseDoAnoRepositorio.Remover(faseDoAno);

            await _faseDoAnoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private async Task<bool> ValidaDataDaFase(FaseDoAno fase)
        {
            var fasesNaData = await _faseDoAnoRepositorio.ObterFaseNoPeriodo(fase);

            if (fasesNaData.Count > 0)
            {
                foreach (var faseNaData in fasesNaData.OrderBy(x => x.DataInicio))
                    Notificar($"A fase {faseNaData.Nome} com datas {faseNaData.DataInicio.Value.ToShortDateString()} e {faseNaData.DataFim.Value.ToShortDateString()} colidem com as informações");                

                return false;
            }

            return true;
        }
    }

}
