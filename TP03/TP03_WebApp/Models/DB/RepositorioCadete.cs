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
                                            "cadeteID, " +
                                            "cadeteNombre, " +
                                            "cadeteApellido, " +
                                            "cadeteTelefono, " +
                                            "cadeteDireccion" +
                                        ")" +
                                        "VALUES (" +
                                            "@cadeteID, " +
                                            "@cadeteNombre, " +
                                            "@cadeteApellido, " +
                                            "@cadeteTelefono, " +
                                            "@cadeteDireccion" +
                                        ");";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", cadete.Id);
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
                            cadeteBuscado.Direccion = dataReader["cadeteDireccion"].ToString();
                            cadeteBuscado.Telefono = Convert.ToInt64(dataReader["cadeteTelefono"]);
                            cadeteBuscado.PedidosDelDia = GetPedidosDeCadete(cadeteBuscado.Id);
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
            bool modificacionRealizada = false;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Cadetes " +
                                        "SET cadeteNombre = @nombre, " +
                                            "cadeteApellido = @apellido, " +
                                            "cadeteDireccion = @direccion, " +
                                            "cadeteTelefono = @telefono " +
                                        "WHERE cadeteID = @id;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", cadete.Nombre);
                        command.Parameters.AddWithValue("@apellido", cadete.Apellido);
                        command.Parameters.AddWithValue("@direccion", cadete.Direccion);
                        command.Parameters.AddWithValue("@telefono", cadete.Telefono);
                        command.Parameters.AddWithValue("@id", cadete.Id);

                        connection.Open();
                        modificacionRealizada = command.ExecuteNonQuery() > 0;
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

            return modificacionRealizada;
        }

        public void PagarACadete(int idCadete)
        {
            Cadete cadete = GetCadeteByID(idCadete);
            IEnumerable<int> pedidosAPagar = cadete.PedidosDelDia.Where(x => x.Estado == EstadoPedido.Entregado).Select(x => x.Nro);

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandText = "UPDATE Pedidos " +
                                                    "SET pedidoEstado = 'Pagado' " +
                                                    "WHERE pedidoID = @pedidoID;";

                            SQLiteParameter parameter = command.CreateParameter();
                            parameter.ParameterName = "@pedidoID";
                            command.Parameters.Add(parameter);
                                                        
                            foreach (int item in pedidosAPagar)
                            {
                                parameter.Value = item;
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
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
                                                "AND clienteActivo = 1 " +
                                                "AND cadeteID = @cadeteID " +
                                                "AND pedidoEstado <> 'Pagado';";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cadeteID", idCadete);
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
    }
}