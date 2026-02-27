using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record CategoryDTO
    {
        public int Id { get; set; }

        public string Description { get; set; } = String.Empty;
    }
}
