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
        private static int nro;
        private readonly ILogger<PedidoController> _logger;
        private readonly DBTemp _DB;

        public static int Nro { get => nro; set => nro = value; }

        public PedidoController(ILogger<PedidoController> logger, DBTemp dB)
        {
            _logger = logger;
            _DB = dB;
            Nro = _DB.AutonumericoPedido;
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
                                         string direccion, string tel)
        {
            if (long.TryParse(tel, out long telefono))
            {
                Pedido nuevoPedido = new(++Nro, obs, idCliente, nombre, apellido, direccion, telefono);
                _DB.Cadeteria.Pedidos.Add(nuevoPedido);
                _DB.GuardarPedidoEnBD(nuevoPedido);
            }

            return View("Index", _DB.Cadeteria);
        }

        public IActionResult AsignarPedidoACadete(int idPedido, int idCadete)
        {
            QuitarPedidoDeCadete(idPedido);

            if (idCadete != 0)
            {
                Cadete cadete = _DB.Cadeteria.Cadetes.Where(a => a.Id == idCadete).First();
                Pedido pedido = _DB.Cadeteria.Pedidos.Where(b => b.Nro == idPedido).First();
                cadete.PedidosDelDia.Add(pedido);
            }

            _DB.GuardarListaCadetesEnBD();

            return View("Index", _DB.Cadeteria);
        }

        private void QuitarPedidoDeCadete(int idPedido)
        {
            Pedido pedido = _DB.Cadeteria.Pedidos.Find(x => x.Nro == idPedido);

            _DB.Cadeteria.Cadetes.ForEach(cadete => cadete.PedidosDelDia.Remove(pedido));
            
        }               

        public IActionResult EliminarPedido(int idPedido)
        {
            if (_DB.Cadeteria.Pedidos.RemoveAll(x => x.Nro == idPedido) != 0)
            {
                _DB.GuardarListaPedidosEnBD();
            }

            return View("Index", _DB.Cadeteria);
        }
    }
}
