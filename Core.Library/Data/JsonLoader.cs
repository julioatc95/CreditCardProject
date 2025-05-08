using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Core.Library.Models;

namespace Core.Library.Data
{
    public static class JsonLoader
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private static string GetPath(string archivo) =>
            Path.Combine(Directory.GetCurrentDirectory(), "Data", archivo);

        public static List<Cliente> CargarClientes(string archivo = "datosIniciales.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el archivo JSON: {path}");

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Cliente>>(json, _options)
                   ?? new List<Cliente>();
        }

        public static List<Tarjeta> CargarTarjetas(string archivo = "datosIniciales.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el archivo JSON: {path}");

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Tarjeta>>(json, _options)
                   ?? new List<Tarjeta>();
        }

        public static List<Transaccion> CargarTransacciones(string archivo = "datosIniciales.json")
        {
            var path = GetPath(archivo);
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el archivo JSON: {path}");

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Transaccion>>(json, _options)
                   ?? new List<Transaccion>();
        }
    }
}
