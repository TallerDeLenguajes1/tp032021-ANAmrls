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
                var SQLQuery = "SELECT * FROM Cadetes;";
                using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Cadete cadete = new Cadete();
                            cadete.Id = Convert.ToInt32(dataReader["cadeteID"]);
                            cadete.Nombre = dataReader["cadeteNombre"].ToString();
                            cadete.Direccion = dataReader["cadeteDireccion"].ToString();
                            //cadete.Telefono = (long)dataReader["cadeteTelefono"];
                            cadetes.Add(cadete);
                        }
                    }
                }
                conexion.Close();
            }

            return cadetes;
        }
    }
}