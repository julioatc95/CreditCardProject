using System.Collections.Generic;
using Core.Library.Models;

namespace Core.Library.Services
{
    public interface ITarjetaService
    {
        IEnumerable<Tarjeta> ObtenerTodas();
        Tarjeta? ObtenerPorId(string id);
        void Crear(Tarjeta tarjeta);
        bool Actualizar(string id, Tarjeta tarjeta);
        bool Eliminar(string id);

        // se puede añadir métodos específicos, p.ej. Saldo, CambioPin…
    }
}
