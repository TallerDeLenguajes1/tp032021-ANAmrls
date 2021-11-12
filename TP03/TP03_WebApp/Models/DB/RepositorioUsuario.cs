using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public class RepositorioUsuario
    {
        private readonly ILogger _logger;
        private readonly string connectionString;

        public RepositorioUsuario(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        public void CreateUsuario(Usuario usuario)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "INSERT INTO Cadetes " +
                                        "(" +
                                            "usuarioNombre, " +
                                            "usuarioPassword" +
                                        ")" +
                                        "VALUES (" +
                                            "@usuarioNombre, " +
                                            "@usuarioPassword" +
                                        ");";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@usuarioNombre", usuario.Nombre);

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

        public int GetUsuarioID (string nombre, string pass)
        {
            int usuarioID = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    string sqlQuery = "SELECT usuarioID FROM Usuarios " +
                                        "WHERE usuarioNombre = @usuarioNombre " +
                                        "AND usuarioPassword = @usuarioPass;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@usuarioNombre", nombre);
                        command.Parameters.AddWithValue("@suarioPass", pass);
                        connection.Open();

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {
                            dataReader.Read();
                            usuarioID = Convert.ToInt32(dataReader["usuarioID"]);
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

            return usuarioID;
        }
    }
}
