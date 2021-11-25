using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public class RepositorioPedido : IPedidoDB
    {
        private readonly ILogger _logger;
        private readonly string connectionString;

        public RepositorioPedido(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        public List<Pedido> GetAll()
        {
            List<Pedido> pedidos = new List<Pedido>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM Pedidos " +
                                            "INNER JOIN Clientes USING(clienteID);";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Pedido pedido = new Pedido();
                                pedido.Nro = Convert.ToInt32(dataReader["pedidoID"]);
                                pedido.Obs = dataReader["pedidoObs"].ToString();

                                if (Enum.TryParse(dataReader["pedidoEstado"].ToString(), out EstadoPedido estadoPedido))
                                {
                                    pedido.Estado = estadoPedido;
                                }
                                else
                                {
                                    pedido.Estado = EstadoPedido.Pendiente;
                                }

                                pedido.Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dataReader["clienteID"]),
                                    Nombre = dataReader["clienteNombre"].ToString(),
                                    Apellido = dataReader["clienteApellido"].ToString(),
                                    Direccion = dataReader["clienteDireccion"].ToString(),
                                    Telefono = Convert.ToInt64(dataReader["clienteTelefono"])
                                };

                                pedidos.Add(pedido);
                            }
                        }
                        
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }

            return pedidos;
        }

        public bool CambiarEstadoPedido(int idPedido)
        {
            bool estadoModificado = false;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Pedidos " +
                                      "SET pedidoEstado = 'Entregado' " +
                                      "WHERE pedidoID = @pedidoID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@pedidoID", idPedido);
                        connection.Open();
                        estadoModificado = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }

            return estadoModificado;
        }

        public bool DeletePedido(int idPedido)
        {
            bool pedidoEliminado = false;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "DELETE FROM Pedidos " +
                                      "WHERE pedidoID = @pedidoID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@pedidoID", idPedido);
                        connection.Open();
                        pedidoEliminado = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }

            return pedidoEliminado;
        }        

        public void GuardarPedidoEnBD(Pedido pedido)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "INSERT INTO Pedidos " +
                                        "(" +
                                            "pedidoObs, " +
                                            "clienteID " +
                                        ")" +
                                        "VALUES (" +
                                            "@pedidoObs, " +
                                            "@clienteID" +
                                        ");";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@pedidoObs", pedido.Obs);
                        command.Parameters.AddWithValue("@clienteID", pedido.Cliente.Id);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        public void AsignarCadete(int idPedido, int idCadete)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Pedidos " +
                                      "SET cadeteID = @cadeteID " +
                                      "WHERE pedidoID = @pedidoID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", idCadete);
                        command.Parameters.AddWithValue("@pedidoID", idPedido);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }
                
        public Pedido GetPedidoByID(int idPedido)
        {
            Pedido pedidoBuscado = new Pedido();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM Pedidos " +
                                        "WHERE pedidoID = @pedidoID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@pedidoID", idPedido);
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            pedidoBuscado.Nro = Convert.ToInt32(dataReader["pedidoID"]);
                            pedidoBuscado.Obs = dataReader["pedidoObs"].ToString();

                            if (Enum.TryParse(dataReader["pedidoEstado"].ToString(), out EstadoPedido estadoPedido))
                            {
                                pedidoBuscado.Estado = estadoPedido;
                            }
                            else
                            {
                                pedidoBuscado.Estado = EstadoPedido.Pendiente;
                            }

                            pedidoBuscado.Cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dataReader["clienteID"]),
                                Nombre = dataReader["clienteNombre"].ToString(),
                                Apellido = dataReader["clienteApellido"].ToString(),
                                Direccion = dataReader["clienteDireccion"].ToString(),
                                Telefono = Convert.ToInt64(dataReader["clienteTelefono"])
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }

            return pedidoBuscado;
        }

        public int GetIDCadeteAsignado(int idPedido)
        {
            int idCadete = 0;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT cadeteID FROM Pedidos " +
                                            "WHERE pedidoID = @idPedido;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idPedido", idPedido);
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            idCadete = Convert.ToInt32(dataReader["cadeteID"]);
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }

            return idCadete;
        }
    }
}
