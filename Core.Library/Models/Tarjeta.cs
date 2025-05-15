using System;
namespace Core.Library.Models
{
    public class Tarjeta
    {
        public string Id { get; set; } = string.Empty;

        public string ClienteId { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;

        public DateTime FechaExpiracion { get; set; }

        public decimal Saldo { get; set; }
    }


}
