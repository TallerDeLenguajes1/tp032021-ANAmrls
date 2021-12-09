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
        private const string SessionKeyID = "ID";
        private const string SessionKeyNivelDeAcceso = "Nivel";

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
            if (HttpContext.Session.GetInt32(SessionKeyID) != null
                && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
            {                
                var cadetesVM = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                var pedidosVM = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll());                
                var pedidoIndexVM = new PedidoIndexViewModel(pedidosVM, cadetesVM);           
                return View(pedidoIndexVM);                
            } 
            else 
            {
                return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
            }            
        }

        [HttpGet]
        public IActionResult CrearPedido()
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 1)
                {
                    int idCliente = HttpContext.Session.GetInt32(SessionKeyID).Value;

                    PedidoCrearViewModel pedidoVM = new()
                    {
                        Cliente = _mapper.Map<ClienteViewModel>(_repoClientes.GetClienteByID(idCliente))
                    };

                    return View(pedidoVM);
                }
                else
                {
                    return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
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

                return NotFound();
            }
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

                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AsignarPedidoACadete(int idPedido, int idCadete)
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    _repoPedidos.AsignarCadete(idPedido, idCadete);
                    return RedirectToAction(nameof(Index)); 
                }
                else
                {
                    return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
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

                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPedido(int idPedido)
        {            
            var pedidoIndexVM = new PedidoIndexViewModel();

            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
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

                    pedidoIndexVM.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                    pedidoIndexVM.Pedidos = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll());

                    return View(nameof(Index), pedidoIndexVM);
                }
                else
                {
                    return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
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

                return NotFound();
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EntregarPedido(int idPedido, string checkBox)
        {
            try
            {
                bool confirmacion = true;
                int idCadete = _repoPedidos.GetIDCadeteAsignado(idPedido);

                if (checkBox == "on" && idCadete > 0)
                {
                    _repoPedidos.CambiarEstadoPedido(idPedido);
                }
                else
                {
                    confirmacion = false;
                }

                if (HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    PedidoIndexViewModel pedidoIndexVM = new()
                    {
                        ConfirmacionDeEntrega = confirmacion,
                        Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll()),
                        Pedidos = _mapper.Map<List<PedidoViewModel>>(_repoPedidos.GetAll())
                    };
                    return View(nameof(Index), pedidoIndexVM);
                }
                else
                {
                    return RedirectToAction(nameof(CadeteController.Index), nameof(Cadete));
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

                return NotFound();
            }            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = "error" });
        }
    }
}
