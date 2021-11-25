using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "Por favor ingrese un nombre de usuario")]
        [StringLength(15)]
        [Display(Name = "Nombre de Usuario")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese una dirección de correo válida")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña")]
        [StringLength(15, ErrorMessage = "La {0} debe tener al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide con la confirmación.")]
        public string ConfirmPassword { get; set; }

        public UsuarioCreateViewModel()
        {

        }
    }
}
