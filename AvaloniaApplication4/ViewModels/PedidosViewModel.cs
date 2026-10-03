using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AvaloniaApplication4.ViewModels
{
    public partial class PedidosViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<Vianda> ViandasDisponibles { get; }

       
        
        public PedidosViewModel(MainViewModel mainViewModel, ObservableCollection<Vianda> todasLasViandas)
        {
            _mainViewModel = mainViewModel;
            var activas = todasLasViandas.Where(v => v.Activa);
            ViandasDisponibles = new ObservableCollection<Vianda>(activas);
        }

        [RelayCommand]
        private void Pedir(Vianda vianda)
        {
            var ahora = DateTime.Now;

            var nuevoPedido = new Pedido
            {
                NombreVianda = vianda.Nombre,
                HoraPedido = ahora,
                HoraEntrega = ahora.AddMinutes(45)
            };

            _mainViewModel.Pedidos.Add(nuevoPedido);
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            _mainViewModel.MostrarInicio();
        }

    }
}
