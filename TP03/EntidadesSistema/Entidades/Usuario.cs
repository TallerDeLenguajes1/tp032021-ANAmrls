using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP03_WebApp.Entidades
{
    public class Usuario
    {
        private string nombre;
        private string email;
        private string password;
        
        public Usuario(string nombre, string password, string email)
        {
            Nombre = nombre;
            Password = password;
            Email = email;
        }

        public Usuario()
        {

        }
               
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
    }
}
