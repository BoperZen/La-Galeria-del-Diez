using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record RolDTO
    {
        public int Id { get; set; }
        [Display (Name = "Role")]
        public string Description { get; set; } = String.Empty;
    }
}
