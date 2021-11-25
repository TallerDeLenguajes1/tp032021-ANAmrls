using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.ViewModels
{
    public class PedidoIndexViewModel
    {
              
        public List<PedidoViewModel> Pedidos { get; set; }
        public List<CadeteViewModel> Cadetes { get; set; }
        public bool? ConfirmacionDeEntrega { get; set; }
        public bool? ConfirmacionDeEliminacion { get; set; }

        public PedidoIndexViewModel(List<PedidoViewModel> Pedidos, List<CadeteViewModel> Cadetes)
        {
            this.Pedidos = Pedidos;
            this.Cadetes = Cadetes;
        }

        public PedidoIndexViewModel()
        {
        }
    }
}
