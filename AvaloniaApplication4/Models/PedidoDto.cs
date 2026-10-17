using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApplication4.Models;

public class PedidoDto
{
    public string Resumen => string.Join(", ", Items.Select(i => $"{i.Cantidad} x {i.ViandaNombre}"));
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public DateTime FechaPedido { get; set; }
    public DateTime HoraEntrega { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<DetallePedidoDto> Items { get; set; } = new();
}

public class DetallePedidoDto
{
    public int ViandaId { get; set; }
    public string ViandaNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}