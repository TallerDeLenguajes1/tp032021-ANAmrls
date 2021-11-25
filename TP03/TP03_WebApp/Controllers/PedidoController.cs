using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using TP03_WebApp.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using TP03_WebApp.Models.ViewModels;
using AutoMapper;
using System.Diagnostics;

namespace TP03_WebApp.Controllers
{
    public class PedidoController : Controller
    {        
        private readonly ILogger<PedidoController> _logger;
        private readonly IPedidoDB _repoPedidos;
        private readonly ICadeteDB _repoCadetes;
        private readonly IClienteDB _repoClientes;
        private readonly IMapper _mapper;

        public PedidoController(
            ILogger<PedidoController> logger,
            IPedidoDB repoPedidos,
            ICadeteDB repoCadetes,
            IClienteDB repoClientes,
            IMapper mapper)
        {
            _logger = logger;
            _repoPedidos = repoPedidos;
            _repoCadetes = repoCadetes;
            _repoClientes = repoClientes;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ID") != null)
            {                
                var cadetesVM = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                var pedidosVM = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll());                
                var pedidoIndexVM = new PedidoIndexViewModel(pedidosVM, cadetesVM);           
                return View(pedidoIndexVM);                
            } 
            else 
            {
                return RedirectToAction("Index", "Usuario");
            }            
        }

        [HttpGet]
        [ViewLayout("_ClienteLayout")]
        public IActionResult CrearPedido(int idCliente)
        {
            PedidoCrearViewModel pedidoVM = new()
            {
                Cliente = _mapper.Map<ClienteViewModel>(_repoClientes.GetClienteByID(idCliente))
            };
            return View(pedidoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearPedido(PedidoCrearViewModel pedido)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pedido nuevoPedido = _mapper.Map<Pedido>(pedido);                    
                    _repoPedidos.GuardarPedidoEnBD(nuevoPedido);
                    return RedirectToAction(nameof(ClienteController.Index), nameof(Cliente));
                }
                else
                {
                    return View(pedido);
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

                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult AsignarPedidoACadete(int idPedido, int idCadete)
        {
            _repoPedidos.AsignarCadete(idPedido, idCadete);

            return RedirectToAction(nameof(Index));
        }        

        public IActionResult EliminarPedido(int idPedido)
        {            
            var pedidoIndexVM = new PedidoIndexViewModel();

            try
            {
                Pedido pedido = _repoPedidos.GetPedidoByID(idPedido);
                if (pedido.Estado == EstadoPedido.Entregado)
                {
                    pedidoIndexVM.ConfirmacionDeEliminacion = false;                    
                }
                else
                {
                    pedidoIndexVM.ConfirmacionDeEliminacion = _repoPedidos.DeletePedido(idPedido);
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

            pedidoIndexVM.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
            pedidoIndexVM.Pedidos = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll());

            return View("Index", pedidoIndexVM);
        }

        public IActionResult EntregarPedido(int idPedido, string checkBox)
        {            
            var pedidoIndexVM = new PedidoIndexViewModel();
            try
            {
                int idCadete = _repoPedidos.GetIDCadeteAsignado(idPedido);

                if (checkBox == "on" && idCadete > 0)
                {
                    _repoPedidos.CambiarEstadoPedido(idPedido);
                }
                else
                {
                    pedidoIndexVM.ConfirmacionDeEntrega = false;
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

            pedidoIndexVM.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
            pedidoIndexVM.Pedidos = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll());
            return View("Index", pedidoIndexVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
