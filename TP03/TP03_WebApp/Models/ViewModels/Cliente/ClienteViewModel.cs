using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class ClienteViewModel
    {
        [Display(Name = "Nro de Cliente")]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }
                
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Número de Teléfono")]
        public long Telefono { get; set; }

        public ClienteViewModel()
        {

        }
    }
}
