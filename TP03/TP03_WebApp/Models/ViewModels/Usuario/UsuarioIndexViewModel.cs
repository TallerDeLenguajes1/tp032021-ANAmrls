using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class UsuarioIndexViewModel
    {
        [Required(ErrorMessage = "Ingrese su nombre de usuario")]
        [StringLength(15)]
        [Display(Name = "Nombre de Usuario")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Ingrese su contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
