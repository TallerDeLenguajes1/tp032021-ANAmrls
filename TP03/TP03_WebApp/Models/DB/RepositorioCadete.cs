using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using TP03_WebApp.Entidades;
using NLog;

namespace TP03_WebApp.Models.DB
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

        public List<Cadete> GetAll()
        {
            List<Cadete> cadetes = new List<Cadete>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * " +
                                        "FROM Cadetes " +
                                        "WHERE cadeteActivo = 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Cadete cadete = new Cadete();
                                cadete.Id = Convert.ToInt32(dataReader["cadeteID"]);
                                cadete.Nombre = dataReader["cadeteNombre"].ToString();
                                cadete.Apellido = dataReader["cadeteApellido"].ToString();
                                cadete.Direccion = dataReader["cadeteDireccion"].ToString();
                                cadete.Telefono = Convert.ToInt64(dataReader["cadeteTelefono"]);
                                cadete.PedidosDelDia = GetPedidosDeCadete(cadete.Id);
                                cadetes.Add(cadete);
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

            return cadetes;
        }

        public void GuardarCadeteEnBD(Cadete cadete)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "INSERT INTO Cadetes " +
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
                                        ");";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteNombre", cadete.Nombre);
                        command.Parameters.AddWithValue("@cadeteApellido", cadete.Apellido);
                        command.Parameters.AddWithValue("@cadeteTelefono", cadete.Telefono);
                        command.Parameters.AddWithValue("@cadeteDireccion", cadete.Direccion);

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

        public Cadete GetCadeteByID(int id)
        {
            Cadete cadeteBuscado = new Cadete();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM Cadetes " +
                                        "WHERE cadeteID = @cadeteID AND cadeteActivo = 1;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", id);
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            cadeteBuscado.Id = Convert.ToInt32(dataReader["cadeteID"]);
                            cadeteBuscado.Nombre = dataReader["cadeteNombre"].ToString();
                            cadeteBuscado.Apellido = dataReader["cadeteApellido"].ToString();
                            cadeteBuscado.Telefono = Convert.ToInt64(dataReader["cadeteTelefono"]);
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

            return cadeteBuscado;
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
            bool cadeteEliminado = false;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Cadetes " +
                                      "SET cadeteActivo = 0 " +
                                      "WHERE cadeteID = @cadeteID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", id);
                        connection.Open();
                        cadeteEliminado = command.ExecuteNonQuery() > 0;
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

            return cadeteEliminado;
        }

        private List<Pedido> GetPedidosDeCadete(int idCadete)
        {
            List<Pedido> pedidos = new List<Pedido>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT pedidoID, " +
                                                "pedidoObs, " +
                                                "pedidoEstado, " +
                                                "clienteID, " +
                                                "clienteNombre, " +
                                                "clienteApellido, " +
                                                "clienteDireccion, " +
                                                "clienteTelefono " +
                                      "FROM Cadetes " +
                                                "INNER JOIN Pedidos USING(cadeteID) " +
                                                "INNER JOIN Clientes USING(clienteID) " +
                                      "WHERE cadeteActivo = 1 " +
                                                "AND cadeteID = @cadeteID;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", idCadete);
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

                                Pedido pedido = new Pedido();
                                pedido.Nro = Convert.ToInt32(dataReader["pedidoID"]);
                                pedido.Obs = dataReader["pedidoObs"].ToString();
                                pedido.Cliente = cliente;

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