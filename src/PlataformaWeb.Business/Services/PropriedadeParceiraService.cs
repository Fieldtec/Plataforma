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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class PropriedadeParceiraService : Service, IPropriedadeParceiraService
    {
        private readonly IPropriedadeParceiraRepositorio _propriedadeParceiraRepositorio;

        public PropriedadeParceiraService(INotificador notificador, 
                                          IUser appUser, 
                                          IPropriedadeParceiraRepositorio propriedadeParceiraRepositorio) : base(notificador, appUser)
        {
            _propriedadeParceiraRepositorio = propriedadeParceiraRepositorio;
        }

        public async Task Adicionar(PropriedadeParceira entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PropriedadeParceiraValidation(TipoOperacao.Inclusao), entity)) return;

            await _propriedadeParceiraRepositorio.Adicionar(entity);

            await _propriedadeParceiraRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(PropriedadeParceira entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PropriedadeParceiraValidation(TipoOperacao.Atualizacao), entity)) return;

            await _propriedadeParceiraRepositorio.Atualizar(entity);

            await _propriedadeParceiraRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<PropriedadeParceira>> Buscar(Expression<Func<PropriedadeParceira, bool>> predicate)
        {
            return await _propriedadeParceiraRepositorio.Buscar(predicate);
        }

        public async Task<PropriedadeParceira> ObterPorId(int id)
        {
            return await _propriedadeParceiraRepositorio.ObterPorId(id);
        }

        public async Task<List<PropriedadeParceiraDTO>> ObterPaginacao(int? id = null)
        {
            return await _propriedadeParceiraRepositorio.ObterPaginacao(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Propriedade é inválida");
                return;
            }

            var propriedadeParceira = await _propriedadeParceiraRepositorio.ObterPorId(id);

            if (propriedadeParceira is null)
            {
                Notificar("Propriedade não existe");
                return;
            }

            await _propriedadeParceiraRepositorio.Remover(propriedadeParceira);

            await _propriedadeParceiraRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
