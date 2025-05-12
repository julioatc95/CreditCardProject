using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Tabla Hash genérica simple con encadenamiento (buckets de listas).
    /// </summary>
    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
        private readonly IEqualityComparer<TKey> _comparer;
        public int Count { get; private set; }

        public HashTable(int capacity = 16, IEqualityComparer<TKey>? comparer = null)
        {
            _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        private int GetBucketIndex(TKey key)
        {
            var hash = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            return hash % _buckets.Length;
        }

        public void Add(TKey key, TValue value)
        {
            var index = GetBucketIndex(key);
            _buckets[index] ??= new LinkedList<KeyValuePair<TKey, TValue>>();

            // Previene claves duplicadas
            foreach (var kv in _buckets[index])
                if (_comparer.Equals(kv.Key, key))
                    throw new ArgumentException($"La clave ya existe: {key}");

            _buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            Count++;
        }

        public bool Remove(TKey key)
        {
            var index = GetBucketIndex(key);
            var bucket = _buckets[index];
            if (bucket == null) return false;

            var node = bucket.First;
            while (node != null)
            {
                if (_comparer.Equals(node.Value.Key, key))
                {
                    bucket.Remove(node);
                    Count--;
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var index = GetBucketIndex(key);
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
            value = default!;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var val)) return val;
                throw new KeyNotFoundException($"Clave no encontrada: {key}");
            }
            set
            {
                var index = GetBucketIndex(key);
                var bucket = _buckets[index] ??= new LinkedList<KeyValuePair<TKey, TValue>>();

                var node = bucket.First;
                while (node != null)
                {
                    if (_comparer.Equals(node.Value.Key, key))
                    {
                        // Reemplaza
                        bucket.Remove(node);
                        bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                    node = node.Next;
                }

                // No existía, agrega
                bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
                Count++;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var bucket in _buckets)
                if (bucket != null)
                    foreach (var kv in bucket)
                        yield return kv;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
