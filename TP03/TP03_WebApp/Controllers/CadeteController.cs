using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;

namespace TP03_WebApp.Controllers
{
    public class CadeteController : Controller
    {
        static int id = 0;
        private readonly ILogger<CadeteController> _logger;
        private readonly DBTemp _DB;

        public CadeteController(ILogger<CadeteController> logger, DBTemp DB)
        {
            _logger = logger;
            _DB = DB;
        }
        public IActionResult Index()
        {
            return View(_DB.Cadeteria.Cadetes);
        }

        public IActionResult AltaCadetes()
        {
            return View();
        }

        public IActionResult DarAltaCadete(string nombre, string apellido, string direccion, string tel)
        {
            Cadete nuevoCadete = new(++id, nombre, apellido, direccion, Convert.ToInt64(tel));
            _DB.Cadeteria.Cadetes.Add(nuevoCadete);
            _DB.GuardarCadetesEnBD(nuevoCadete);

            return View("Index", _DB.Cadeteria.Cadetes);
        }

        public IActionResult EliminacionCheck(int idCadete)
        {
            foreach (Cadete item in _DB.Cadeteria.Cadetes)
            {
                if (idCadete == item.Id)
                {
                    return View(item);
                }                
            }

            return View();
        }

        public IActionResult EliminarCadete(int idCadete)
        {
            _DB.Cadeteria.Cadetes.RemoveAll(cadete => cadete.Id == idCadete);

            _DB.EliminarCadeteDeBD();

            return View("Index", _DB.Cadeteria.Cadetes);
        }
    }
}
