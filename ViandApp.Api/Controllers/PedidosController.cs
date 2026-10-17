using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViandApp.Api.Data;
using ViandApp.Api.Dtos;
using ViandApp.Api.Models;

namespace ViandApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private static readonly string[] EstadosValidos = { "Pendiente", "EnPreparacion", "Listo", "Entregado" };

    private readonly ViandAppContext _context;

    public PedidosController(ViandAppContext context)
    {
        _context = context;
    }

    // Trae los pedidos con su cliente y sus viandas ("Include" = hacer los JOIN)
    private IQueryable<Pedido> ConsultaCompleta() =>
        _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Detalles).ThenInclude(d => d.Vianda);

    // El total se calcula en memoria porque SQLite no suma decimales en SQL
    private static PedidoDto ToDto(Pedido p) => new()
    {
        Id = p.Id,
        ClienteId = p.ClienteId,
        ClienteNombre = p.Cliente?.Nombre ?? string.Empty,
        FechaPedido = p.FechaPedido,
        HoraEntrega = p.HoraEntrega,
        Estado = p.Estado,
        Total = p.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
        Items = p.Detalles.Select(d => new DetallePedidoDto
        {
            ViandaId = d.ViandaId,
            ViandaNombre = d.Vianda?.Nombre ?? string.Empty,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario,
            Subtotal = d.Cantidad * d.PrecioUnitario
        }).ToList()
    };

    // GET api/pedidos                    -> todos, ordenados por hora de entrega
    // GET api/pedidos?estado=Pendiente   -> filtrados por estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetAll([FromQuery] string? estado = null)
    {
        var query = ConsultaCompleta();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(p => p.Estado == estado);

        var pedidos = await query.OrderBy(p => p.HoraEntrega).ToListAsync();
        return Ok(pedidos.Select(ToDto));
    }

    // GET api/pedidos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDto>> GetById(int id)
    {
        var pedido = await ConsultaCompleta().FirstOrDefaultAsync(p => p.Id == id);

        if (pedido is null)
            return NotFound(new { error = $"No existe el pedido con id {id}." });

        return Ok(ToDto(pedido));
    }

    // POST api/pedidos
    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Create(CrearPedidoDto dto)
    {
        if (!await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
            return BadRequest(new { error = "El cliente indicado no existe." });

        // Buscamos todas las viandas pedidas de una sola vez
        var ids = dto.Items.Select(i => i.ViandaId).Distinct().ToList();
        var viandas = await _context.Viandas
            .Where(v => ids.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id);

        foreach (var item in dto.Items)
        {
            if (!viandas.TryGetValue(item.ViandaId, out var vianda))
                return BadRequest(new { error = $"La vianda con id {item.ViandaId} no existe." });

            if (!vianda.Activa)
                return BadRequest(new { error = $"La vianda '{vianda.Nombre}' no está disponible." });
        }

        var ahora = DateTime.Now;

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            FechaPedido = ahora,
            HoraEntrega = ahora.AddMinutes(45),   // la regla de los 45 minutos vive en el servidor
            Estado = "Pendiente",
            Detalles = dto.Items.Select(i => new DetallePedido
            {
                ViandaId = i.ViandaId,
                Cantidad = i.Cantidad,
                PrecioUnitario = viandas[i.ViandaId].Precio   // se guarda el precio de este momento
            }).ToList()
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var creado = await ConsultaCompleta().FirstAsync(p => p.Id == pedido.Id);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, ToDto(creado));
    }

    // PATCH api/pedidos/5/estado   body: { "estado": "EnPreparacion" }
    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoDto dto)
    {
        if (!EstadosValidos.Contains(dto.Estado))
            return BadRequest(new { error = $"Estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}." });

        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is null)
            return NotFound(new { error = $"No existe el pedido con id {id}." });

        pedido.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}