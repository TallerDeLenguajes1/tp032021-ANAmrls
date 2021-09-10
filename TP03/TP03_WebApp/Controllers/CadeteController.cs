using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Controllers
{
    public class CadeteController : Controller
    {
        static int id = 0;
        private readonly ILogger<CadeteController> _logger;
        private readonly List<Cadete> cadetes;

        public CadeteController(ILogger<CadeteController> logger, List<Cadete> Cadetes)
        {
            _logger = logger;
            cadetes = Cadetes;
        }
        public IActionResult Index()
        {
            Cadete nuevoCadete = new(++id, "nombre", "direccion", 212121);
            cadetes.Add(nuevoCadete);

            return View(cadetes);
        }
    }
}
