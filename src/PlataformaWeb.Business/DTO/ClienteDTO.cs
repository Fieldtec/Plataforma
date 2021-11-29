using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Propriedade { get; set; }
        public string Nome { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public Status Status { get; set; }
        public string Tecnico { get; set; }

        public ClienteDTO()
        { }

        public ClienteDTO(Cliente model)
        {
            Id = model.Id;
            Propriedade = model.NomePropriedade;
            Nome = model.Nome;
            Municipio = model.Cidade;
            Uf = model.Uf;
            Status = model.Status;
            Tecnico = model.Tecnico?.Nome;
        }
    }
}
