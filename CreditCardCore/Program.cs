using Core.Library.Data;
using Core.Library.DataStructures;
using Core.Library.Models;
using Core.Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ————— 1) Carga inicial de datos —————
var clientesIniciales = JsonLoader.CargarClientes();
var tarjetasIniciales = JsonLoader.CargarTarjetas();
var transaccionesIniciales = JsonLoader.CargarTransacciones();

// ————— 2) Registro de servicios en DI —————
builder.Services.AddSingleton<IClienteService>(new ClienteService(clientesIniciales));
builder.Services.AddSingleton<ITarjetaService>(new TarjetaService(tarjetasIniciales));
builder.Services.AddSingleton<ITransaccionService>(new TransaccionService(transaccionesIniciales));

// ————— 3) Controllers + Swagger/OpenAPI —————
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  // para swagger explorer
builder.Services.AddSwaggerGen();            // Swashbuckle

var app = builder.Build();

// ————— 4) Middleware —————
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                           // /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CreditCard API V1");
        c.RoutePrefix = "swagger";              // UI en /swagger
    });
}

app.UseHttpsRedirection();

// ————— 5) Mapea tus controllers etiquetados con [ApiController] —————
app.MapControllers();

// ————— 6) (Opcional) Minimal API para lista enlazada de movimientos —————
app.MapGet("/api/movimientos", () =>
{
    var lista = new SinglyLinkedList<Transaccion>();
    foreach (var t in transaccionesIniciales)
        lista.AddLast(t);
    return lista;
})
.WithName("GetMovimientos");

app.Run();
