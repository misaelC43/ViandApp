using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication4.ViewModels
{
    public partial class PedidosViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        // Arranca vacía: las viandas disponibles vienen de la API
        public ObservableCollection<Vianda> ViandasDisponibles { get; } = new();

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private string _mensajeExito = string.Empty;

        [ObservableProperty]
        private bool _cargando;

        public PedidosViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Pide a la API solo las viandas activas (el filtro LINQ está en el servidor)
        public async Task CargarAsync()
        {
            MensajeError = string.Empty;
            MensajeExito = string.Empty;
            Cargando = true;
            try
            {
                var viandas = await _mainViewModel.Api.GetViandasAsync(soloActivas: true);

                ViandasDisponibles.Clear();
                foreach (var v in viandas)
                    ViandasDisponibles.Add(v);
            }
            catch (HttpRequestException)
            {
                MensajeError = "No se pudo conectar con la API. ¿Está corriendo?";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar las viandas: {ex.Message}";
            }
            finally
            {
                Cargando = false;
            }
        }

        // El comando generado se sigue llamando PedirCommand (se le saca el "Async")
        [RelayCommand]
        private async Task PedirAsync(Vianda vianda)
        {
            MensajeError = string.Empty;
            MensajeExito = string.Empty;
            try
            {
                var clienteId = await _mainViewModel.Api.ObtenerClienteIdAsync();
                var pedido = await _mainViewModel.Api.CrearPedidoAsync(clienteId, vianda.Id, 1);

                MensajeExito = $"Pedido #{pedido.Id} confirmado: {vianda.Nombre}. Entrega estimada: {pedido.HoraEntrega:HH:mm}";
            }
            catch (HttpRequestException)
            {
                MensajeError = "No se pudo conectar con la API. ¿Está corriendo?";
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            _mainViewModel.MostrarInicio();
        }
    }
}