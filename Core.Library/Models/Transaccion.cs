using System;

namespace Core.Library.Models
{
    /// <summary>
    /// Tipos de operación que puede tener una transacción.
    /// </summary>
    public enum TipoTransaccion
    {
        Pago,
        Consumo
    }

    /// <summary>
    /// Representa un movimiento (transacción) sobre una tarjeta de crédito.
    /// </summary>
    public class Transaccion
    {
        /// <summary>
        /// Identificador único de la transacción (p.ej. "T1").
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Id de la tarjeta a la que pertenece esta transacción.
        /// </summary>
        public required string TarjetaId { get; set; }

        /// <summary>
        /// Tipo de la transacción (Pago o Consumo).
        /// </summary>
        public TipoTransaccion Tipo { get; set; }

        /// <summary>
        /// Descripción libre del movimiento (p.ej. "Compra en Supermercado").
        /// </summary>
        public required string Descripcion { get; set; }

        /// <summary>
        /// Fecha en que se registró la transacción.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Importe de la transacción (positivo para consumos, negativo para pagos).
        /// </summary>
        public decimal Monto { get; set; }
    }
}
