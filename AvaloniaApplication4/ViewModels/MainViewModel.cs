// Ya lo conocías: nos da acceso a [ObservableProperty]
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvaloniaApplication4.ViewModels;

// Mismo patrón que ya usás: partial + hereda de ViewModelBase
public partial class MainViewModel : ViewModelBase
{

    // Guardamos UNA sola instancia de InicioViewModel para toda la app
    private readonly InicioViewModel _inicioViewModel;


    // Esta es la propiedad MÁS IMPORTANTE de este ViewModel.
    // Su tipo es "ViewModelBase" (la clase base de la que heredan
    // TODOS tus ViewModels: LoginViewModel, InicioViewModel, etc.)
    // Esto significa que PantallaActual puede "apuntar" a CUALQUIERA
    // de tus ViewModels, sin importar cuál sea.
    //
    // Es literalmente la variable que dice "qué pantalla se está
    // mostrando ahora mismo".
    [ObservableProperty]
    public partial ViewModelBase PantallaActual { get; set; }

    // Lista central de pedidos, vive toda la sesión
    public ObservableCollection<Pedido> Pedidos { get; } = new();


    // El constructor se ejecuta UNA sola vez, apenas se crea
    // un MainViewModel (que pasa una sola vez, al arrancar la app).
    public MainViewModel()
    {
        // Apenas arranca la aplicación, le decimos:
        // "la pantalla actual es un LoginViewModel nuevo"
        // Esto hace que, al abrir la app, se vea el Login primero.
        _inicioViewModel = new InicioViewModel(this); // se crea una sola vez, acá
        PantallaActual = new LoginViewModel(this);
    }

    // Método público para que cualquier pantalla pueda volver a Inicio,
    // reutilizando siempre la MISMA instancia
    public void MostrarInicio()
    {
        PantallaActual = _inicioViewModel;
    }
}