using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public string Password { get; set; } = String.Empty;

        public DateTime RegistrationDate { get; set; }

        public int IdRol { get; set; }

        public bool? UserState { get; set; }

        public RolDTO IdRolNavigation { get; set; } = new();
    }
}
