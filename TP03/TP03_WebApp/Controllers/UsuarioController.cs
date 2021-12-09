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
        private const string SessionKeyID = "ID";
        private const string SessionKeyNivelDeAcceso = "Nivel";

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
                        HttpContext.Session.SetInt32(SessionKeyID, usuarioID);
                        HttpContext.Session.SetInt32(SessionKeyNivelDeAcceso, usuarioNivel);

                        switch (usuarioNivel)
                        {
                            case 1: return RedirectToAction(nameof(ClienteController.Index), nameof(Cliente));
                            case 2: return RedirectToAction(nameof(CadeteController.Index), nameof(Cadete));
                            case 3: return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                else
                {
                    return View(nameof(Index), usuario);
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            try
            {
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

        // GET
        public ActionResult CreateUsuario()
        {
            return View(new UsuarioCreateViewModel());
        }

        // POST
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
                    int usuarioID = _repoUsuario.GetUsuarioID(nuevoUsuario.Nombre, nuevoUsuario.Password);

                    if (HttpContext.Session.GetInt32(SessionKeyID) != null
                        && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                    {
                        _repoUsuario.SetUsuarioNivel(usuarioID);
                        return RedirectToAction(nameof(CadeteController.AltaCadete),
                                                nameof(Cadete),
                                                new { idNuevoCadete = usuarioID });
                    }
                    else
                    {
                        return RedirectToAction(nameof(ClienteController.CreateCliente),
                                                nameof(Cliente),
                                                new { idNuevoCliente = usuarioID });
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