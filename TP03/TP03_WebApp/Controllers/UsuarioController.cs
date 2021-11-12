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

namespace TP03_WebApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly RepositorioUsuario _repoUsuario;

        public UsuarioController(ILogger<UsuarioController> logger, RepositorioUsuario repoUsuario)
        {
            _logger = logger;
            _repoUsuario = repoUsuario;
        }

        public IActionResult Index()
        {
           
            return View();
        }
                                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string username, string password)
        {            
            try
            {
                int usuarioID = _repoUsuario.GetUsuarioID(username, password);
                int usuarioNivel =  _repoUsuario.GetUsuarioNivel(usuarioID);

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
            catch
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}