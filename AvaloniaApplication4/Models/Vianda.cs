using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication4.Models;

public partial class Vianda : ObservableObject
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }

    // Observables porque el checkbox del menú las modifica
    [ObservableProperty]
    private string _nombre = string.Empty;

    [ObservableProperty]
    private bool _activa = true;
}