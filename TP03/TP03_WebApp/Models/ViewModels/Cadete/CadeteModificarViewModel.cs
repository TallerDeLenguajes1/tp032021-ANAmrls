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
        public string Direccion { get; set; }
        [Required]
        [Phone]
        public long Telefono { get; set; }

        public CadeteModificarViewModel()
        {

        }
    }
}
