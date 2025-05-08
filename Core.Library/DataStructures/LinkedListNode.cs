namespace Core.Library.DataStructures
{
    /// <summary>
    /// Nodo para lista enlazada simple genérica.
    /// </summary>
    public class LinkedListNode<T>
    {
        public T Value { get; set; }

        // Permitir null en Next, porque al final de la lista no hay siguiente nodo
        public LinkedListNode<T>? Next { get; set; }

        public LinkedListNode(T value)
        {
            Value = value;
            Next = null;
        }
    }
}
