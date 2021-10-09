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
        private static int id;
        private readonly ILogger<CadeteController> _logger;
        private readonly DBTemp _DB;

        public static int Id { get => id; set => id = value; }

        public CadeteController(ILogger<CadeteController> logger, DBTemp DB)
        {
            _logger = logger;
            _DB = DB;
            Id = _DB.AutonumericoCadete;
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
            if (long.TryParse(tel, out long telefono))
            {
                Cadete nuevoCadete = new(++Id, nombre, apellido, direccion, telefono);
                _DB.Cadeteria.Cadetes.Add(nuevoCadete);
                _DB.GuardarCadeteEnBD(nuevoCadete);
            }

            return View("Index", _DB.Cadeteria.Cadetes);
        }

        public IActionResult EliminacionCheck(int idCadete)
        {
            return BuscarCadeteEnLista(idCadete);
        }        

        public IActionResult EliminarCadete(int idCadete)
        {
            if (_DB.Cadeteria.Cadetes.RemoveAll(cadete => cadete.Id == idCadete) != 0)
            {
                _DB.GuardarListaCadetesEnBD();
            }

            return View("Index", _DB.Cadeteria.Cadetes);
        }

        public IActionResult ModificarCadeteForm(int idCadete)
        {
            return BuscarCadeteEnLista(idCadete);
        }

        public IActionResult ModificarCadete(string nombre, string apellido, string direccion, string tel, int id)
        {
            int i = _DB.Cadeteria.Cadetes.FindIndex(x => x.Id == id);

            if (i >= 0)
            {
                if (long.TryParse(tel, out long telefono))
                {
                    _DB.Cadeteria.Cadetes[i].Nombre = nombre;
                    _DB.Cadeteria.Cadetes[i].Apellido = apellido;
                    _DB.Cadeteria.Cadetes[i].Direccion = direccion;
                    _DB.Cadeteria.Cadetes[i].Telefono = telefono;

                    _DB.GuardarListaCadetesEnBD();
                }
            }

            return View("Index", _DB.Cadeteria.Cadetes);
        }
        
        private IActionResult BuscarCadeteEnLista(int idCadete)
        {
            Cadete cadete = _DB.Cadeteria.Cadetes.Find(x => x.Id == idCadete);

            if (cadete != null)
            {
                return View(cadete);
            }
            else
            {
                return View("Index", _DB.Cadeteria.Cadetes);
            }
        }
    }
}
