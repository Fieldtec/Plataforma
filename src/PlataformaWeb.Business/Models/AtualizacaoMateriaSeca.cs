using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class AtualizacaoMateriaSeca
    {
        public int Id { get; set; }
        public int IdInsumo { get; set; }
        public InsumoAlimento Insumo { get; set; }
        public decimal MateriaSecaAnterior { get; set; }
        public decimal MateriaSecaAtual { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int IdUsuario { get; set; }
    }
}
