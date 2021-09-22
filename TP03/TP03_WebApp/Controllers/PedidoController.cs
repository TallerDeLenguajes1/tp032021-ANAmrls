using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;

namespace TP03_WebApp.Controllers
{
    public class PedidoController : Controller
    {
        static int nro = 0;
        private readonly ILogger<PedidoController> _logger;
        private readonly DBTemp _DB;

        public PedidoController(ILogger<PedidoController> logger, DBTemp dB)
        {
            _logger = logger;
            _DB = dB;
        }

        public IActionResult Index()
        {
            return View(_DB.Cadeteria);
        }

        public IActionResult RealizarPedido()
        {
            return View();
        }

        public IActionResult CrearPedido(string obs, int idCliente, string nombre, string apellido,
                                         string direccion, long tel)
        {
            Pedido nuevoPedido = new(++nro, obs, idCliente, nombre, apellido, direccion, tel);
            _DB.Cadeteria.Pedidos.Add(nuevoPedido);

            return View("Index", _DB.Cadeteria);
        }

        public IActionResult AsignarPedido(int idPedido, int idCadete)
        {
            QuitarPedido(idPedido);

            if (idCadete != 0)
            {
                Cadete cadete = _DB.Cadeteria.Cadetes.Where(a => a.Id == idCadete).First();
                Pedido pedido = _DB.Cadeteria.Pedidos.Where(b => b.Nro == idPedido).First();
                cadete.PedidosDelDia.Add(pedido);
            }

            return View("Index", _DB.Cadeteria);
        }

        private void QuitarPedido(int idPedido)
        {            
            Pedido pedido = _DB.Cadeteria.Pedidos.Where(b => b.Nro == idPedido).First();

            _DB.Cadeteria.Cadetes.ForEach(cadete => cadete.PedidosDelDia.Remove(pedido));
            
        }
    }
}
