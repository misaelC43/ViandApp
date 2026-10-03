using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MenuViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<Vianda> Viandas { get; } = new()
        {
            new Vianda { Nombre = "Vianda Clásica - Milanesa con puré" },
            new Vianda { Nombre = "Vianda Vegetariana - Tarta de verduras" },
            new Vianda { Nombre = "Vianda Sin TACC - Pollo con arroz" },
            new Vianda { Nombre = "Vianda Fitness - Ensalada con pollo grillado" }
        };

        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            // Volvemos a la instancia de Inicio ya gestionada por MainViewModel
            _mainViewModel.MostrarInicio();
        }

    }
}
