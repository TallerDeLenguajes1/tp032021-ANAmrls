using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP03_WebApp.Entidades
{
    public class Cliente
    {
        private int id;
        private string nombre;
        private string apellido;
        private string direccion;
        private long telefono;

        public Cliente(int id, string nombre, string apellido, string direccion, long tel)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Direccion = direccion;
            Telefono = tel;
        }

        [JsonConstructor]
        public Cliente()
        {

        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public long Telefono { get => telefono; set => telefono = value; }
        public string Apellido { get => apellido; set => apellido = value; }
    }
}
