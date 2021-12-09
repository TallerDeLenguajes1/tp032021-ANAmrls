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
        private const string SessionKeyID = "ID";
        private const string SessionKeyNivelDeAcceso = "Nivel";

        private readonly ILogger<CadeteController> _logger;
        private readonly DBTemp _DB;
        private readonly ICadeteDB _repoCadetes;
        private readonly IUsuarioDB _repoUsuarios;
        private readonly IMapper _mapper;

        public CadeteController(
            ILogger<CadeteController> logger,
            DBTemp DB,
            ICadeteDB repoCadetes,
            IUsuarioDB repoUsuarios,
            IMapper mapper)
        {
            _logger = logger;
            _DB = DB;
            _repoCadetes = repoCadetes;
            _repoUsuarios = repoUsuarios;
            _mapper = mapper;
        }               

        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 2)
                {
                    int idCadete = HttpContext.Session.GetInt32(SessionKeyID).Value;
                    CadeteIndexViewModel cadeteVM = _mapper.Map<CadeteIndexViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                    return View(cadeteVM);
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

                return NotFound();
            }
        }

        public IActionResult ListadoCadetes()
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    var cadetesVM = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                    CadeteListadoViewModel listadoViewModel = new()
                    {
                        Cadetes = cadetesVM
                    };
                    return View(listadoViewModel);                    
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

                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult AltaCadete(int idNuevoCadete)
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    CadeteAltaViewModel nuevoCadeteVM = new()
                    {
                        Id = idNuevoCadete
                    };

                    return View(nuevoCadeteVM);
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

                return NotFound();
            }
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

                    return RedirectToAction(nameof(ListadoCadetes));
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

                return NotFound();
            }
        }

        public IActionResult ConfirmarEliminacion(int idCadete)
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    var cadeteAEliminar = _mapper.Map<CadeteEliminarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                    return View(cadeteAEliminar); 
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

                return NotFound();
            }
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCadete(int idCadete)
        {
            try
            {
                CadeteListadoViewModel listadoViewModel = new()
                {
                    ConfirmacionDeEliminacion = _repoCadetes.DeleteCadete(idCadete)
                };

                if (listadoViewModel.ConfirmacionDeEliminacion == true)
                {
                    _repoUsuarios.DeleteUsuario(idCadete);
                }
                
                listadoViewModel.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());
                return View(nameof(ListadoCadetes), listadoViewModel);
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

        [HttpGet]
        public IActionResult ModificarCadete(int idCadete)
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) >= 2)
                {
                    var cadeteVM = _mapper.Map<CadeteModificarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                    return View(cadeteVM);
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

                return NotFound();
            }
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
                    bool confirmacion = _repoCadetes.ModificarCadete(cadeteModificado);

                    if (HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                    {
                        CadeteListadoViewModel listadoViewModel = new()
                        {
                            ConfirmacionDeModificacion = confirmacion
                        };

                        listadoViewModel.Cadetes = _mapper.Map<List<CadeteViewModel>>(_repoCadetes.GetAll());

                        return RedirectToAction(nameof(ListadoCadetes));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
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

                return NotFound();
            }
        }

        public IActionResult PagarJornalCheck(int idCadete)
        {
            try
            {
                if (HttpContext.Session.GetInt32(SessionKeyID) != null
                    && HttpContext.Session.GetInt32(SessionKeyNivelDeAcceso) == 3)
                {
                    var cadeteAPagar = _mapper.Map<CadetePagarViewModel>(_repoCadetes.GetCadeteByID(idCadete));
                    return View(cadeteAPagar); 
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

                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PagarJornal(int idCadete)
        {
            try
            {
                _repoCadetes.PagarACadete(idCadete);
                return RedirectToAction(nameof(ListadoCadetes));
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
