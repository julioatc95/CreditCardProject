using System;
namespace Core.Library.Models
{
    public class Tarjeta
    {
        //Identificador unico de tarjeta
        public string id { get; set; }

        //Relacion al cliente (Cliente.Id)
        public string ClienteId { get; set; }
        public string Numero { get; set; }
        public string Pin { get; set; }

        public DateTime FechaExpiracion { get; set; }
        public decimal Saldo { get; set; }
    }

}
