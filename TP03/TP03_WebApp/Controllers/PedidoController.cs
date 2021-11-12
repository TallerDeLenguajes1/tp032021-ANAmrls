using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using TP03_WebApp.Models.DB;
using TP03_WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace TP03_WebApp.Controllers
{
    public class PedidoController : Controller
    {        
        private readonly ILogger<PedidoController> _logger;
        private readonly IPedidoDB _repoPedidos;
        private readonly ICadeteDB _repoCadetes;
        private readonly IClienteDB _repoClientes;

        public PedidoController(ILogger<PedidoController> logger, IPedidoDB repoPedidos, ICadeteDB repoCadetes, IClienteDB repoClientes)
        {
            _logger = logger;
            _repoPedidos = repoPedidos;
            _repoCadetes = repoCadetes;
            _repoClientes = repoClientes;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                PedidoIndexViewModel pedidoIndexViewModel = new(_repoPedidos.GetAll(), _repoCadetes.GetAll());
                return View(pedidoIndexViewModel);                
            } 
            else 
            {
                return RedirectToAction("Index", "Usuario");
            }            
        }

        public IActionResult RealizarPedido()
        {
            return View();
        }

        public IActionResult CrearPedido(string obs, string nombre, string apellido,
                                         string direccion, string tel)
        {
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    Cliente nuevoCliente = new(nombre, apellido, direccion, telefono);
                    _repoClientes.CreateCliente(nuevoCliente);
                    nuevoCliente.Id = _repoClientes.GetLastClienteID();
                    Pedido nuevoPedido = new(obs);
                    nuevoPedido.Cliente = nuevoCliente;
                    _repoPedidos.GuardarPedidoEnBD(nuevoPedido);
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

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AsignarPedidoACadete(int idPedido, int idCadete)
        {
            _repoPedidos.AsignarCadete(idPedido, idCadete);

            return RedirectToAction(nameof(Index));
        }

        //private void QuitarPedidoDeCadete(int idPedido)
        //{
        //    _DB.Cadeteria.Cadetes.ForEach(cadete =>
        //    {
        //        cadete.PedidosDelDia.RemoveAll(y => y.Nro == idPedido);
        //    });
        //}

        public IActionResult EliminarPedido(int idPedido)
        {
            ViewBag.Eliminacion = _repoPedidos.DeletePedido(idPedido);
            //TODO agregar lo de viewbag al view model
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EntregarPedido(int idPedido, string estado)
        {
            if (estado == "on")
            {
                ViewBag.Entrega = _repoPedidos.CambiarEstadoPedido(idPedido);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
