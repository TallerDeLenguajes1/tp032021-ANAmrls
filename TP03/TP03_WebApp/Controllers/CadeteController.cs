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
using AutoMapper;
using TP03_WebApp.Models.ViewModels;

namespace TP03_WebApp.Controllers
{
    public class CadeteController : Controller
    {           
        private readonly ILogger<CadeteController> _logger;
        private readonly DBTemp _DB;
        private readonly ICadeteDB _repoCadetes;
        private readonly IMapper _mapper;

        public CadeteController(
            ILogger<CadeteController> logger,
            DBTemp DB,
            ICadeteDB repoCadetes,
            IMapper mapper)
        {
            _logger = logger;
            _DB = DB;
            _repoCadetes = repoCadetes;
            _mapper = mapper;
        }               

        public IActionResult Index()
        {            
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                if (HttpContext.Session.GetInt32("nivel") == 3)
                {
                    //var cadetesVM = _mapper.Map<List<CadeteIndexViewModel>>(_repoCadetes.GetAll());
                    
                    return View(_repoCadetes.GetAll());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }                
            } 
            else 
            {
                return RedirectToAction("Index", "Usuario");
            }
            
        }

        public IActionResult AltaCadetes()
        {
            return View();
        }

        public IActionResult DarAltaCadete(string nombre, string apellido, string direccion, string tel)
        {
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    Cadete nuevoCadete = new(nombre, apellido, direccion, telefono);                    
                    _repoCadetes.GuardarCadeteEnBD(nuevoCadete);
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

        public IActionResult EliminacionCheck(int idCadete)
        {
            try
            {
                return View(_repoCadetes.GetCadeteByID(idCadete));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }        

        public IActionResult EliminarCadete(int idCadete)
        {
            ViewBag.Eliminacion = _repoCadetes.DeleteCadete(idCadete);
            return View("Index", _repoCadetes.GetAll());
        }

        public IActionResult ModificarCadeteForm(int idCadete)
        {
            return View(_repoCadetes.GetCadeteByID(idCadete));
        }

        public IActionResult ModificarCadete(string nombre, string apellido, string direccion, string tel, int id)
        {
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    Cadete cadete = new(id, nombre,apellido, direccion, telefono);
                    ViewBag.Modificacion = _repoCadetes.ModificarCadete(cadete);
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

            return View("Index", _repoCadetes.GetAll());
        }

        public IActionResult PagarJornalCheck(int idCadete)
        {
            return View(_repoCadetes.GetCadeteByID(idCadete));
        }

        public IActionResult PagarJornal(int idCadete)
        {
            _repoCadetes.PagarACadete(idCadete);
            return View("Index", _repoCadetes.GetAll());
        }
    }
}
