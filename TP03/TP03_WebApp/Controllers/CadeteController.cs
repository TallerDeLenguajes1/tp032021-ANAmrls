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
                    var cadetesVM = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                    CadeteIndexViewModel indexViewModel = new()
                    {
                        Cadetes = cadetesVM
                    };
                    return View(indexViewModel);
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

        [HttpGet]
        public IActionResult AltaCadete()
        {
            return View(new CadeteAltaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaCadete(CadeteAltaViewModel cadete)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cadete nuevoCadete = _mapper.Map<Cadete>(cadete);
                    _repoCadetes.GuardarCadeteEnBD(nuevoCadete);
                }
                else
                {
                    return View(cadete);
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

        public IActionResult ConfirmarEliminacion(int idCadete)
        {
            try
            {
                var cadeteAEliminar = _mapper.Map<CadeteEliminarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                return View(cadeteAEliminar);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }        

        [HttpPost]
        public IActionResult EliminarCadete(int idCadete)
        {
            CadeteIndexViewModel indexViewModel = new()
            {
                ConfirmacionDeEliminacion = _repoCadetes.DeleteCadete(idCadete)
            };
            indexViewModel.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());            
            return View("Index", indexViewModel);
        }

        [HttpGet]
        public IActionResult ModificarCadete(int idCadete)
        {
            var cadeteVM = _mapper.Map<CadeteModificarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
            return View(cadeteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificarCadete(CadeteModificarViewModel cadete)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cadete cadeteModificado = _mapper.Map<Cadete>(cadete);

                    CadeteIndexViewModel indexViewModel = new()
                    {
                        ConfirmacionDeModificacion = _repoCadetes.ModificarCadete(cadeteModificado)
                    };

                    indexViewModel.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());

                    return View("Index", indexViewModel);
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

        public IActionResult PagarJornalCheck(int idCadete)
        {
            try
            {
                var cadeteAPagar = _mapper.Map<CadetePagarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                return View(cadeteAPagar);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult PagarJornal(int idCadete)
        {
            try
            {
                _repoCadetes.PagarACadete(idCadete);
            }
            catch
            {
                
            }

            return RedirectToAction(nameof(Index));
        }

        //TODO try catches 
    }
}
