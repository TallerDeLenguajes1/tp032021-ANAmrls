using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03_ConsoleApp
{
    public enum EstadoPedido
    {
        pendiente,
        entregado
    }
    public class Pedido
    {
        private int nro;
        private string obs;
        private string cliente;
        private EstadoPedido estado;

        public int Nro { get => nro; set => nro = value; }
        public string Obs { get => obs; set => obs = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public EstadoPedido Estado { get => estado; set => estado = value; }
    }
}
