using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.ViewModels
{
    public class PedidoCrearViewModel
    {
        [Required(ErrorMessage = "Ingrese los detalles del pedido en Observación")]
        [StringLength(200)]
        [Display(Name = "Observación")]
        public string Obs { get; set; }

        public ClienteViewModel Cliente { get; set; }

        public PedidoCrearViewModel()
        {

        }
    }
}