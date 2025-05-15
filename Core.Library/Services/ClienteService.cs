using System;
using System.Collections.Generic;
using Core.Library.DataStructures;
using Core.Library.Models;


namespace Core.Library.Services
{
    /// <summary>
    /// Interfaz que define operaciones CRUD para Cliente.
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        IEnumerable<Cliente> ObtenerTodos();

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        Cliente? ObtenerPorId(string id);

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        void Crear(Cliente cliente);

        /// <summary>
        /// Actualiza un cliente existente. Devuelve true si existe y se actualiza.
        /// </summary>
        bool Actualizar(string id, Cliente cliente);

        /// <summary>
        /// Elimina un cliente por su identificador. Devuelve true si se eliminó.
        /// </summary>
        bool Eliminar(string id);
    }

    /// <summary>
    /// Implementación de IClienteService usando HashTable para almacenamiento en memoria.
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly HashTable<string, Cliente> _tablaClientes;

        /// <summary>
        /// Inicializa el servicio con una lista inicial de clientes.
        /// </summary>
        public ClienteService(IEnumerable<Cliente> clientesIniciales)
        {
            _tablaClientes = new HashTable<string, Cliente>();
            foreach (var c in clientesIniciales)
            {
                _tablaClientes.Add(c.Id, c);  // Agrega cada cliente al bucket correspondiente
            }
        }

        public IEnumerable<Cliente> ObtenerTodos()
        {
            // Recorre todos los pares en la tabla y devuelve el valor (Cliente)
            foreach (var kv in _tablaClientes)
                yield return kv.Value;
        }

        public Cliente? ObtenerPorId(string id)
        {
            // Intenta obtener el cliente, devuelve null si no existe
            if (_tablaClientes.TryGetValue(id, out var cliente))
                return cliente;
            return null;
        }

        public void Crear(Cliente cliente)
        {
            // Lanza si ya existe la clave
            if (_tablaClientes.TryGetValue(cliente.Id, out _))
                throw new ArgumentException($"Ya existe un cliente con Id {cliente.Id}");
            _tablaClientes.Add(cliente.Id, cliente);
        }

        public bool Actualizar(string id, Cliente cliente)
        {
            // Solo actualiza si existe previamente
            if (!_tablaClientes.TryGetValue(id, out _))
                return false;
            _tablaClientes[id] = cliente;  // Usa el indexer para reemplazar el valor
            return true;
        }

        public bool Eliminar(string id)
        {
            // Intenta eliminar y retorna si tuvo éxito
            return _tablaClientes.Remove(id);
        }
    }
}
