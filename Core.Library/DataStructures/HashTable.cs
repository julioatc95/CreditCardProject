using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Tabla Hash genérica simple con encadenamiento.
    /// Usa un array de buckets, cada uno es una LinkedList de pares (clave, valor).
    /// </summary>
    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        // Array de buckets; cada bucket es una lista ligada de pares key/value
        private readonly LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
        // Comparador para las claves (por defecto, EqualityComparer<TKey>.Default)
        private readonly IEqualityComparer<TKey> _comparer;
        // Número total de pares almacenados
        public int Count { get; private set; }

        /// <summary>
        /// Crea la tabla con capacidad inicial y comparador opcional.
        /// </summary>
        public HashTable(int capacity = 16, IEqualityComparer<TKey>? comparer = null)
        {
            _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        /// <summary>
        /// Genera el índice de bucket a partir del hash de la clave.
        /// </summary>
        private int GetBucketIndex(TKey key)
        {
            // Obtiene hash no negativo
            var hash = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            // Módulo por el número de buckets
            return hash % _buckets.Length;
        }

        /// <summary>
        /// Agrega un nuevo par (key, value). Lanza si la clave ya existe.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);
            // Inicializa el bucket si está vacío
            _buckets[index] ??= new LinkedList<KeyValuePair<TKey, TValue>>();

            // Verifica duplicados
            foreach (var kv in _buckets[index])
            {
                if (_comparer.Equals(kv.Key, key))
                    throw new ArgumentException($"La clave ya existe: {key}");
            }

            // Añade al final de la lista del bucket
            _buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            Count++;
        }

        /// <summary>
        /// Elimina el elemento con la clave dada. Devuelve true si se eliminó.
        /// </summary>
        public bool Remove(TKey key)
        {
            int index = GetBucketIndex(key);
            var bucket = _buckets[index];
            if (bucket == null) return false;

            var node = bucket.First;
            while (node != null)
            {
                if (_comparer.Equals(node.Value.Key, key))
                {
                    // Quita el nodo de la lista
                    bucket.Remove(node);
                    Count--;
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        /// <summary>
        /// Intenta obtener el valor para la clave dada.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetBucketIndex(key);
            var bucket = _buckets[index];
            if (bucket != null)
            {
                foreach (var kv in bucket)
                {
                    if (_comparer.Equals(kv.Key, key))
                    {
                        value = kv.Value;
                        return true;
                    }
                }
            }
            // Si no existe, devuelve default y false
            value = default!;
            return false;
        }

        /// <summary>
        /// Indexer para acceder o asignar por clave.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var val))
                    return val;
                throw new KeyNotFoundException($"Clave no encontrada: {key}");
            }
            set
            {
                int index = GetBucketIndex(key);
                // Asegura bucket inicializado
                var bucket = _buckets[index] ??= new LinkedList<KeyValuePair<TKey, TValue>>();

                // Busca y reemplaza si existe
                var node = bucket.First;
                while (node != null)
                {
                    if (_comparer.Equals(node.Value.Key, key))
                    {
                        bucket.Remove(node);
                        bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                    node = node.Next;
                }

                // Si no existía, añade como nuevo
                bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
                Count++;
            }
        }

        /// <summary>
        /// Iterador sobre todos los pares (key, value) de la tabla.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var bucket in _buckets)
            {
                if (bucket != null)
                {
                    foreach (var kv in bucket)
                        yield return kv;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
