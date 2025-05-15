using Core.Library.Data;
using Core.Library.DataStructures;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de aplicación
var clientesIniciales = JsonLoader.CargarClientes();
builder.Services.AddSingleton<IClienteService>(new ClienteService(clientesIniciales));

// Aquí podrías registrar TarjetaService, TransaccionService, etc.

builder.Services.AddOpenApi();

var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Cargar datos y poblar estructuras
var clientes = clientesIniciales;
Console.WriteLine($"⚙️  Cargados {clientes.Count()} clientes");

var transacciones = JsonLoader.CargarTransacciones();
Console.WriteLine($"⚙️  Cargadas {transacciones.Count} transacciones");

var listaMovimientos = new SinglyLinkedList<Transaccion>();
foreach (var t in transacciones)
    listaMovimientos.AddLast(t);
Console.WriteLine($"👉 Movimientos en lista enlazada: {listaMovimientos.Count}");

// Usamos la pila de nuestra librería DataStructures calificada para evitar ambigüedad
var pilaReciente = new Core.Library.DataStructures.Stack<Transaccion>();
foreach (var t in transacciones)
    pilaReciente.Push(t);
Console.WriteLine($"🗂️  1er elemento de la pila: {pilaReciente.Peek().Id}");
Console.WriteLine($"🗂️  Tamaño de la pila: {pilaReciente.Count}");

// Usamos la cola de nuestra librería DataStructures calificada para evitar ambigüedad
var colaPendientes = new Core.Library.DataStructures.Queue<Transaccion>();
foreach (var t in transacciones)
    colaPendientes.Enqueue(t);
Console.WriteLine($"⏳ Primer en cola: {colaPendientes.Peek().Id}");
Console.WriteLine($"⏳ Tamaño de la cola: {colaPendientes.Count}");
var procesado = colaPendientes.Dequeue();
Console.WriteLine($"✅ Procesado: {procesado.Id}");
Console.WriteLine($"⏳ Nuevo tope en cola: {colaPendientes.Peek().Id}");
Console.WriteLine($"⏳ Tamaño restante: {colaPendientes.Count}");

var tablaClientes = new HashTable<string, Cliente>();
foreach (var c in clientes)
    tablaClientes.Add(c.Id, c);
if (tablaClientes.TryGetValue(clientes.First().Id, out var cli))
    Console.WriteLine($"🔍 Cliente recuperado: {cli.Nombre} ({cli.Id})");
Console.WriteLine($"🔑 Indexer: {tablaClientes[clientes.First().Id].Email}");
tablaClientes.Remove(clientes.First().Id);
Console.WriteLine($"❌ Eliminado {clientes.First().Id}, Count ahora = {tablaClientes.Count}");

var bstClientes = new BinarySearchTree<string>();
foreach (var c in clientes)
    bstClientes.Insert(c.Id);
Console.WriteLine("🌳 IDs de clientes en orden: " + string.Join(", ", bstClientes));

var avl = new AvlTree<string>();
foreach (var c in clientes)
    avl.Insert(c.Id);
Console.WriteLine("🌲 IDs en AVL ordenado: " + string.Join(", ", avl));

// Endpoints básicos
app.MapGet("/api/clientes", () => clientes);
app.MapGet("/api/transacciones", () => transacciones);
app.MapGet("/api/movimientos", () => listaMovimientos);

app.Run();
