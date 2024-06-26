using Azure;
using Azure.Data.Tables;

namespace Formacion.Azure.Tables.ConsoleApp1
{
    internal class Program
    {
        static readonly string connection = "escribe tu cadena de conexión";

        static void Main(string[] args)
        {
            // Cliente del Servicio de Tables
            var clientService = new TableServiceClient(connection);

            // Añadir una nueva tabla si no existe
            clientService.CreateTableIfNotExists("clientes");

            // Borrar una tabla
            clientService.CreateTable("clientes");

            // Listar tables
            var tables = clientService.Query();
            foreach ( var table in tables ) Console.WriteLine($"Tabla: {table.Name}");


            // Cliente de una Table
            var clientTables = new TableClient(connection, "productos");

            // Anadir un producto
            var producto = new Producto2()
            {
                RowKey = "11",
                PartitionKey = "Bebidas",
                id = "11",
                referencia = "11",
                categoria = "Bebidas",
                descripción = "Refresco Cola 1L",
                cantidad = 54,
                precio = 2.60
            };

            clientTables.AddEntity(producto);
            //Console.WriteLine("Producto insertado correctamente");

            // Consultas

            // Listado Completo
            var resultado = clientTables.Query<Producto2>();
            foreach(var item in resultado ) Console.WriteLine($"{item.RowKey}# {item.descripción}");
            Console.WriteLine("");

            // Listado producto con precio igual o mayor a 2
            var resultado2 = clientTables.Query<Producto2>("precio ge 2");
            foreach (var item in resultado2) Console.WriteLine($"{item.RowKey}# {item.descripción}");
            Console.WriteLine("");

            // Listado producto con precio igual o mayor a 2 con LINQ
            var resultado3 = clientTables.Query<Producto2>(r => r.precio >= 2);
            foreach (var item in resultado3) Console.WriteLine($"{item.RowKey}# {item.descripción}");

            // Borrar un item
            clientTables.DeleteEntity("Bebidas", "11");
            Console.WriteLine("Producto eliminado correctamente");

        }
    }

    public class Producto
    {
        public string id { get; set; }
        public string referencia { get; set; }
        public string categoria { get; set; }
        public string descripción { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
    }

    public class Producto2 : ITableEntity
    {
        public string id { get; set; }
        public string referencia { get; set; }
        public string categoria { get; set; }
        public string descripción { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
