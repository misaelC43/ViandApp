using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Telefono { get; set; } = string.Empty;

    public List<Pedido> Pedidos { get; set; } = new();
}