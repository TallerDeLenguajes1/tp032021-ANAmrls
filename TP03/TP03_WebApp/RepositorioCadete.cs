using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using TP03_WebApp.Entidades;

namespace TP03_WebApp
{
    public class RepositorioCadete
    {
        private readonly string connectionString;

        public RepositorioCadete(string connectionString) {
            this.connectionString = connectionString;
        }

        public List<Cadete> GetAll() {
            List<Cadete> cadetes = new List<Cadete>();
            using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
            {
                conexion.Open();
                var SQLQuery = "SELECT * FROM Cadetes INNER JOIN Pedidos USING(cadeteID) INNER JOIN Clientes USING(clienteID) GROUP BY pedidoID ORDER BY cadeteID;";
                using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        Cadete cadete = new Cadete();
                        cadete.Id = 0;
                      
                        while (dataReader.Read())
                        {
                            if(Convert.ToInt32(dataReader["cadeteID"]) != cadete.Id)
                            {
                                cadete = new Cadete();
                                cadete.PedidosDelDia = new List<Pedido>();                                
                            }
                            
                            cadete.Id = Convert.ToInt32(dataReader["cadeteID"]);
                            cadete.Nombre = dataReader["cadeteNombre"].ToString();
                            cadete.Apellido = dataReader["cadeteApellido"].ToString();
                            cadete.Direccion = dataReader["cadeteDireccion"].ToString();
                            cadete.Telefono = Convert.ToInt64(dataReader["cadeteTelefono"]);
                                                                        
                            Cliente cliente = new Cliente();
                            cliente.Id = Convert.ToInt32(dataReader["clienteID"]);
                            cliente.Nombre = dataReader["clienteNombre"].ToString();
                            cliente.Apellido = dataReader["clienteApellido"].ToString();
                            cliente.Direccion = dataReader["clienteDireccion"].ToString();
                            cliente.Telefono = Convert.ToInt64(dataReader["clienteTelefono"]);

                            Pedido pedido = new Pedido();
                            pedido.Nro = Convert.ToInt32(dataReader["pedidoID"]);
                            pedido.Obs = dataReader["pedidoObs"].ToString();
                            pedido.Cliente = cliente;

                            if (Enum.TryParse(dataReader["pedidoEstado"].ToString(), out EstadoPedido estadoPedido))
                            {
                                pedido.Estado = estadoPedido;
                            } else
                            {
                                pedido.Estado = EstadoPedido.Pendiente;
                            }                        
                                                        
                            cadete.PedidosDelDia.Add(pedido);

                            if(cadetes.Where(ca => ca.Id == cadete.Id).Count() == 0)cadetes.Add(cadete);
                        }
                    }
                }
                conexion.Close();
            }

            return cadetes;
        }
    }
}