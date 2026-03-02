using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Id User")]
        public int Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; } = String.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;

        [Display(Name = "Password")]
        public string Password { get; set; } = String.Empty;

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Role Id")]
        public int IdRol { get; set; }

        [Display(Name = "User State")]
        public bool? UserState { get ; set; }

        [Display(Name = "State")]
        public string State
        {
            get
            {
                return UserState == true ? "Active" : "Inactive";
            }
        }

        [Display(Name = "Role")]
        public RolDTO IdRolNavigation { get; set; } = new();
        public int Tally { get; set; }
    }
}
