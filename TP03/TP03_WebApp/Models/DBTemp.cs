using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models
{
    public class DBTemp
    {
        public Cadeteria Cadeteria { get; set; }

        public DBTemp()
        {
            Cadeteria = new Cadeteria();
        }
    }
}
