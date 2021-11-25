﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class ClienteIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<PedidoViewModel> HistorialDePedidos { get; set; }

        public ClienteIndexViewModel()
        {

        }
    }
}
