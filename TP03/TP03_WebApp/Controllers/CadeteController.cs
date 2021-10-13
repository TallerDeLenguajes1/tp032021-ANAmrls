﻿using Microsoft.AspNetCore.Mvc;
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
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    int id = _DB.GetAutonumericoDeCadete();
                    Cadete nuevoCadete = new(++id, nombre, apellido, direccion, telefono);
                    _DB.Cadeteria.Cadetes.Add(nuevoCadete);
                    _DB.GuardarCadeteEnBD(nuevoCadete);
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

            return View("Index", _DB.Cadeteria.Cadetes);
        }

        public IActionResult EliminacionCheck(int idCadete)
        {
            return BuscarCadeteEnLista(idCadete);
        }        

        public IActionResult EliminarCadete(int idCadete)
        {
            ViewBag.Eliminacion = _DB.DeleteCadete(idCadete);
            return View("Index", _DB.Cadeteria.Cadetes);
        }

        public IActionResult ModificarCadeteForm(int idCadete)
        {
            return BuscarCadeteEnLista(idCadete);
        }

        public IActionResult ModificarCadete(string nombre, string apellido, string direccion, string tel, int id)
        {
            try
            {
                if (long.TryParse(tel, out long telefono))
                {
                    Cadete cadete = new(id, nombre, apellido, direccion, telefono);
                    ViewBag.Modificacion = _DB.ModificarCadete(cadete);
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

        public IActionResult PagarJornalCheck(int idCadete)
        {
            return BuscarCadeteEnLista(idCadete);
        }

        public IActionResult PagarJornal(int idCadete)
        {
            _DB.PagarACadete(idCadete);
            return View("Index", _DB.Cadeteria.Cadetes);
        }
    }
}
