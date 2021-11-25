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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using TP03_WebApp.Models.ViewModels;
using AutoMapper;

namespace TP03_WebApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioDB _repoUsuario;
        private readonly IMapper _mapper;

        public UsuarioController(
            ILogger<UsuarioController> logger,
            IUsuarioDB repoUsuario,
            IMapper mapper)
        {
            _logger = logger;
            _repoUsuario = repoUsuario;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
           
            return View(new UsuarioIndexViewModel());
        }
                                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UsuarioIndexViewModel usuario)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario logInUsuario = _mapper.Map<Usuario>(usuario);
                    int usuarioID = _repoUsuario.GetUsuarioID(logInUsuario.Nombre, logInUsuario.Password);
                    int usuarioNivel = _repoUsuario.GetUsuarioNivel(usuarioID);

                    if (usuarioID != 0)
                    {
                        HttpContext.Session.SetInt32("ID", usuarioID);
                        HttpContext.Session.SetInt32("nivel", usuarioNivel);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Error));
                    }
                }
                else
                {
                    return View(usuario);
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

        // GET: ClienteController/Create
        public ActionResult CreateUsuario()
        {
            return View(new UsuarioCreateViewModel());
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsuario(UsuarioCreateViewModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario nuevoUsuario = _mapper.Map<Usuario>(usuario);
                    _repoUsuario.CreateUsuario(nuevoUsuario);
                }
                else
                {
                    return View(usuario);
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

            return RedirectToAction(nameof(ClienteController.CreateCliente), nameof(Cliente));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}