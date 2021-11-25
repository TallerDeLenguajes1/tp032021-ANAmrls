using System.Collections.Generic;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models
{
    public interface IPedidoDB
    {
        List<Pedido> GetAll();
        bool CambiarEstadoPedido(int idPedido);
        bool DeletePedido(int idPedido);
        void GuardarPedidoEnBD(Pedido pedido);
        void AsignarCadete(int idPedido, int idCadete);        
        Pedido GetPedidoByID(int idPedido);
        int GetIDCadeteAsignado(int idPedido);
    }
}