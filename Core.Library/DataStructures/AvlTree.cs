using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Library.DataStructures
{
    /// <summary>
    /// Nodo para árbol AVL genérico.
    /// </summary>
    public class AvlNode<T> where T : IComparable<T>
    {
        public T Value { get; set; }                 // Valor del nodo
        public AvlNode<T>? Left { get; set; }        // Subárbol izquierdo
        public AvlNode<T>? Right { get; set; }       // Subárbol derecho
        public int Height { get; set; }              // Altura del nodo

        public AvlNode(T value)
        {
            Value = value;
            Height = 1; // Nuevo nodo inicia con altura 1
        }
    }

    /// <summary>
    /// Árbol AVL auto-balanceado (subclase de BST con rotaciones).
    /// </summary>
    public class AvlTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private AvlNode<T>? _root;

        /// <summary>
        /// Inserta un valor y reequilibra el árbol.
        /// </summary>
        public void Insert(T value)
        {
            _root = InsertRec(_root, value);
        }

        /// <summary>
        /// Lógica recursiva de inserción con balanceo.
        /// </summary>
        private AvlNode<T> InsertRec(AvlNode<T>? node, T value)
        {
            if (node == null)
                return new AvlNode<T>(value);

            int cmp = value.CompareTo(node.Value);
            if (cmp < 0)
                node.Left = InsertRec(node.Left, value);
            else if (cmp > 0)
                node.Right = InsertRec(node.Right, value);
            else
                return node; // Valores duplicados no permitidos

            // Actualiza altura
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // Obtiene factor de balanceo
            int balance = GetBalance(node);

            // Rotaciones según el caso:
            // Left Left Case
            if (balance > 1 && value.CompareTo(node.Left!.Value) < 0)
                return RotateRight(node);

            // Right Right Case
            if (balance < -1 && value.CompareTo(node.Right!.Value) > 0)
                return RotateLeft(node);

            // Left Right Case
            if (balance > 1 && value.CompareTo(node.Left!.Value) > 0)
            {
                node.Left = RotateLeft(node.Left!);
                return RotateRight(node);
            }

            // Right Left Case
            if (balance < -1 && value.CompareTo(node.Right!.Value) < 0)
            {
                node.Right = RotateRight(node.Right!);
                return RotateLeft(node);
            }

            return node; // Sin rotación necesaria
        }

        /// <summary>Altura de un nodo (0 si es null).</summary>
        private int GetHeight(AvlNode<T>? node) => node?.Height ?? 0;

        /// <summary>Factor de balance: altura(Left) - altura(Right).</summary>
        private int GetBalance(AvlNode<T>? node) => node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        /// <summary>Rotación derecha (para balance LL).</summary>
        private AvlNode<T> RotateRight(AvlNode<T> y)
        {
            var x = y.Left!;
            var T2 = x.Right;

            // Rotación
            x.Right = y;
            y.Left = T2;

            // Actualizar alturas
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x; // Nueva raíz
        }

        /// <summary>Rotación izquierda (para balance RR).</summary>
        private AvlNode<T> RotateLeft(AvlNode<T> x)
        {
            var y = x.Right!;
            var T2 = y.Left;

            // Rotación
            y.Left = x;
            x.Right = T2;

            // Actualizar alturas
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y; // Nueva raíz
        }

        /// <summary>
        /// Recorrido In-Order para iterar los valores en orden ascendente.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => InOrder(_root).GetEnumerator();

        private IEnumerable<T> InOrder(AvlNode<T>? node)
        {
            if (node != null)
            {
                foreach (var v in InOrder(node.Left))
                    yield return v;
                yield return node.Value;
                foreach (var v in InOrder(node.Right))
                    yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
