using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models.DB
{
    public interface IUsuarioDB
    {
        void CreateUsuario(Usuario usuario);
        int GetUsuarioID(string nombre, string pass);
        int GetUsuarioNivel(int idUsuario);
        void SetUsuarioNivel(int idUsuario);
        void DeleteUsuario(int idUsuario);
    }
}