using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace TP03_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKeyID = "ID";
        private const string SessionKeyNivelDeAcceso = "Nivel";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {            
            if (HttpContext.Session.GetInt32(SessionKeyID) != null
                && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
            {
                return View();
            } 
            else 
            {
                return RedirectToAction(nameof(UsuarioController.Index), nameof(Usuario));
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = "error" });
        }
    }
}
