using StorageRestApiAuth;
using System.Globalization;
using System.Xml.Linq;

namespace Formacion.Azure.Storage.ConsoleApp1
{
    internal class Program
    {
        static string storageName = "";
        static string storageKey = "";

        static void Main(string[] args)
        {
        }

        static void GetListContainers()
        { 
            HttpClient http = new HttpClient();
            string url = $"https://{storageName}.blob.core.windows.net/?comp=list";

            //string url = $"https://{storageName}.blob.core.windows.net/?comp=list&include=system,deleted";

            ////////////////////////////////////////////////////////////////////
            // Mensaje de petición al API
            ////////////////////////////////////////////////////////////////////
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = null;

            DateTime fecha = DateTime.UtcNow;

            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2021-12-02");
            request.Headers.Authorization = AzureStorageAuthenticationHelper
                .GetAuthorizationHeader(storageName, storageKey, fecha, request);

            ////////////////////////////////////////////////////////////////////
            // Mensaje de respuesta del API
            ////////////////////////////////////////////////////////////////////
            HttpResponseMessage response = http.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                XElement xml = XElement.Parse(responseData);
                foreach (var element in xml.Element("Containers").Elements("Container"))
                { 
                    Console.WriteLine($"Nombre del Contenedor: {element.Element("Name").Value}");
                }
            }
            else Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
}
