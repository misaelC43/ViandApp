using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaApplication4.Models
{
    public class Pedido
    {
        public string NombreVianda { get; set; } = string.Empty;
        public DateTime HoraPedido { get; set; }
        public DateTime HoraEntrega { get; set; }
    }
}
