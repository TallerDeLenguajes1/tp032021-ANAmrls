using System.IO;
using System.Text.Json;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models
{
    public class DBTemp
    {
        public Cadeteria Cadeteria { get; set; }

        public DBTemp()
        {
            Cadeteria = new Cadeteria();

            LeerBD();
        }

        public void GuardarCadeteEnBD(Cadete cadete)
        {
            string rutaArchivo = @"ListadoCadetes.Json";

            using (FileStream cadetesArchivo = new FileStream(rutaArchivo, FileMode.Append))
            {
                using (StreamWriter strWriter = new StreamWriter(cadetesArchivo))
                {
                    string strJason = JsonSerializer.Serialize(cadete);
                    strWriter.WriteLine(strJason);
                }
            }
        }

        public void LeerBD()
        {
            string rutaArchivo = @"ListadoCadetes.Json";

            if (File.Exists(rutaArchivo))
            {
                using (FileStream cadetesArchivo = new FileStream(rutaArchivo, FileMode.Open))
                {
                    using(StreamReader strReader = new StreamReader(cadetesArchivo))
                    {
                        while (!strReader.EndOfStream)
                        {
                            string cadete = strReader.ReadLine();
                            Cadeteria.Cadetes.Add(JsonSerializer.Deserialize<Cadete>(cadete));
                        }
                    }
                }
            }
        }

        public void GuardarListaCadetesEnBD()
        {
            string rutaArchivo = @"ListadoCadetes.Json";

            using (FileStream cadetesArchivo = new FileStream(rutaArchivo, FileMode.Create))
            {
                using (StreamWriter strWriter = new StreamWriter(cadetesArchivo))
                {
                    foreach (Cadete item in Cadeteria.Cadetes)
                    {
                        string strJason = JsonSerializer.Serialize(item);
                        strWriter.WriteLine(strJason);
                    }
                    
                }
            }
        }
    }
}
