using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class ClienteDeleteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        ClienteDeleteViewModel()
        {

        }
    }
}
