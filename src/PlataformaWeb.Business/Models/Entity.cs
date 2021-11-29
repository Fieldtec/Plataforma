using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public Entity()
        {
            Status = Status.Ativado;
        }
    }
}
