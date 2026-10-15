using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViandApp.Api.Data;
using ViandApp.Api.Dtos;
using ViandApp.Api.Models;

namespace ViandApp.Api.Controllers;

[ApiController]                  // activa la validación automática (devuelve 400 si el DTO es inválido)
[Route("api/[controller]")]      // la ruta queda: api/viandas
public class ViandasController : ControllerBase
{
    private readonly ViandAppContext _context;

    // ASP.NET inyecta el DbContext que registramos en Program.cs
    public ViandasController(ViandAppContext context)
    {
        _context = context;
    }

    // Convierte una Vianda en ViandaDto. Es una "expresión" para que EF
    // la traduzca a SQL y traiga solo las columnas necesarias.
    private static readonly Expression<Func<Vianda, ViandaDto>> AsDto = v => new ViandaDto
    {
        Id = v.Id,
        Nombre = v.Nombre,
        Descripcion = v.Descripcion,
        Precio = v.Precio,
        Activa = v.Activa,
        CategoriaId = v.CategoriaId,
        CategoriaNombre = v.Categoria!.Nombre
    };

    // GET api/viandas            -> todas
    // GET api/viandas?soloActivas=true  -> solo las disponibles (consulta LINQ con filtro)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ViandaDto>>> GetAll([FromQuery] bool soloActivas = false)
    {
        var query = _context.Viandas.AsQueryable();

        if (soloActivas)
            query = query.Where(v => v.Activa);

        var viandas = await query.OrderBy(v => v.Nombre).Select(AsDto).ToListAsync();
        return Ok(viandas);
    }

    // GET api/viandas/3
    [HttpGet("{id}")]
    public async Task<ActionResult<ViandaDto>> GetById(int id)
    {
        var vianda = await _context.Viandas.Where(v => v.Id == id).Select(AsDto).FirstOrDefaultAsync();

        if (vianda is null)
            return NotFound(new { error = $"No existe la vianda con id {id}." });

        return Ok(vianda);
    }

    // POST api/viandas
    [HttpPost]
    public async Task<ActionResult<ViandaDto>> Create(ViandaDto dto)
    {
        if (!await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId))
            return BadRequest(new { error = "La categoría indicada no existe." });

        var vianda = new Vianda
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Activa = dto.Activa,
            CategoriaId = dto.CategoriaId
        };

        _context.Viandas.Add(vianda);
        await _context.SaveChangesAsync();

        var creada = await _context.Viandas.Where(v => v.Id == vianda.Id).Select(AsDto).FirstAsync();

        // 201 Created + la ubicación del recurso nuevo
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    // PUT api/viandas/3
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ViandaDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { error = "El id de la URL no coincide con el del cuerpo." });

        var vianda = await _context.Viandas.FindAsync(id);
        if (vianda is null)
            return NotFound(new { error = $"No existe la vianda con id {id}." });

        if (!await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId))
            return BadRequest(new { error = "La categoría indicada no existe." });

        vianda.Nombre = dto.Nombre;
        vianda.Descripcion = dto.Descripcion;
        vianda.Precio = dto.Precio;
        vianda.Activa = dto.Activa;
        vianda.CategoriaId = dto.CategoriaId;

        await _context.SaveChangesAsync();
        return NoContent();   // 204: se actualizó, no hay nada que devolver
    }

    // DELETE api/viandas/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vianda = await _context.Viandas.FindAsync(id);
        if (vianda is null)
            return NotFound(new { error = $"No existe la vianda con id {id}." });

        // Si ya fue pedida, borrarla arrastraría los pedidos: mejor avisar (409 Conflict)
        if (await _context.DetallesPedido.AnyAsync(d => d.ViandaId == id))
            return Conflict(new { error = "La vianda ya tiene pedidos. Podés darla de baja (Activa = false) en vez de eliminarla." });

        _context.Viandas.Remove(vianda);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}