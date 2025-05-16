using System;
using System.Collections.Generic;
using Core.Library.DataStructures;
using Core.Library.Models;

namespace Core.Library.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly HashTable<string, Transaccion> _tabla;

        public TransaccionService(IEnumerable<Transaccion> iniciales)
        {
            _tabla = new HashTable<string, Transaccion>();
            foreach (var tx in iniciales)
                _tabla.Add(tx.Id, tx);
        }

        public IEnumerable<Transaccion> ObtenerTodas()
        {
            foreach (var kv in _tabla)
                yield return kv.Value;
        }

        public Transaccion? ObtenerPorId(string id)
            => _tabla.TryGetValue(id, out var tx) ? tx : null;

        public void Crear(Transaccion tx)
        {
            // Intentamos leer; si devuelve true, la clave ya existía
            if (_tabla.TryGetValue(tx.Id, out _))
                throw new ArgumentException($"Ya existe transacción con Id '{tx.Id}'.");

            _tabla.Add(tx.Id, tx);
        }


        public bool Actualizar(string id, Transaccion tx)
        {
            if (!_tabla.TryGetValue(id, out _)) return false;
            _tabla[id] = tx;
            return true;
        }

        public bool Eliminar(string id)
            => _tabla.Remove(id);
    }
}
