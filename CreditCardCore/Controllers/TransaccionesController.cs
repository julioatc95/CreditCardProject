using System.Collections.Generic;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardCore.Controllers
{
    [ApiController]
    [Route("api/transacciones")]
    public class TransaccionesController : ControllerBase
    {
        private readonly ITransaccionService _svc;
        public TransaccionesController(ITransaccionService svc) => _svc = svc;

        [HttpGet]
        public IEnumerable<Transaccion> GetAll() => _svc.ObtenerTodas();

        [HttpGet("{id}")]
        public ActionResult<Transaccion> GetById(string id)
        {
            var tx = _svc.ObtenerPorId(id);
            return tx is not null ? Ok(tx) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transaccion tx)
        {
            _svc.Crear(tx);
            return CreatedAtAction(nameof(GetById), new { id = tx.Id }, tx);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Transaccion tx)
            => _svc.Actualizar(id, tx) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
            => _svc.Eliminar(id) ? NoContent() : NotFound();
    }
}
