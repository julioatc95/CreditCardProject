using System.Collections.Generic;
using Core.Library.Models;

namespace Core.Library.Services
{
    /// <summary>
    /// Operaciones CRUD básicas para Transaccion.
    /// </summary>
    public interface ITransaccionService
    {
        IEnumerable<Transaccion> ObtenerTodas();
        Transaccion? ObtenerPorId(string id);
        void Crear(Transaccion tx);
        bool Actualizar(string id, Transaccion tx);
        bool Eliminar(string id);
    }
}
