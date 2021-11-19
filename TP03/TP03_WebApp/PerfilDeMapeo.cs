﻿using AutoMapper;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models.ViewModels;

namespace TP03_WebApp
{
    public class PerfilDeMapeo : Profile
    {
        public PerfilDeMapeo()
        {
            //Maps para cadetes
            CreateMap<Cadete, CadeteViewModel>().ReverseMap();
            CreateMap<Cadete, CadeteIndexViewModel>().ReverseMap();
            CreateMap<Cadete, CadeteAltaViewModel>().ReverseMap();
            CreateMap<Cadete, CadeteModificarViewModel>().ReverseMap();

            //Maps para pedidos
            CreateMap<Pedido, PedidoViewModel>().ReverseMap();

            //Maps para clientes
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            CreateMap<Cliente, ClienteIndexViewModel>().ReverseMap();
        }
    }
}