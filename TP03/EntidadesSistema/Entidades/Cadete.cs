using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Entidades
{
    public class Cadete
    {
        private int id;
        private string nombre;
        private string apellido;
        private string direccion;
        private long telefono;
        private List<Pedido> pedidosDelDia;
        
        public Cadete(int id, string nombre, string apellido, string direccion, long telefono)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Direccion = direccion;
            Telefono = telefono;
            PedidosDelDia = new();
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public long Telefono { get => telefono; set => telefono = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public List<Pedido> PedidosDelDia { get => pedidosDelDia; set => pedidosDelDia = value; }
    }
}
