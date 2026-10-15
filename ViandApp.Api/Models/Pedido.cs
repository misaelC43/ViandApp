namespace ViandApp.Api.Models;

public class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public DateTime FechaPedido { get; set; }
    public DateTime HoraEntrega { get; set; }   // FechaPedido + 45 min, lo calcula la API

    public string Estado { get; set; } = "Pendiente";   // Pendiente, EnPreparacion, Listo, Entregado

    // Un pedido tiene varios renglones (una vianda con su cantidad cada uno)
    public List<DetallePedido> Detalles { get; set; } = new();
}