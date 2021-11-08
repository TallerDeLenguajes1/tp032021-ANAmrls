using System.Collections.Generic;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models
{
    public interface ICadeteDB
    {
        List<Cadete> GetAll();
        bool DeleteCadete(int id);
        void GuardarCadeteEnBD(Cadete cadete);
        void GuardarListaCadetesEnBD();
        bool ModificarCadete(Cadete cadete);
        void PagarACadete(int idCadete);
    }
}