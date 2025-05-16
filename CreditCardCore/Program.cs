using Core.Library.Data;
using Core.Library.DataStructures;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ——————— 1. Carga de datos iniciales ———————
var clientesIniciales = JsonLoader.CargarClientes();
var tarjetasIniciales = JsonLoader.CargarTarjetas();
var transaccionesIniciales = JsonLoader.CargarTransacciones();

// ——————— 2. Registro de servicios en DI ———————
builder.Services.AddSingleton<IClienteService>(
    new ClienteService(clientesIniciales));
builder.Services.AddSingleton<ITarjetaService>(
    new TarjetaService(tarjetasIniciales));
builder.Services.AddSingleton<ITransaccionService>(
    new TransaccionService(transaccionesIniciales));

// ——————— 3. Añadimos Controllers y Swagger/OpenAPI ———————
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ——————— 4. Middlewares ———————
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();      // Swagger UI (Development only)
}

app.UseHttpsRedirection();

// ——————— 5. Mapeo de Controllers ———————
app.MapControllers();

// ——————— 6. Minimal API de prueba (movimientos) ———————
app.MapGet("/api/movimientos", () =>
{
    var lista = new SinglyLinkedList<Transaccion>();
    foreach (var t in transaccionesIniciales)
        lista.AddLast(t);
    return lista;
})
.WithName("GetMovimientos");

app.Run();
