using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class LoteSaidaViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Número do Lote")]
        public int NumeroLote { get; set; }

        [Required(ErrorMessage = "Data do Embarque é obrigatório")]
        [DisplayName("Data do Embarque")]
        public DateTime? DataEmbarque { get; set; }

        [Required(ErrorMessage = "Tipo da Saída é obrigatório")]
        [DisplayName("Tipo da Saída")]
        public TipoSaida? TipoSaida { get; set; }

        [DisplayName("Produtor Destino")]
        public ProdutorParceiroViewModel ProdutorDestino { get; set; }

        [DisplayName("Frigorifico Destino")]
        public FrigorificoViewModel FrigorificoDestino { get; set; }

        [DisplayName("Qtd. Animais Previsto")]
        public int? QuantidadeAnimalPrevisto { get; set; }

        [DisplayName("Qtd. Animais Embarcados")]
        public int QuantidadeAnimaEmbarcado { get; set; }
    }
}
