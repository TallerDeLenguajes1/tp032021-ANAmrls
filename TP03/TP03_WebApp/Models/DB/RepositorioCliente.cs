using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public class RepositorioCliente : IClienteDB
    {
        private readonly ILogger _logger;
        private readonly string connectionString;

        public RepositorioCliente(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;            
        }

        public bool DeleteCliente(int idCliente)
        {
            bool clienteEliminado = false;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Cliente " +
                                      "SET clienteActivo = 0 " +
                                      "WHERE clienteID = @clienteID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@clienteID", idCliente);
                        connection.Open();
                        clienteEliminado = command.ExecuteNonQuery() > 0;
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

            return clienteEliminado;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * " +
                                        "FROM Clientes " +
                                        "WHERE clienteActivo = 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Cliente cliente = new Cliente();
                                cliente.Id = Convert.ToInt32(dataReader["clienteID"]);
                                cliente.Nombre = dataReader["clienteNombre"].ToString();
                                cliente.Apellido = dataReader["clienteApellido"].ToString();
                                cliente.Direccion = dataReader["clienteDireccion"].ToString();
                                cliente.Telefono = Convert.ToInt64(dataReader["clienteTelefono"]);
                                clientes.Add(cliente);
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

            return clientes;
        }

        public void CreateCliente(Cliente cliente)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "INSERT INTO Clientes " +
                                        "(" +
                                            "clienteNombre, " +
                                            "clienteApellido, " +
                                            "clienteTelefono, " +
                                            "clienteDireccion" +
                                        ")" +
                                        "VALUES (" +
                                            "@clienteNombre, " +
                                            "@clienteApellido, " +
                                            "@clienteTelefono, " +
                                            "@clienteDireccion" +
                                        ");";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@clienteNombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@clienteApellido", cliente.Apellido);
                        command.Parameters.AddWithValue("@clienteTelefono", cliente.Telefono);
                        command.Parameters.AddWithValue("@clienteDireccion", cliente.Direccion);

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

        public int GetLastClienteID()
        {
            int idBuscado = 0;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT clienteID FROM Clientes WHERE clienteActivo = 1 ORDER BY clienteID DESC LIMIT 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            idBuscado = Convert.ToInt32(dataReader["clienteID"]);
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

            return idBuscado;
        }

        public Cliente GetClienteByID(int idCliente)
        {
            Cliente clienteBuscado = new Cliente();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM Clientes " +
                                        "WHERE clienteID = @clienteID AND clienteActivo = 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@clienteID", idCliente);
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            clienteBuscado.Id = Convert.ToInt32(dataReader["clienteID"]);
                            clienteBuscado.Nombre = dataReader["clienteNombre"].ToString();
                            clienteBuscado.Apellido = dataReader["clienteApellido"].ToString();
                            clienteBuscado.Direccion = dataReader["clienteDireccion"].ToString();
                            clienteBuscado.Telefono = Convert.ToInt64(dataReader["clienteTelefono"]);
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

            return clienteBuscado;
        }

        public List<Pedido> GetPedidos(int idCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = @"SELECT pedidoID,
                                            pedidoObs,
                                            pedidoEstado
                                        FROM Pedidos
                                            INNER JOIN
                                            Clientes USING (
                                            clienteID
                                            )
                                        WHERE clienteID = @clienteID
                                            AND clienteActivo = 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@clienteID", idCliente);
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
    }
}
