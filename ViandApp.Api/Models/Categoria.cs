using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Models
{
    public class Categoria
    {
        public int Id { get; set; }   // clave primaria (EF la detecta por llamarse "Id")

        [Required, MaxLength(50)]     // validación: obligatorio, máximo 50 caracteres
        public string Nombre { get; set; } = string.Empty;

        // Una categoría tiene muchas viandas
        public List<Vianda> Viandas { get; set; } = new();
    }
}
