using Core.Library.Data;
using Core.Library.DataStructures;
using Core.Library.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ———————– Carga de datos ———————–
var clientes = JsonLoader.CargarClientes();
Console.WriteLine($"⚙️  Cargados {clientes.Count} clientes");

var transacciones = JsonLoader.CargarTransacciones();
Console.WriteLine($"⚙️  Cargadas {transacciones.Count} transacciones");

// Poblamos la lista enlazada de Transacciones
var listaMovimientos = new SinglyLinkedList<Transaccion>();
foreach (var t in transacciones)
{
    listaMovimientos.AddLast(t);
}
Console.WriteLine($"👉 Movimientos en lista enlazada: {listaMovimientos.Count}");
// ————————————————————————————————

// Prueba de pila: historial reciente (últimas 2 transacciones)
var pilaReciente = new Core.Library.DataStructures.Stack<Transaccion>();

foreach (var t in transacciones)
    pilaReciente.Push(t);

// Veamos el tope y cuántos hay
Console.WriteLine($"🗂️  1er elemento de la pila: {pilaReciente.Peek().Id}");
Console.WriteLine($"🗂️  Tamaño de la pila: {pilaReciente.Count}");

// ———————– Prueba de cola: procesamiento FIFO ———————–
var colaPendientes = new Core.Library.DataStructures.Queue<Transaccion>();
foreach (var t in transacciones)
    colaPendientes.Enqueue(t);

// Veamos el primer elemento sin sacarlo
Console.WriteLine($"⏳ Primer en cola: {colaPendientes.Peek().Id}");
Console.WriteLine($"⏳ Tamaño de la cola: {colaPendientes.Count}");

// Sacamos uno y mostramos el nuevo tope
var procesado = colaPendientes.Dequeue();
Console.WriteLine($"✅ Procesado: {procesado.Id}");
Console.WriteLine($"⏳ Nuevo tope en cola: {colaPendientes.Peek().Id}");
Console.WriteLine($"⏳ Tamaño restante: {colaPendientes.Count}");
// ————————————————————————————————————————————————

// ———————– Endpoints ———————–
app.MapGet("/api/clientes", () => clientes);
app.MapGet("/api/transacciones", () => transacciones);
app.MapGet("/api/movimientos", () => listaMovimientos);
// ———————————————————————————

// Ejecuta la aplicación
app.Run();
