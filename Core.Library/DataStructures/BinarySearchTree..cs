using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Nodo de árbol binario de búsqueda genérico.
    /// </summary>
    /// <typeparam name="T">Tipo de dato comparable almacenado en el nodo.</typeparam>
    public class BinarySearchTreeNode<T>
        where T : IComparable<T>
    {
        /// <summary>Valor contenido en el nodo.</summary>
        public T Value { get; set; }

        /// <summary>Hijo izquierdo (valores menores).</summary>
        public BinarySearchTreeNode<T>? Left { get; set; }

        /// <summary>Hijo derecho (valores mayores).</summary>
        public BinarySearchTreeNode<T>? Right { get; set; }

        public BinarySearchTreeNode(T value)
        {
            Value = value;  // Asigna el valor al nodo
        }
    }

    /// <summary>
    /// Árbol Binario de Búsqueda (BST) genérico.
    /// Permite Insert, Contains, Remove y recorrido InOrden.
    /// </summary>
    public class BinarySearchTree<T> : IEnumerable<T>
        where T : IComparable<T>
    {
        private BinarySearchTreeNode<T>? _root;  // Raíz del árbol

        /// <summary>
        /// Inserta un valor en el BST.
        /// </summary>
        public void Insert(T value)
        {
            if (_root == null)
            {
                // Caso base: si no hay raíz, este nodo se convierte en raíz
                _root = new BinarySearchTreeNode<T>(value);
            }
            else
            {
                // De lo contrario, inserta recursivamente
                InsertRec(_root, value);
            }
        }

        /// <summary>
        /// Lógica recursiva para insertar un valor.
        /// </summary>
        private void InsertRec(BinarySearchTreeNode<T> node, T value)
        {
            // Compara el valor con el nodo actual
            int compare = value.CompareTo(node.Value);
            if (compare <= 0)
            {
                // Si es menor o igual, va al subárbol izquierdo
                if (node.Left == null)
                    node.Left = new BinarySearchTreeNode<T>(value);
                else
                    InsertRec(node.Left, value);
            }
            else
            {
                // Si es mayor, va al subárbol derecho
                if (node.Right == null)
                    node.Right = new BinarySearchTreeNode<T>(value);
                else
                    InsertRec(node.Right, value);
            }
        }

        /// <summary>
        /// Verifica si un valor existe en el BST.
        /// </summary>
        public bool Contains(T value)
        {
            return ContainsRec(_root, value);
        }

        /// <summary>
        /// Lógica recursiva de búsqueda.
        /// </summary>
        private bool ContainsRec(BinarySearchTreeNode<T>? node, T value)
        {
            if (node == null)
                return false;  // Llegó a hoja sin encontrar

            int compare = value.CompareTo(node.Value);
            if (compare == 0)
                return true;   // Valor encontrado
            else if (compare < 0)
                return ContainsRec(node.Left, value);  // Buscar en izquierda
            else
                return ContainsRec(node.Right, value); // Buscar en derecha
        }

        /// <summary>
        /// Elimina un valor del BST.
        /// Retorna true si se eliminó correctamente.
        /// </summary>
        public bool Remove(T value)
        {
            bool removed;
            (_root, removed) = RemoveRec(_root, value);
            return removed;
        }

        /// <summary>
        /// Lógica recursiva de eliminación.
        /// Devuelve la nueva raíz del subtree y flag de eliminación.
        /// </summary>
        private (BinarySearchTreeNode<T>?, bool) RemoveRec(BinarySearchTreeNode<T>? node, T value)
        {
            if (node == null)
                return (null, false);  // No existe el nodo

            int compare = value.CompareTo(node.Value);
            if (compare < 0)
            {
                // El valor a eliminar está en el subárbol izquierdo
                (node.Left, var removed) = RemoveRec(node.Left, value);
                return (node, removed);
            }
            else if (compare > 0)
            {
                // Está en el subárbol derecho
                (node.Right, var removed) = RemoveRec(node.Right, value);
                return (node, removed);
            }
            else
            {
                // Nodo encontrado: tres casos de eliminación
                if (node.Left == null)
                    return (node.Right, true);    // Solo hijo derecho o ninguno
                if (node.Right == null)
                    return (node.Left, true);     // Solo hijo izquierdo

                // Dos hijos: reemplaza con el sucesor (mínimo en subárbol derecho)
                var successor = FindMin(node.Right);
                node.Value = successor.Value;
                (node.Right, _) = RemoveRec(node.Right, successor.Value);
                return (node, true);
            }
        }

        /// <summary>
        /// Encuentra el nodo con el valor mínimo en un subtree.
        /// </summary>
        private BinarySearchTreeNode<T> FindMin(BinarySearchTreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <summary>
        /// Recorrido In-Order (ascendente) para iterar los valores.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraverse(_root).GetEnumerator();
        }

        /// <summary>
        /// Genera un enumerable con recorrido In-Order.
        /// </summary>
        private IEnumerable<T> InOrderTraverse(BinarySearchTreeNode<T>? node)
        {
            if (node != null)
            {
                foreach (var v in InOrderTraverse(node.Left))
                    yield return v;
                yield return node.Value;
                foreach (var v in InOrderTraverse(node.Right))
                    yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
