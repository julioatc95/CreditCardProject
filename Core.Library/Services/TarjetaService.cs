using System;
using System.Collections.Generic;
using Core.Library.DataStructures;
using Core.Library.Models;

namespace Core.Library.Services
{


    /// <summary>
    /// Implementación de ITarjetaService usando HashTable para almacenamiento en memoria.
    /// </summary>
    public class TarjetaService : ITarjetaService
    {
        private readonly HashTable<string, Tarjeta> _tabla;

        public TarjetaService(IEnumerable<Tarjeta> tarjetasIniciales)
        {
            _tabla = new HashTable<string, Tarjeta>();
            foreach (var t in tarjetasIniciales)
            {
                _tabla.Add(t.Id, t);
            }
        }

        public IEnumerable<Tarjeta> ObtenerTodas()
        {
            // Recorre cada par (KeyValuePair) y devuelve solo el valor (Tarjeta)
            foreach (var kv in _tabla)
                yield return kv.Value;
        }

        public Tarjeta? ObtenerPorId(string id)
        {
            // Devuelve la tarjeta si existe, o null si no
            return _tabla.TryGetValue(id, out var tarjeta) ? tarjeta : null;
        }

        public void Crear(Tarjeta tarjeta)
        {
            // Evita duplicados
            if (_tabla.TryGetValue(tarjeta.Id, out _))
                throw new ArgumentException($"Ya existe tarjeta con Id '{tarjeta.Id}'.");
            _tabla.Add(tarjeta.Id, tarjeta);
        }

        public bool Actualizar(string id, Tarjeta tarjeta)
        {
            // Solo actualiza si la tarjeta existe
            if (!_tabla.TryGetValue(id, out _))
                return false;
            _tabla[id] = tarjeta;
            return true;
        }

        public bool Eliminar(string id)
        {
            // Elimina el par y devuelve si tuvo éxito
            return _tabla.Remove(id);
        }
    }
}
