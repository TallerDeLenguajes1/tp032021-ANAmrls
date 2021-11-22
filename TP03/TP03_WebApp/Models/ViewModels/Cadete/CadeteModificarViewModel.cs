using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class CadeteModificarViewModel
    {        
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Por favor ingrese su Teléfono")]
        [RegularExpression(pattern: "^[0-9]{10}$",
            ErrorMessage = "Ingrese un número de teléfono válido",
            MatchTimeoutInMilliseconds = 350)]
        [Display(Name = "Número de Teléfono")]
        public string Telefono { get; set; }

        public CadeteModificarViewModel()
        {

        }
    }
}
