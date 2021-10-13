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
                                         string direccion, string tel)
        {
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    int nro = _DB.GetAutonumericoDePedido();
                    Pedido nuevoPedido = new(++nro, obs, idCliente, nombre, apellido, direccion, telefono);
                    _DB.Cadeteria.Pedidos.Add(nuevoPedido);
                    _DB.GuardarPedidoEnBD(nuevoPedido);
                }
            }
            catch (Exception ex)
            {
                var mensaje = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    mensaje = mensaje + " Inner exception: " + ex.InnerException.Message;
                }

                mensaje = mensaje + " Stack trace: " + ex.StackTrace;
                _logger.LogError(mensaje);
            }

            return View("Index", _DB.Cadeteria);
        }

        public IActionResult AsignarPedidoACadete(int idPedido, int idCadete)
        {
            try
            {
                QuitarPedidoDeCadete(idPedido);

                if (idCadete != 0)
                {
                    Cadete cadete = _DB.Cadeteria.Cadetes.Where(a => a.Id == idCadete).First();
                    Pedido pedido = _DB.Cadeteria.Pedidos.Where(b => b.Nro == idPedido).First();
                    cadete.PedidosDelDia.Add(pedido);
                }

                _DB.GuardarListaCadetesEnBD();
            }
            catch (Exception ex)
            {
                var mensaje = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    mensaje = mensaje + " Inner exception: " + ex.InnerException.Message;
                }

                mensaje = mensaje + " Stack trace: " + ex.StackTrace;
                _logger.LogError(mensaje);
            }

            return View("Index", _DB.Cadeteria);
        }

        private void QuitarPedidoDeCadete(int idPedido)
        {
            _DB.Cadeteria.Cadetes.ForEach(cadete =>
            {
                cadete.PedidosDelDia.RemoveAll(y => y.Nro == idPedido);
            });
        }               

        public IActionResult EliminarPedido(int idPedido)
        {
            _DB.DeletePedido(idPedido);
            return View("Index", _DB.Cadeteria);
        }

        public IActionResult EntregarPedido(int idPedido, string estado)
        {
            if (estado == "on")
            {
                ViewBag.Entrega = _DB.CambiarEstadoPedido(idPedido);
            }

            return View("Index", _DB.Cadeteria);
        }
    }
}
