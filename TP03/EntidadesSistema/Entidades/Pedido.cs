using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP03_WebApp.Entidades
{
    public enum EstadoPedido
    {
        Pendiente,
        Entregado
    }
    public class Pedido
    {
        private int nro;
        private string obs;
        private Cliente cliente;
        private EstadoPedido estado;

        public Pedido(int nroPedido, string obs, int idCliente, string nombre, string apellido, string direccion,
                      long tel)
        {
            Cliente = new Cliente(idCliente, nombre, apellido, direccion, tel);
            Nro = nroPedido;
            Obs = obs;
            Estado = EstadoPedido.Pendiente;
        }

        [JsonConstructor]
        public Pedido()
        {

        }

        public int Nro { get => nro; set => nro = value; }
        public string Obs { get => obs; set => obs = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public EstadoPedido Estado { get => estado; set => estado = value; }
    }
}
