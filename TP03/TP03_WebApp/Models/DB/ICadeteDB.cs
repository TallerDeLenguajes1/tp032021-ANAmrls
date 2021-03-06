using System.Collections.Generic;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public interface ICadeteDB
    {
        List<Cadete> GetAll();
        bool DeleteCadete(int id);
        void GuardarCadeteEnBD(Cadete cadete);
        Cadete GetCadeteByID(int id);
        bool ModificarCadete(Cadete cadete);
        void PagarACadete(int idCadete);
    }
}