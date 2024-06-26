﻿using Azure.Storage.Blobs;

namespace Formacion.Azure.Storage.ConsoleApp2
{
    internal class Program
    {
        static string connectionString = "inserta tu cadena de conexión";
        static BlobServiceClient client;

        static void Main(string[] args)
        {
            // Configurar el cliente
            client = new BlobServiceClient(connectionString);

            // Listado de Contenedores
            var contenedores = client.GetBlobContainers();
            foreach (var container in contenedores)
            {
                Console.WriteLine($"Nombre del contenedor: {container.Name}");

                // Listado de Blobs
                var containerClient = client.GetBlobContainerClient(container.Name);
                var blobs = containerClient.GetBlobs();

                foreach (var blob in blobs)
                {
                    Console.WriteLine($"  > {blob.Name}");
                    Console.WriteLine($"    - {blob.Properties.ContentType}");

                    var blobClient = containerClient.GetBlobClient(blob.Name);
                    blobClient.DownloadTo(@$"C:\Formación_EOI\{blob.Name}");
                }
            }
        }
    }
}