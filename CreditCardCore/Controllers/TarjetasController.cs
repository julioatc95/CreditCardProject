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

        // Inyectamos los servicios de Tarjeta y Transacción
        public TarjetasController(
            ITarjetaService tarjetaSvc,
            ITransaccionService txSvc)
        {
            _tarjetaSvc = tarjetaSvc;
            _txSvc = txSvc;
        }

        // ------------------ CRUD Básico ------------------

        /// <summary>
        /// GET /api/tarjetas
        /// Devuelve todas las tarjetas.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Tarjeta>> GetAll()
            => Ok(_tarjetaSvc.ObtenerTodas());

        /// <summary>
        /// GET /api/tarjetas/{id}
        /// Devuelve la tarjeta por Id o 404 si no existe.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Tarjeta> GetById(string id)
        {
            var t = _tarjetaSvc.ObtenerPorId(id);
            return t is not null ? Ok(t) : NotFound();
        }

        /// <summary>
        /// POST /api/tarjetas
        /// Crea una nueva tarjeta en memoria.
        /// </summary>
        [HttpPost]
        public ActionResult Create([FromBody] Tarjeta nueva)
        {
            _tarjetaSvc.Crear(nueva);
            // 201 Created + cabecera Location apuntando a GET /api/tarjetas/{id}
            return CreatedAtAction(nameof(GetById), new { id = nueva.Id }, nueva);
        }

        /// <summary>
        /// PUT /api/tarjetas/{id}
        /// Actualiza una tarjeta existente o devuelve 404.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Tarjeta upd)
            => _tarjetaSvc.Actualizar(id, upd) ? NoContent() : NotFound();

        /// <summary>
        /// DELETE /api/tarjetas/{id}
        /// Elimina una tarjeta o devuelve 404.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
            => _tarjetaSvc.Eliminar(id) ? NoContent() : NotFound();


        // --------------- Operaciones Especiales ---------------

        /// <summary>
        /// GET /api/tarjetas/{id}/saldo
        /// Suma todos los montos de sus transacciones para devolver el saldo.
        /// </summary>
        [HttpGet("{id}/saldo")]
        public ActionResult<decimal> GetSaldo(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            // Filtramos transacciones de esta tarjeta
            var movs = _txSvc.ObtenerTodas()
                             .Where(tx => tx.TarjetaId == id);
            var saldo = movs.Sum(tx => tx.Monto);
            return Ok(saldo);
        }

        /// <summary>
        /// GET /api/tarjetas/{id}/movimientos
        /// Devuelve el historial completo de transacciones.
        /// </summary>
        [HttpGet("{id}/movimientos")]
        public ActionResult<IEnumerable<Transaccion>> GetMovimientos(string id)
        {
            if (_tarjetaSvc.ObtenerPorId(id) == null) return NotFound();

            var movs = _txSvc.ObtenerTodas()
                             .Where(tx => tx.TarjetaId == id);
            return Ok(movs);
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/pago
        /// Registra un pago (monto NEGATIVO).
        /// </summary>
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
                Monto = -Math.Abs(monto)  // <0
            };
            _txSvc.Crear(tx);
            return CreatedAtAction(nameof(GetMovimientos), new { id }, tx);
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/consumo
        /// Registra un consumo (monto POSITIVO).
        /// </summary>
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
                Monto = Math.Abs(monto)  // >0
            };
            _txSvc.Crear(tx);
            return CreatedAtAction(nameof(GetMovimientos), new { id }, tx);
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/pin
        /// Cambia el PIN de la tarjeta.
        /// </summary>
        [HttpPost("{id}/pin")]
        public ActionResult CambiarPin(string id, [FromBody] string nuevoPin)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            tarjeta.Pin = nuevoPin;
            _tarjetaSvc.Actualizar(id, tarjeta);
            return NoContent();
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/bloquear
        /// Bloqueo temporal (marcar flag en el modelo).
        /// </summary>
        [HttpPost("{id}/bloquear")]
        public ActionResult Bloquear(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            // Aquí podrías hacer: tarjeta.Bloqueada = true;
            // _tarjetaSvc.Actualizar(id, tarjeta);
            return NoContent();
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/limite
        /// Solicita aumento de límite (simulado).
        /// </summary>
        [HttpPost("{id}/limite")]
        public ActionResult SolicitarAumento(string id, [FromBody] decimal nuevoLimite)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            // Lógica de negocio aquí…
            return Accepted($"Solicitud de aumento a {nuevoLimite} recibida");
        }

        /// <summary>
        /// POST /api/tarjetas/{id}/renovar
        /// Extiende la expiración de la tarjeta 3 años.
        /// </summary>
        [HttpPost("{id}/renovar")]
        public ActionResult Renovar(string id)
        {
            var tarjeta = _tarjetaSvc.ObtenerPorId(id);
            if (tarjeta == null) return NotFound();

            tarjeta.FechaExpiracion = tarjeta.FechaExpiracion.AddYears(3);
            _tarjetaSvc.Actualizar(id, tarjeta);
            return NoContent();
        }
    }
}
