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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class InsumoAlimentoService : Service, IInsumoAlimentoService
    {
        private readonly IInsumoAlimentoRepositorio _insumoAlimentoRepositorio;
        private readonly IFornecedorInsumoRepositorio _fornecedorInsumoRepositorio;
        private readonly IRacaoRepositorio _racaoRepositorio;

        public InsumoAlimentoService(INotificador notificador,
                                     IUser appUser,
                                     IInsumoAlimentoRepositorio insumoAlimentoRepositorio,
                                     IFornecedorInsumoRepositorio fornecedorInsumoRepositorio, 
                                     IRacaoRepositorio racaoRepositorio) : base(notificador, appUser)
        {
            _insumoAlimentoRepositorio = insumoAlimentoRepositorio;
            _fornecedorInsumoRepositorio = fornecedorInsumoRepositorio;
            _racaoRepositorio = racaoRepositorio;
        }

        public async Task Adicionar(InsumoAlimento entity)
        {

            if (!ExecutarValidacao(new InsumoAlimentoValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaInsercaoAlimento(entity)) return;

            await _insumoAlimentoRepositorio.Adicionar(entity);

            await _insumoAlimentoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(InsumoAlimento entity)
        {

            if (!ExecutarValidacao(new InsumoAlimentoValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaInsercaoAlimento(entity)) return;

            await _insumoAlimentoRepositorio.Atualizar(entity);

            await _insumoAlimentoRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<InsumoAlimentoDTO>> Buscar(Expression<Func<InsumoAlimento, bool>> predicate)
        {
            return await _insumoAlimentoRepositorio.BuscarQuery(predicate);
        }

        public async Task<InsumoAlimento> ObterPorId(int id)
        {
            return await _insumoAlimentoRepositorio.ObterPorId(id);
        }

        public async Task<List<InsumoAlimentoDTO>> ObterPaginacao(int? id = null)
        {
            return await _insumoAlimentoRepositorio.ObterPaginacao();
        }

        public async Task AtualizarMateriaSeca(InsumoAlimento insumoAlimento)
        {
            var insumo = await _insumoAlimentoRepositorio.ObterPorId(insumoAlimento.Id);

            if (insumo is null)
            {
                Notificar("Insumo não encontrado na base de dados");
                return;
            }

            if (insumoAlimento.MateriaSeca == insumo.MateriaSeca)
            {
                Notificar("Materia Seca não foi alterada");
                return;
            }

            decimal msAnterior = insumo.MateriaSeca;
            insumo.MateriaSeca = insumoAlimento.MateriaSeca;
            insumo.DataAtualizacaoMateriaSeca = DateTime.Now;            

            //lógica para atualizar nas rações
            var racoes = await _racaoRepositorio.ObterRacoesContemInsumo(insumo.Id);
            foreach (var racao in racoes)
            {
                racao.InsumosRacao.First(c => c.IdInsumoAlimento == insumo.Id).PercentualMateriaSeca = insumo.MateriaSeca;
                racao.CalcularInformacoesInsumos();

                await _racaoRepositorio.Atualizar(racao);
            }

            await _insumoAlimentoRepositorio.AtualizarMateriaSeca(insumo, msAnterior);

            await _insumoAlimentoRepositorio.UnitOfWork.Commit();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Insumo é inválido");
                return;
            }

            var insumoAlimento = await _insumoAlimentoRepositorio.ObterPorId(id);

            if (insumoAlimento is null)
            {
                Notificar("Insumo não existe");
                return;
            }

            await _insumoAlimentoRepositorio.Remover(insumoAlimento);

            await _insumoAlimentoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private async Task<bool> ValidaInsercaoAlimento(InsumoAlimento entity)
        {
            var fornecedorInsumo = await _fornecedorInsumoRepositorio.ObterPorId(entity.IdFornecedor);

            if (fornecedorInsumo is null || fornecedorInsumo.IdCliente != AppUser.ObterIdCliente())
            {
                Notificar("Fornecedor de Insumo não pertence ao Cliente");
                return false;
            }

            return true;
        }

        public async Task<List<LogAtualizacaoMaterialSecaDTO>> ObterLogsAlteracao(int id)
        {
            return await _insumoAlimentoRepositorio.ObterLogsAlteracao(id);
        }
    }
}
