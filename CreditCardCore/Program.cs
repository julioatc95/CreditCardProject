using Core.Library.Data;
using Core.Library.DataStructures;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ——————— 1. Carga de datos iniciales ———————
var clientesIniciales = JsonLoader.CargarClientes();       // lee clientes.json
var tarjetasIniciales = JsonLoader.CargarTarjetas();       // lee tarjetas.json
var transaccionesIniciales = JsonLoader.CargarTransacciones();  // lee transacciones.json

// ——————— 2. Registro de servicios en el contenedor DI ———————
builder.Services.AddSingleton<IClienteService>(new ClienteService(clientesIniciales));
builder.Services.AddSingleton<ITarjetaService>(new TarjetaService(tarjetasIniciales));
// Si más adelante creas ITransaccionService, lo registras igual…

// ——————— 3. MVC y Swagger/OpenAPI ———————
builder.Services.AddControllers();  // discovery de Controllers con [ApiController]
builder.Services.AddOpenApi();      // Swagger / OpenAPI

var app = builder.Build();

// ——————— 4. Middlewares ———————
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();         // UI de Swagger en /openapi o /swagger
}

app.UseHttpsRedirection();

app.MapControllers();        // Mapea tus Controllers con rutas [HttpGet], [HttpPost], etc.

// ——————— 5. Minimal API de prueba ———————
// (solo para ver en consola o devolver la lista enlazada)
app.MapGet("/api/movimientos", () =>
{
    var lista = new SinglyLinkedList<Transaccion>();
    foreach (var t in transaccionesIniciales)
        lista.AddLast(t);
    return lista;
})
.WithName("GetMovimientos");

app.Run();
