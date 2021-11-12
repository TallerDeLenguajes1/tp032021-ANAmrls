using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.ViewModels
{
    public class PedidoIndexViewModel
    {
        private List<Pedido> pedidos;
        private List<Cadete> cadetes;

        public PedidoIndexViewModel(List<Pedido> pedidos, List<Cadete> cadetes)
        {
            this.pedidos = pedidos;
            this.cadetes = cadetes;
        }

        public List<Pedido> Pedidos { get => pedidos; set => pedidos = value; }
        public List<Cadete> Cadetes { get => cadetes; set => cadetes = value; }
    }
}
