using System;
namespace Core.Library.Models
{
    public class Tarjeta
    {
        public string Id { get; set; } = default!;

        public string ClienteId { get; set; } = default!;

        public required string Numero { get; set; }

        public required string Pin { get; set; }

        public DateTime FechaExpiracion { get; set; }
        public decimal Saldo { get; set; }
    }


}
