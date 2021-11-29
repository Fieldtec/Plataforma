using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class LogAtualizacaoMaterialSecaDTO
    {
        public int Id { get; set; }
        public string Insumo { get; set; }
        public String Usuario { get; set; }
        public DateTime DataAlteracao { get; set; }
        public decimal MateriaSecaAtual { get; set; }
        public decimal MateriaSecaAnterior { get; set; }
    }
}
