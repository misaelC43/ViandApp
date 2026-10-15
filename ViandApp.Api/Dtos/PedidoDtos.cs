using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Dtos;

// Una línea del pedido que manda el cliente: qué vianda y cuántas
public class ItemPedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Vianda inválida")]
    public int ViandaId { get; set; }

    [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
    public int Cantidad { get; set; }
}

// Lo que se manda para CREAR un pedido (no incluye hora ni precios: los calcula la API)
public class CrearPedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Tenés que indicar el cliente")]
    public int ClienteId { get; set; }

    [Required, MinLength(1, ErrorMessage = "El pedido debe tener al menos una vianda")]
    public List<ItemPedidoDto> Items { get; set; } = new();
}

public class CambiarEstadoDto
{
    [Required(ErrorMessage = "El estado es obligatorio")]
    public string Estado { get; set; } = string.Empty;
}

// Lo que DEVUELVE la API
public class DetallePedidoDto
{
    public int ViandaId { get; set; }
    public string ViandaNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class PedidoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public DateTime FechaPedido { get; set; }
    public DateTime HoraEntrega { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<DetallePedidoDto> Items { get; set; } = new();
}