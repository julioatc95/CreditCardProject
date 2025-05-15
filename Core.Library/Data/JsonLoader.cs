using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;      // <-- aquí
using Core.Library.Models;

namespace Core.Library.Data
{
    public static class JsonLoader
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        static JsonLoader()
        {
            // Necesario para que "Pago"/"Consumo" en el JSON se conviertan
            // al enum TipoTransaccion.Pago, etc.
            _options.Converters.Add(new JsonStringEnumConverter());
        }



        private static string GetPath(string archivo) =>
            Path.Combine(Directory.GetCurrentDirectory(), "Data", archivo);

        public static List<Cliente> CargarClientes(string archivo = "clientes.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo no existe: {path}");
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Cliente>>(json, _options)
                   ?? new List<Cliente>();
        }

        public static List<Tarjeta> CargarTarjetas(string archivo = "tarjetas.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo no existe: {path}");
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Tarjeta>>(json, _options)
                   ?? new List<Tarjeta>();
        }

        public static List<Transaccion> CargarTransacciones(string archivo = "transacciones.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo no existe: {path}");
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Transaccion>>(json, _options)
                   ?? new List<Transaccion>();
        }
    }
}
