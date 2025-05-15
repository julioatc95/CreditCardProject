using System.Collections.Generic;
using Core.Library.Models;

namespace Core.Library.Services
{
    /// <summary>
    /// Interfaz que define las operaciones CRUD básicas para la entidad Cliente.
    /// </summary>
    public interface IClienteService
    {
        IEnumerable<Cliente> ObtenerTodos();
        Cliente? ObtenerPorId(string id);
        void Crear(Cliente cliente);
        bool Actualizar(string id, Cliente cliente);
        bool Eliminar(string id);

    }
}
