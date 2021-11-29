using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class FornecedorInsumo : Entity
    {
        public FornecedorInsumo()
        {
            InsumosAlimentos = new List<InsumoAlimento>();
            SuplementosMinerais = new List<SuplementoMineral>();
        }

        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string CpfCnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string ContatoPessoa { get; set; }        
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<InsumoAlimento> InsumosAlimentos { get; set; }
        public virtual List<SuplementoMineral> SuplementosMinerais { get; set; }
    }
}
