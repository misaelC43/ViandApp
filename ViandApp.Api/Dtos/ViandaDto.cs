using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Dtos;

public class ViandaDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a cero")]
    public decimal Precio { get; set; }

    public bool Activa { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "Tenés que elegir una categoría")]
    public int CategoriaId { get; set; }

    // Solo de lectura: la API lo completa con el nombre de la categoría
    public string? CategoriaNombre { get; set; }
}