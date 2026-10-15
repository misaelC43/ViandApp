using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Models;

public class DetallePedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ViandaId { get; set; }
    public Vianda? Vianda { get; set; }

    [Range(1, 100)]
    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }   // se guarda el precio al momento de pedir
}