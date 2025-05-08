using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Implementación de Pila genérica (LIFO).
    /// </summary>
    public class Stack<T> : IEnumerable<T>
    {
        private readonly List<T> _elements = new();

        /// <summary>
        /// Cuántos elementos hay en la pila.
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// Añade un elemento al tope de la pila.
        /// </summary>
        public void Push(T item)
        {
            _elements.Add(item);
        }

        /// <summary>
        /// Saca el elemento del tope de la pila.
        /// </summary>
        public T Pop()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Pila vacía");
            var index = _elements.Count - 1;
            var item = _elements[index];
            _elements.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Lee el elemento del tope sin sacarlo.
        /// </summary>
        public T Peek()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Pila vacía");
            return _elements[^1];
        }

        public IEnumerator<T> GetEnumerator()
        {
            // Devolvemos de arriba abajo
            for (int i = _elements.Count - 1; i >= 0; i--)
                yield return _elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
