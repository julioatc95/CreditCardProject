using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;  // Inyección de dependencia del servicio
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// GET /api/clientes
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            var clientes = _clienteService.ObtenerTodos();
            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por su Id.
        /// GET /api/clientes/{id}
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(string id)
        {
            var cliente = _clienteService.ObtenerPorId(id);
            if (cliente == null)
                return NotFound($"Cliente con Id='{id}' no encontrado.");
            return Ok(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// POST /api/clientes
        /// </summary>
        [HttpPost]
        public ActionResult Create([FromBody] Cliente nuevo)
        {
            try
            {
                _clienteService.Crear(nuevo);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// PUT /api/clientes/{id}
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody] Cliente actualizacion)
        {
            if (!_clienteService.Actualizar(id, actualizacion))
                return NotFound($"Cliente con Id='{id}' no encontrado.");
            return NoContent();
        }

        /// <summary>
        /// Elimina un cliente.
        /// DELETE /api/clientes/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (!_clienteService.Eliminar(id))
                return NotFound($"Cliente con Id='{id}' no encontrado.");
            return NoContent();
        }
    }
}
