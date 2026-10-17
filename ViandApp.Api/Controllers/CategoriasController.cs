using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViandApp.Api.Data;
using ViandApp.Api.Dtos;

namespace ViandApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ViandAppContext _context;

    public CategoriasController(ViandAppContext context)
    {
        _context = context;
    }

    // GET api/categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre })
            .ToListAsync();

        return Ok(categorias);
    }
}