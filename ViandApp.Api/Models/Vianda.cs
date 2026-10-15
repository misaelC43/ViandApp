using System.ComponentModel.DataAnnotations;

namespace ViandApp.Api.Models
{
    public class Vianda
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 1000000)]        // el precio tiene que ser mayor a cero
        public decimal Precio { get; set; }

        public bool Activa { get; set; } = true;

        // Clave foránea: a qué categoría pertenece
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
