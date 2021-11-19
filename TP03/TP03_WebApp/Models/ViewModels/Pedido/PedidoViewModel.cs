using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int Nro { get; set; }
        public string Obs { get; set; }
        public Cliente Cliente { get; set; }
        public EstadoPedido Estado { get; set; }

        public PedidoViewModel()
        {

        }
    }
}
