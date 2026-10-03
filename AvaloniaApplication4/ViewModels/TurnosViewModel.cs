using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.ViewModels
{
    public partial class TurnosViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        // Exponemos directamente la misma lista de pedidos de MainViewModel
        public ObservableCollection<Pedido> Pedidos => _mainViewModel.Pedidos;

        public TurnosViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            _mainViewModel.MostrarInicio();
        }
    }
}