using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class CadeteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public long Telefono { get; set; }
        public List<PedidoViewModel> PedidosDelDia { get; set; }

        public CadeteViewModel()
        {

        }
    }
}
