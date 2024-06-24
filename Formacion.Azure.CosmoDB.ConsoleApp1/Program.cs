﻿using Microsoft.Azure.Cosmos;

namespace Formacion.Azure.CosmoDB.ConsoleApp1
{
    internal class Program
    {
        static readonly string endpointCosmosDB = "https://demodbbcr.documents.azure.com:443/";
        static readonly string keyCosmosDB = "raCAln1cL6SdeEiHEUkQGPiAm1Pu4VOyHXDWMmRFcQ79sqmii2m0FeHEoSOfPPoD8LMKgQKlXlzTACDbmUHSgA==";

        static CosmosClient clientCosmosDB;

        static void Main(string[] args)
        {
            clientCosmosDB = new CosmosClient(endpointCosmosDB, keyCosmosDB);

            Console.Clear();
            GetDatabases();
        }

        static void GetDatabases()
        {
            var resultIterator = clientCosmosDB.GetDatabaseQueryIterator<DatabaseProperties>();

            while (resultIterator.HasMoreResults)
            { 
                var allProperties = resultIterator.ReadNextAsync().Result;
                foreach (var property in allProperties)
                {
                    Console.WriteLine($"Base de datos: {property.Id}");
                    GetContainers(property.Id);
                }
            }
        }

        static void GetContainers(string databaseName)
        {
            Database clientDatabase = clientCosmosDB.GetDatabase(databaseName);

            var resultIterator = clientDatabase.GetContainerQueryIterator<ContainerProperties>();

            while (resultIterator.HasMoreResults)
            {
                var allProperties = resultIterator.ReadNextAsync().Result;
                foreach (var property in allProperties)
                {
                    Console.WriteLine($" -> {property.Id}");
                }
            }
        }
    }
}
