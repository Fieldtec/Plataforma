using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Extensions
{
    public static class RazorHelper
    {

        public static string FormataDecimal(this RazorPage page, decimal? valor, int quantidadeCasas)
        {
            if (valor.HasValue)
                return valor.Value.ToString($"N{quantidadeCasas}");

            return "";
        }

        public static HtmlString ObterBadgeStatus(this RazorPage page, bool status)
        {
            String badge = String.Format("<span class='badge badge-{0}'>{1}</span>",
                                status ? "success" : "danger", status ? "Sim" : "Não");

            return new HtmlString(badge);
        }

        public static HtmlString ObterBadgeStatus(this RazorPage page, Status status)
        {
            String badge = String.Format("<span class='badge badge-{0}'>{1}</span>",
                                status == Status.Ativado ? "success" : "danger", status == Status.Ativado ? "Sim" : "Não");

            return new HtmlString(badge);
        }

        public static List<SelectListItem> ObterSelectTipoPastoCurral(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoPastoCurral tipo in Enum.GetValues(typeof(TipoPastoCurral)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectOrigemFornecimento(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (OrigemSuplemento tipo in Enum.GetValues(typeof(OrigemSuplemento)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectDestinoFornecimento(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (DestinoSuplemento tipo in Enum.GetValues(typeof(DestinoSuplemento)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSexo(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (Sexo sexo in Enum.GetValues(typeof(Sexo)))
            {
                select.Add(new SelectListItem { Value = (sexo).ToString(), Text = sexo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectTipoMovimentacaoEntrePastos(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoMovimentacaoEntreLotes tipo in Enum.GetValues(typeof(TipoMovimentacaoEntreLotes)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectTipoRacao(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoRacao tipo in Enum.GetValues(typeof(TipoRacao)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectTipoPlanejamento(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoPlanejamentoNutricional tipo in Enum.GetValues(typeof(TipoPlanejamentoNutricional)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterListaSuspensa(this RazorPage page, List<SelectListItem> data)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            select.AddRange(data);

            return select;
        }

        public static List<SelectListItem> ObterSelectTipoEntrada(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoEntradaLote tipo in Enum.GetValues(typeof(TipoEntradaLote)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectTipoSaida(this RazorPage page)
        {
            List<SelectListItem> select = new List<SelectListItem>();
            select.Add(new SelectListItem { Value = "", Text = "SELECIONE" });

            foreach (TipoSaida tipo in Enum.GetValues(typeof(TipoSaida)))
            {
                select.Add(new SelectListItem { Value = ((int)tipo).ToString(), Text = tipo.ObterDescricao() });
            }

            return select;
        }

        public static List<SelectListItem> ObterSelectEstados(this RazorPage page)
        {
            return new List<SelectListItem>
            {
                    new SelectListItem { Value = "" , Text = "SELECIONE"  },
                    new SelectListItem { Value = "AC" , Text = "ACRE"  },
                    new SelectListItem { Value = "AL" , Text = "ALAGOAS" },
                    new SelectListItem { Value = "AP" , Text = "AMAPÁ"},
                    new SelectListItem { Value = "AM" , Text = "AMAZONAS"},
                    new SelectListItem { Value = "BA" , Text = "BAHIA"},
                    new SelectListItem { Value = "CE" , Text = "CEARÁ"},
                    new SelectListItem { Value = "DF" , Text = "DISTRITO FEDERAL"},
                    new SelectListItem { Value = "ES" , Text = "ESPÍRITO SANTO"},
                    new SelectListItem { Value = "GO" , Text = "GOIÁS"},
                    new SelectListItem { Value = "MA" , Text = "MARANHÃO"},
                    new SelectListItem { Value = "MT" , Text = "MATO GROSSO"},
                    new SelectListItem { Value = "MS" , Text = "MATO GROSSO DO SUL"},
                    new SelectListItem { Value = "MG" , Text = "MINAS GERAIS"},
                    new SelectListItem { Value = "PA" , Text = "PARÁ"},
                    new SelectListItem { Value = "PB" , Text = "PARAÍBA"},
                    new SelectListItem { Value = "PR" , Text = "PARANÁ"},
                    new SelectListItem { Value = "PE" , Text = "PERNAMBUCO"},
                    new SelectListItem { Value = "PI" , Text = "PIAUÍ"},
                    new SelectListItem { Value = "RJ" , Text = "RIO DE JANEIRO"},
                    new SelectListItem { Value = "RN" , Text = "RIO GRANDE DO NORTE"},
                    new SelectListItem { Value = "RS" , Text = "RIO GRANDE DO SUL"},
                    new SelectListItem { Value = "RO" , Text = "RONDÔNIA"},
                    new SelectListItem { Value = "RR" , Text = "RORAIMA"},
                    new SelectListItem { Value = "SC" , Text = "SANTA CATARINA"},
                    new SelectListItem { Value = "SP" , Text = "SÃO PAULO"},
                    new SelectListItem { Value = "SE" , Text = "SERGIPE"},
                    new SelectListItem { Value = "TO" , Text = "TOCANTINS"}
            };

           
        }
    }
}
