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
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteDB _repoClientes;
        private readonly IMapper _mapper;

        public ClienteController(
            ILogger<ClienteController> logger,
            IClienteDB repoClientes,
            IMapper mapper)
        {
            _logger = logger;
            _repoClientes = repoClientes;
            _mapper = mapper;
        }

        // GET: ClienteController
        public ActionResult Index()
        {            
            try
            {
                if (HttpContext.Session.GetInt32("ID") != null)
                {
                    int? idCliente = HttpContext.Session.GetInt32("ID");
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

                return RedirectToAction(nameof(Error));
            }            
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult CreateCliente()
        {
            return View(new ClienteCreateViewModel());
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
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ClienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
