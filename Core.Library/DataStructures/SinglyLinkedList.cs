using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    ///<summary
    ///Implementacion de lista enlazada simple ( singly linked list).
    ///</summary>
    
    public class SinglyLinkedList<T> : IEnumerable<T>
    {
        private LinkedListNode<T>? _head;
        private LinkedListNode<T>? _tail;
        public int Count { get; private set; }

        ///<summary
        ///Agregando un nuevo elemento al final de la lista
        ///</summary>
        
        public void AddLast(T value)
        {
           var node = new LinkedListNode<T>(value);
            if (_head == null) 
            {
                _head = _tail = node;
            }
            else
            {
                _tail!.Next = node;
                _tail = node;
            }
            Count++;
        }

        /// <summary>
        /// Elimina la primera ocurrencia del valor y retorna true si se eliminó.
        /// </summary>

        public bool Remove(T Value)
        {
            LinkedListNode<T>? prev = null, current = _head;

            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, Value))
                { 
                    if (prev == null)
                        _head = current.Next;
                    else
                        prev.Next = current.Next;
                    if (current == _tail)
                        _tail = prev;

                    Count--;
                    return true;
                } 
                
                prev = current;
                current = current.Next;
            }

            return false;

        }

        /// <summary>
        /// Recorre la lista y devuelve cada elemento.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            var node = _head;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}