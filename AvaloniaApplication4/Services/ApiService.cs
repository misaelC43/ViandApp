using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.Services;

public class ApiService
{
    private readonly HttpClient _http = new() { BaseAddress = new Uri("https://localhost:7248/") };

    // ---------- Viandas ----------

    // GET api/viandas  (o api/viandas?soloActivas=true)
    public async Task<List<Vianda>> GetViandasAsync(bool soloActivas = false)
    {
        var url = soloActivas ? "api/viandas?soloActivas=true" : "api/viandas";
        return await _http.GetFromJsonAsync<List<Vianda>>(url) ?? new List<Vianda>();
    }

    // PUT api/viandas/{id}
    public async Task ActualizarViandaAsync(Vianda vianda)
    {
        var respuesta = await _http.PutAsJsonAsync($"api/viandas/{vianda.Id}", vianda);
        respuesta.EnsureSuccessStatusCode();
    }

    // ---------- Clientes ----------

    // Por ahora la app no tiene pantalla de clientes: usamos el primero de la base,
    // y si no hay ninguno, creamos uno genérico.
    public async Task<int> ObtenerClienteIdAsync()
    {
        var clientes = await _http.GetFromJsonAsync<List<ClienteApi>>("api/clientes") ?? new();
        if (clientes.Count > 0)
            return clientes[0].Id;

        var respuesta = await _http.PostAsJsonAsync("api/clientes", new { nombre = "Cliente mostrador", telefono = "" });
        respuesta.EnsureSuccessStatusCode();
        var creado = await respuesta.Content.ReadFromJsonAsync<ClienteApi>();
        return creado!.Id;
    }

    // ---------- Pedidos ----------

    // POST api/pedidos  (la API calcula la hora de entrega y los precios)


    public async Task<PedidoDto> CrearPedidoAsync(int clienteId, int viandaId, int cantidad)
    {
        var cuerpo = new { clienteId, items = new[] { new { viandaId, cantidad } } };
        var respuesta = await _http.PostAsJsonAsync("api/pedidos", cuerpo);

        if (!respuesta.IsSuccessStatusCode)
        {
            // Si la API explicó el motivo ({ "error": "..." }), lo mostramos
            var mensaje = $"La API respondió con error {(int)respuesta.StatusCode}.";
            try
            {
                var error = await respuesta.Content.ReadFromJsonAsync<ErrorApi>();
                if (!string.IsNullOrEmpty(error?.Error))
                    mensaje = error.Error;
            }
            catch (JsonException) { }

            throw new InvalidOperationException(mensaje);
        }

        return (await respuesta.Content.ReadFromJsonAsync<PedidoDto>())!;
    }
    // GET api/pedidos  (o api/pedidos?estado=Pendiente)
    public async Task<List<PedidoDto>> GetPedidosAsync(string? estado = null)
    {
        var url = string.IsNullOrEmpty(estado) ? "api/pedidos" : $"api/pedidos?estado={estado}";
        return await _http.GetFromJsonAsync<List<PedidoDto>>(url) ?? new List<PedidoDto>();
    }

    // Clases chicas, solo para leer las respuestas de la API
    private class ClienteApi
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    private class ErrorApi
    {
        public string? Error { get; set; }
    }

    // GET api/categorias
    public async Task<List<Categoria>> GetCategoriasAsync()
    {
        return await _http.GetFromJsonAsync<List<Categoria>>("api/categorias") ?? new List<Categoria>();
    }

    // POST api/viandas
    public async Task CrearViandaAsync(string nombre, string descripcion, decimal precio, int categoriaId)
    {
        var cuerpo = new { nombre, descripcion, precio, activa = true, categoriaId };
        var respuesta = await _http.PostAsJsonAsync("api/viandas", cuerpo);

        if (!respuesta.IsSuccessStatusCode)
            throw new InvalidOperationException(await LeerErrorAsync(respuesta));
    }

    // Saca el mensaje de error de la respuesta de la API (sirve para { "error": ... } y para errores de validación)
    private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta)
    {
        var mensaje = $"La API respondió con error {(int)respuesta.StatusCode}.";
        try
        {
            using var doc = JsonDocument.Parse(await respuesta.Content.ReadAsStringAsync());
            var raiz = doc.RootElement;

            if (raiz.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.String)
                return error.GetString() ?? mensaje;

            if (raiz.TryGetProperty("errors", out var errores) && errores.ValueKind == JsonValueKind.Object)
            {
                foreach (var campo in errores.EnumerateObject())
                {
                    if (campo.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var texto in campo.Value.EnumerateArray())
                            return texto.GetString() ?? mensaje;
                    }
                }
            }
        }
        catch (JsonException) { }

        return mensaje;
    }
}