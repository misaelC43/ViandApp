using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MenuViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        // Arranca vacía: las viandas ahora vienen de la API
        public ObservableCollection<Vianda> Viandas { get; } = new();

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private bool _cargando;

        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Pide las viandas a la API y llena la lista
        public async Task CargarAsync()
        {
            MensajeError = string.Empty;
            Cargando = true;
            try
            {
                var viandas = await _mainViewModel.Api.GetViandasAsync();

                foreach (var v in Viandas)
                    v.PropertyChanged -= OnViandaChanged;
                Viandas.Clear();

                foreach (var v in viandas)
                {
                    v.PropertyChanged += OnViandaChanged;   // avisame cuando cambie algo
                    Viandas.Add(v);
                }
            }
            catch (HttpRequestException)
            {
                MensajeError = "No se pudo conectar con la API. ¿Está corriendo?";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar el menú: {ex.Message}";
            }
            finally
            {
                Cargando = false;
            }
        }

        // Se dispara cuando se tilda o destilda un checkbox: guarda el cambio en la base
        private async void OnViandaChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Vianda.Activa) || sender is not Vianda vianda)
                return;

            try
            {
                await _mainViewModel.Api.ActualizarViandaAsync(vianda);
                MensajeError = string.Empty;
            }
            catch (Exception)
            {
                MensajeError = "No se pudo guardar el cambio. Se restauró el estado anterior.";

                // Deshacemos el cambio en pantalla (sin volver a disparar el guardado)
                vianda.PropertyChanged -= OnViandaChanged;
                vianda.Activa = !vianda.Activa;
                vianda.PropertyChanged += OnViandaChanged;
            }
        }

        [RelayCommand]
        private void VolverAInicio()
        {
            _mainViewModel.MostrarInicio();
        }
    }
}