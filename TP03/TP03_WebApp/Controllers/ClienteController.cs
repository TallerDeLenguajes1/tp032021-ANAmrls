using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using TP03_WebApp.Models.DB;
using TP03_WebApp.Models.ViewModels;

namespace TP03_WebApp.Controllers
{   
    public class ClienteController : Controller
    {
        private const string SessionKeyID = "ID";
        private const string SessionKeyNivelDeAcceso = "Nivel";

        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteDB _repoClientes;
        private readonly IUsuarioDB _repoUsuarios;
        private readonly IMapper _mapper;

        public ClienteController(
            ILogger<ClienteController> logger,
            IClienteDB repoClientes,
            IUsuarioDB repoUsuarios,
            IMapper mapper)
        {
            _logger = logger;
            _repoClientes = repoClientes;
            _repoUsuarios = repoUsuarios;
            _mapper = mapper;
        }

        // GET: ClienteController
        public ActionResult Index()
        {            
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 1)
                {
                    int idCliente = HttpContext.Session.GetInt32(SessionKeyID).Value;
                    ClienteIndexViewModel clienteVM = _mapper.Map<ClienteIndexViewModel>(_repoClientes.GetClienteByID((int)idCliente));
                    clienteVM.HistorialDePedidos = _mapper.Map<List<PedidoViewModel>>(_repoClientes.GetPedidos((int)idCliente));
                    return View(clienteVM);
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

        // GET: ClienteController/Create
        public ActionResult CreateCliente(int idNuevoCliente)
        {
            try
            {
                ClienteCreateViewModel nuevoClienteVM = new()
                {
                    Id = idNuevoCliente
                };

                return View(nuevoClienteVM);
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

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCliente(ClienteCreateViewModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cliente clienteNuevo = _mapper.Map<Cliente>(cliente);
                    _repoClientes.CreateCliente(clienteNuevo);

                    HttpContext.Session.SetInt32(SessionKeyID, clienteNuevo.Id);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(cliente);
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

        // GET: ClienteController/Edit/5
        public ActionResult Edit()
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 1)
                {
                    int idCliente = HttpContext.Session.GetInt32(SessionKeyID).Value;
                    var clienteVM = _mapper.Map<ClienteEditViewModel>(_repoClientes.GetClienteByID(idCliente));
                    return View(clienteVM);
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

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteEditViewModel clienteEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cliente cliente = _mapper.Map<Cliente>(clienteEdit);
                    _repoClientes.EditCliente(cliente);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(clienteEdit);
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

        // GET: ClienteController/Delete/5
        public ActionResult Delete()
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 1)
                {
                    ClienteDeleteViewModel clienteAEliminar = _mapper.Map<ClienteDeleteViewModel>(_repoClientes.GetClienteByID(HttpContext.Session.GetInt32(SessionKeyID).Value));
                    return View(clienteAEliminar);
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

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idCliente)
        {
            try
            {
                _repoClientes.DeleteCliente(idCliente);
                _repoUsuarios.DeleteUsuario(idCliente);
                HttpContext.Session.Remove(SessionKeyID);
                HttpContext.Session.Remove(SessionKeyNivelDeAcceso);
                HttpContext.Session.Clear();
                return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
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
