using System.Text.Json.Serialization;

namespace TP03_WebApp.Entidades
{
    public enum EstadoPedido
    {
        Pendiente,
        Entregado,
        Pagado
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

        public Pedido(string obs)
        {
            Cliente = new Cliente();
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
