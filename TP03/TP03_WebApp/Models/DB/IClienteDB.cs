using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public interface IClienteDB
    {
        List<Cliente> GetAll();
        void SaveCliente(Cliente cliente);
        bool DeleteCliente(int idCliente);
        
    }
}
