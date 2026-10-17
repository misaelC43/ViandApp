using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

        public ObservableCollection<Vianda> Viandas { get; } = new();
        public ObservableCollection<Categoria> Categorias { get; } = new();

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private string _mensajeExito = string.Empty;

        [ObservableProperty]
        private bool _cargando;

        // Campos del formulario de alta
        [ObservableProperty]
        private string _nuevoNombre = string.Empty;

        [ObservableProperty]
        private string _nuevaDescripcion = string.Empty;

        [ObservableProperty]
        private string _nuevoPrecio = string.Empty;

        [ObservableProperty]
        private Categoria? _categoriaSeleccionada;

        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Pide las viandas y las categorías a la API
        public async Task CargarAsync()
        {
            MensajeError = string.Empty;
            Cargando = true;
            try
            {
                var viandas = await _mainViewModel.Api.GetViandasAsync();
                var categorias = await _mainViewModel.Api.GetCategoriasAsync();

                foreach (var v in Viandas)
                    v.PropertyChanged -= OnViandaChanged;
                Viandas.Clear();

                foreach (var v in viandas)
                {
                    v.PropertyChanged += OnViandaChanged;
                    Viandas.Add(v);
                }

                Categorias.Clear();
                foreach (var c in categorias)
                    Categorias.Add(c);
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

        // Valida el formulario y manda el POST a la API
        [RelayCommand]
        private async Task AgregarViandaAsync()
        {
            MensajeError = string.Empty;
            MensajeExito = string.Empty;

            if (string.IsNullOrWhiteSpace(NuevoNombre))
            {
                MensajeError = "El nombre es obligatorio.";
                return;
            }

            // Aceptamos coma o punto como separador decimal
            if (!decimal.TryParse(NuevoPrecio.Replace(',', '.'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out var precio) || precio <= 0)
            {
                MensajeError = "El precio tiene que ser un número mayor a cero.";
                return;
            }

            if (CategoriaSeleccionada is null)
            {
                MensajeError = "Tenés que elegir una categoría.";
                return;
            }

            try
            {
                var nombre = NuevoNombre.Trim();
                await _mainViewModel.Api.CrearViandaAsync(
                    nombre, NuevaDescripcion.Trim(), precio, CategoriaSeleccionada.Id);

                // Limpiamos el formulario y recargamos la lista desde la base
                NuevoNombre = string.Empty;
                NuevaDescripcion = string.Empty;
                NuevoPrecio = string.Empty;
                CategoriaSeleccionada = null;

                await CargarAsync();
                MensajeExito = $"Vianda '{nombre}' agregada.";
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

        // Se dispara al tildar o destildar un checkbox: guarda el cambio en la base
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