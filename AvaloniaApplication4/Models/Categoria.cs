using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaApplication4.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // El ComboBox muestra el resultado de ToString()
        public override string ToString() => Nombre;
    }
}
