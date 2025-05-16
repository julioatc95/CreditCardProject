using System;
using System.Collections.Generic;
using System.Linq;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetasController : ControllerBase
    {
        private readonly ITarjetaService _tarjetaSvc;
        private readonly ITransaccionService _txSvc;

        public TarjetasController(
            ITarjetaService tarjetaSvc,
            ITransaccionService txSvc)
        {
            _tarjetaSvc = tarjetaSvc;
            _txSvc = txSvc;
        }

        // --- Aquí pegas tus endpoints personalizados ---

        // 1) Saldo actual de la tarjeta
        [HttpGet("{id}/saldo")]
        public ActionResult<decimal> GetSaldo(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            var movs = _txSvc.ObtenerTodas()
                             .Where(tx => tx.TarjetaId == id);
            var saldo = movs.Sum(tx => tx.Monto);
            return Ok(saldo);
        }

        // 2) Lista de movimientos de una tarjeta
        [HttpGet("{id}/movimientos")]
        public ActionResult<IEnumerable<Transaccion>> GetMovimientos(string id)
        {
            if (_tarjetaSvc.ObtenerPorId(id) == null) return NotFound();

            var movs = _txSvc.ObtenerTodas()
                             .Where(tx => tx.TarjetaId == id);
            return Ok(movs);
        }

        // 3) Registrar un pago (Monto negativo)
        [HttpPost("{id}/pago")]
        public ActionResult Pagar(string id, [FromBody] decimal monto)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            var tx = new Transaccion
            {
                Id = Guid.NewGuid().ToString(),
                TarjetaId = id,
                Tipo = TipoTransaccion.Pago,
                Descripcion = "Pago manual",
                Fecha = DateTime.UtcNow,
                Monto = -Math.Abs(monto)
            };
            _txSvc.Crear(tx);
            return CreatedAtAction(nameof(GetMovimientos), new { id }, tx);
        }

        // 4) Registrar un consumo (Monto positivo)
        [HttpPost("{id}/consumo")]
        public ActionResult Consumir(string id, [FromBody] decimal monto)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            var tx = new Transaccion
            {
                Id = Guid.NewGuid().ToString(),
                TarjetaId = id,
                Tipo = TipoTransaccion.Consumo,
                Descripcion = "Consumo manual",
                Fecha = DateTime.UtcNow,
                Monto = Math.Abs(monto)
            };
            _txSvc.Crear(tx);
            return CreatedAtAction(nameof(GetMovimientos), new { id }, tx);
        }

        // 5) Cambio de PIN
        [HttpPost("{id}/pin")]
        public ActionResult CambiarPin(string id, [FromBody] string nuevoPin)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            tarjeta.Pin = nuevoPin;
            _tarjetaSvc.Actualizar(id, tarjeta);
            return NoContent();
        }

        // 6) Bloqueo temporal
        [HttpPost("{id}/bloquear")]
        public ActionResult Bloquear(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            // Si añades un flag Locked en el modelo, aquí lo ajustarías
            return NoContent();
        }

        // 7) Solicitud de aumento de límite
        [HttpPost("{id}/limite")]
        public ActionResult SolicitarAumento(string id, [FromBody] decimal nuevoLimite)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            // Lógica de solicitud…
            return Accepted($"Solicitud de aumento a {nuevoLimite} recibida");
        }

        // 8) Renovación de tarjeta
        [HttpPost("{id}/renovar")]
        public ActionResult Renovar(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            tarjeta.FechaExpiracion = tarjeta.FechaExpiracion.AddYears(3);
            _tarjetaSvc.Actualizar(id, tarjeta);
            return NoContent();
        }
        // --- Fin de los endpoints personalizados ---
    }
}
