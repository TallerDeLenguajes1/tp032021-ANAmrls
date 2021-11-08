using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using NLog;

namespace TP03_WebApp
{
    public class RepositorioCadete : ICadeteDB
    {
        private readonly ILogger _logger;
        private readonly string connectionString;

        public RepositorioCadete(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }               

        public List<Cadete> GetAll() {
            List<Cadete> cadetes = new List<Cadete>();
            using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
            {                
                string SQLQuery = "SELECT"
                                  + " * "
                                  + "FROM "
                                  + "Cadetes "
                                  + "INNER JOIN "
                                  + "Pedidos USING(cadeteID) "
                                  + "INNER JOIN "
                                  + "Clientes USING(clienteID) "
                                  + "GROUP BY pedidoID "
                                  + "ORDER BY cadeteID;";
                
                using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                {
                    conexion.Open();

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
                    conexion.Close();
                }                
            }

            return cadetes;
        }

        public void GuardarCadeteEnBD(Cadete cadete)
        {
            try
            {
                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {                    
                    string SQLQuery = "INSERT INTO Cadetes " +
                                        "(" +
                                            "cadeteNombre, " +
                                            "cadeteApellido, " +
                                            "cadeteTelefono, " +
                                            "cadeteDireccion" +
                                        ")" +
                                        "VALUES (" +
                                            "@cadeteNombre, " +
                                            "@cadeteApellido, " +
                                            "@cadeteTelefono, " +
                                            "@cadeteDireccion" +
                                        ")";

                    SQLiteCommand command= new SQLiteCommand(SQLQuery, connection);
                    command.Parameters.AddWithValue("@cadeteNombre", cadete.Nombre);
                    command.Parameters.AddWithValue("@cadeteApellido", cadete.Apellido);
                    command.Parameters.AddWithValue("@cadeteTelefono", cadete.Telefono);
                    command.Parameters.AddWithValue("@cadeteDireccion", cadete.Direccion);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        public void GuardarListaCadetesEnBD()
        {
            throw new NotImplementedException();
        }

        public bool ModificarCadete(Cadete cadete)
        {
            throw new NotImplementedException();
        }

        public void PagarACadete(int idCadete)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCadete(int id)
        {
            throw new NotImplementedException();
        }
    }
}