using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class SuplementoMineralDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeFornecedorInsumo { get; set; }
        public decimal ConsumoEsperado { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
    }
}
