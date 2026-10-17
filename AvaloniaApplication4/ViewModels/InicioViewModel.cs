using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

        [ObservableProperty]
        private string _proximoTurno = "Sin Turnos Pendientes";


        // Ya no es un campo fijo: calculamos el valor cada vez que se pide,
        // leyendo directamente la cantidad real de pedidos en MainViewModel
        [ObservableProperty]
        private int _PedidosPendientes ;

        

        // Constructor: recibe el MainViewModel y lo guarda
        public InicioViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _menuViewModel = new MenuViewModel(_mainViewModel); // se crea UNA vez acá
        }
        // Cuenta los pedidos pendientes y busca el próximo turno, ambos desde la API
        public async Task CargarResumenAsync()
        {
            try
            {
                var pendientes = await _mainViewModel.Api.GetPedidosAsync("Pendiente");
                PedidosPendientes = pendientes.Count;

                var proximo = pendientes.OrderBy(p => p.HoraEntrega).FirstOrDefault();
                ProximoTurno = proximo is null
                    ? "Sin turnos pendientes"
                    : $"{proximo.HoraEntrega:dd/MM HH:mm} hs";
            }
            catch (Exception)
            {
                // Si la API no responde, el resumen queda como estaba
            }
        }


        [RelayCommand]
        private async Task IrAMenuAsync()
        {
            _mainViewModel.PantallaActual = _menuViewModel;
            await _menuViewModel.CargarAsync();   // trae las viandas de la API cada vez que entrás
        }

        [RelayCommand]
        private async Task IrAPedidosAsync()
        {
            var pedidosVm = new PedidosViewModel(_mainViewModel);
            _mainViewModel.PantallaActual = pedidosVm;
            await pedidosVm.CargarAsync();   // trae las viandas activas de la API
        }

        [RelayCommand]
        private async Task IrATurnosAsync()
        {
            var turnosVm = new TurnosViewModel(_mainViewModel);
            _mainViewModel.PantallaActual = turnosVm;
            await turnosVm.CargarAsync();
        }

    }
}
