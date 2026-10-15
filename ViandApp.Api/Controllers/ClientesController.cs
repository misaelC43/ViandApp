using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViandApp.Api.Data;
using ViandApp.Api.Dtos;
using ViandApp.Api.Models;

namespace ViandApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ViandAppContext _context;

    public ClientesController(ViandAppContext context)
    {
        _context = context;
    }

    // GET api/clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        var clientes = await _context.Clientes
            .OrderBy(c => c.Nombre)
            .Select(c => new ClienteDto { Id = c.Id, Nombre = c.Nombre, Telefono = c.Telefono })
            .ToListAsync();

        return Ok(clientes);
    }

    // GET api/clientes/2
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _context.Clientes
            .Where(c => c.Id == id)
            .Select(c => new ClienteDto { Id = c.Id, Nombre = c.Nombre, Telefono = c.Telefono })
            .FirstOrDefaultAsync();

        if (cliente is null)
            return NotFound(new { error = $"No existe el cliente con id {id}." });

        return Ok(cliente);
    }

    // POST api/clientes
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteDto dto)
    {
        var cliente = new Cliente { Nombre = dto.Nombre, Telefono = dto.Telefono };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        dto.Id = cliente.Id;
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, dto);
    }
}