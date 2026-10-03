using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace AvaloniaApplication4.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {

        // Referencia al ViewModel "padre", para poder cambiar de pantalla
        private readonly MainViewModel _mainViewModel;
        // Esto es un campo privado normal, como cualquier variable de clase.
        // Empieza con "_" por convención (indica que es privado).
        // Guarda el texto que el usuario escribe en el campo "Usuario".
        [ObservableProperty]
        private string _usuario = string.Empty;

        // Lo mismo, pero para la contraseña.
        [ObservableProperty]
        private string _contrasena = string.Empty;

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private string _mensajeExito = string.Empty;

        // Usuario y contraseña fijos, "hardcodeados"
        private const string UsuarioValido = "admin";
        private const string ContrasenaValida = "1234";

        // Constructor: recibe el MainViewModel y lo guarda
        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        [RelayCommand]
        private void Ingresar()
        {
            if (Usuario == UsuarioValido && Contrasena == ContrasenaValida)
            {
                MensajeError = string.Empty;
                // Acá, más adelante, vamos a navegar a la pantalla de Inicio
                MensajeExito = "Bienvenido admin";
                // Le decimos al MainViewModel que cambie de pantalla a Inicio // actualizacion pasamos un mainviewmodel al constructor de inicioViewModel
                _mainViewModel.MostrarInicio();
            }
            else
            {
                MensajeExito = string.Empty;
                MensajeError = "Usuario o contraseña incorrectos";
            }
        }
    }
}
