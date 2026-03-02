using System.ComponentModel.DataAnnotations;
using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record BiddingDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int IdUser { get; set; }

        public int IdAuction { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
    }
}
