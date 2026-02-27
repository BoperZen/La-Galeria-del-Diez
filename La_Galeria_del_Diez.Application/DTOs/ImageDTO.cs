using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record ImageDTO
    {
        public int Id { get; set; }

        public byte[] Data { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }

        public int IdObject { get; set; }

        public virtual AuctionableObject IdObjectNavigation { get; set; } = null!;
    }
}
