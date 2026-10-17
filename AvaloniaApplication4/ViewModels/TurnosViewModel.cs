using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication4.ViewModels
{
    public partial class TurnosViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<PedidoDto> Pedidos { get; } = new();

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private bool _cargando;

        public TurnosViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // La API devuelve los pedidos ordenados por hora de entrega
        public async Task CargarAsync()
        {
            MensajeError = string.Empty;
            Cargando = true;
            try
            {
                var pedidos = await _mainViewModel.Api.GetPedidosAsync();

                Pedidos.Clear();
                foreach (var p in pedidos)
                    Pedidos.Add(p);
            }
            catch (HttpRequestException)
            {
                MensajeError = "No se pudo conectar con la API. ¿Está corriendo?";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar los turnos: {ex.Message}";
            }
            finally
            {
                Cargando = false;
            }
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            _mainViewModel.MostrarInicio();
        }
    }
}