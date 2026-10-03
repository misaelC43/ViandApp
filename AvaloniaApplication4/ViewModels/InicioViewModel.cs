using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaApplication4.ViewModels
{
    public partial class InicioViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        // Guardamos UNA sola instancia de MenuViewModel acá,
        // para reutilizarla siempre (no crear una nueva cada vez)
        private readonly MenuViewModel _menuViewModel;

        [ObservableProperty]
        private string _nombreUsuario = "admin";

        // Ya no es un campo fijo: calculamos el valor cada vez que se pide,
        // leyendo directamente la cantidad real de pedidos en MainViewModel
        public int PedidosPendientes => _mainViewModel.Pedidos.Count;

        

        // Constructor: recibe el MainViewModel y lo guarda
        public InicioViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _menuViewModel = new MenuViewModel(_mainViewModel); // se crea UNA vez acá
        }

        [RelayCommand]
        private void IrAMenu()
        {
            // Reutilizamos la instancia única de MenuViewModel creada en el constructor
            _mainViewModel.PantallaActual = _menuViewModel;
        }

        [RelayCommand]
        private void IrAPedidos()
        {
            // Le pasamos la MISMA lista de viandas que usa Menú
            _mainViewModel.PantallaActual = new PedidosViewModel(_mainViewModel,_menuViewModel.Viandas);
        }

        [RelayCommand]
        private void IrATurnos()
        {
            _mainViewModel.PantallaActual = new TurnosViewModel(_mainViewModel);
        }

    }
}
