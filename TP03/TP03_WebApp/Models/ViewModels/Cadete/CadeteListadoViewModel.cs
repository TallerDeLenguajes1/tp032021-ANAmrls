using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp.Models.ViewModels
{
    public class CadeteListadoViewModel
    {
        public List<CadeteViewModel> Cadetes { get; set; }
        public bool? ConfirmacionDeEliminacion { get; set; }
        public bool? ConfirmacionDeModificacion { get; set; }

        public CadeteListadoViewModel()
        {

        }
    }
}