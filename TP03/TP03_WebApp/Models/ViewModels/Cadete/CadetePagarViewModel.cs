using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class CadetePagarViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public List<PedidoViewModel> PedidosDelDia { get; set; }

        public CadetePagarViewModel()
        {

        }
    }
}
