using System.ComponentModel.DataAnnotations;

namespace Libreria.Web.ViewModels
{
    public record ViewModelLogin
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        public string User { get; set; } = default!;

        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password length policy error")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers are allowed")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;
    }
}
