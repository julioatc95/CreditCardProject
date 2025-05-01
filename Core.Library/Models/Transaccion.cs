using System;
namespace Core.Library.Models
{
    public enum TipoTransaccion
    {
        consumo,
        Pago,
        Bloqueo,
        Renovacion,
        CambioPin,
        AumentoLimite
    }

    public class Transaccion
    {
        public string Id { get; set; }

        //Relacion a tarjeta (tarjeta.id)

        public string TarjetaId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public TipoTransaccion Tipo { get; set; }
    }
}
