using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Implementación de Cola genérica (FIFO).
    /// </summary>
    public class Queue<T> : IEnumerable<T>
    {
        private readonly List<T> _elements = new();

        /// <summary>Cuántos elementos hay en la cola.</summary>
        public int Count => _elements.Count;

        /// <summary>Añade un elemento al final de la cola.</summary>
        public void Enqueue(T item)
        {
            _elements.Add(item);
        }

        /// <summary>Retira y devuelve el primer elemento de la cola.</summary>
        public T Dequeue()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Cola vacía");
            var item = _elements[0];
            _elements.RemoveAt(0);
            return item;
        }

        /// <summary>Lee sin retirar el primer elemento de la cola.</summary>
        public T Peek()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Cola vacía");
            return _elements[0];
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _elements)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
