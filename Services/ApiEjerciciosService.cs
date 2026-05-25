using System.Net.Http.Json;
using System.Text.Json;
using TrainiumNeon.Models;

namespace TrainiumNeon.Services
{
    public class ApiEjerciciosService : IApiEjerciciosService
    {

        //Propiedad privada
        private readonly HttpClient _httpClient;

        //Constructor
        public ApiEjerciciosService(HttpClient http)
        {
            // Inicializa HttpClient por DI
            _httpClient = http;

            // Configuracion de la URL base del HttpClient
            _httpClient.BaseAddress = new Uri("https://68dafade23ebc87faa31b9f9.mockapi.io/");
        }

        // Task asincrona para obtener todos los ejercicios de la API
        public async Task<IReadOnlyList<EjercicioModel>> GetEjerciciosAsync()
        {
            //Try catch para manejar errores
            try
            {
                // Capturo la respuesta de la api
                var response = await _httpClient.GetAsync("ejercicios");
                // Proceso la respuesta
                return await ProcesarRespuestaAsyncApi<List<EjercicioModel>>(response);
            }// Capturo y lanzo nuevas excepciones para manejarlas desde el servicio de sincronizacion
            catch (HttpRequestException)
            {
                throw new Exception("Error de red al obtener los ejercicios. Verificá tu conexión o intenta nuevamente.");
            }
            catch (NotSupportedException)
            {
                throw new Exception("El formato de respuesta por la API no es soportado");
            }
            catch (JsonException)
            {
                throw new Exception("No se pudo interpretar la respuesta de la API");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }


        //Task privada para reutilizar codigo
        private async Task<T> ProcesarRespuestaAsyncApi<T>(HttpResponseMessage response)
        {
            // Si la respuesta no es exitosa analizo el codigo de Error y lanzo la excepcion
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound: // 404
                        throw new Exception("No se encontraron los ejercicios. Intentá mas tarde.");

                    case System.Net.HttpStatusCode.BadRequest: // 400
                        throw new Exception("Solicitud inválida. Verificá los parámetros enviados.");

                    case System.Net.HttpStatusCode.Unauthorized: // 401
                        throw new Exception("No autorizado. Verificá tus credenciales.");

                    case System.Net.HttpStatusCode.InternalServerError: // 500
                        throw new Exception("Error del servidor. Intentá mas tarde.");

                    default: // Cualquier otro codigo http
                        throw new Exception($"Error inesperado. Intentá mas tarde.");
                }
            }

            // Si la respueesta es exitosa (200 o 204)
            var contenido = await response.Content.ReadFromJsonAsync<T>();
            if (contenido == null) // 204
            {
                throw new Exception("No se pudo interpretar la respuesta de la API");
            }
                
            // Si es 200 devuelve en contenido (Ejercicio o lista de ejercicios)
            return contenido;
        }
    } 
}
