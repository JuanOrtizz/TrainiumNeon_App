using System.Net.Http.Json;
using System.Text.Json;
using TrainiumNeon.Models;

namespace TrainiumNeon.Services
{
    public class ApiEjerciciosService : IApiEjerciciosService
    {

        //Variable para el HttpClient
        private readonly HttpClient _httpClient;

        //Constructor
        public ApiEjerciciosService(HttpClient http)
        {
            // Inicializo el HttpClient
            _httpClient = http;

            // Configuro la URL base del HttpClient
            _httpClient.BaseAddress = new Uri("https://68dafade23ebc87faa31b9f9.mockapi.io/");
        }

        //Metodo de la interfaz para obtener un ejercicio por su ID
        public async Task<EjercicioModel> GetEjercicioByIdAsync(int id)
        {
            //Try catch para manejar errores
            try
            {
                var response = await _httpClient.GetAsync($"ejercicios/{id}");
                return await ProcesarRespuestaAsyncApi<EjercicioModel>(response, "No se encontró el ejercicio. Intentá de nuevo");
            }
            catch (HttpRequestException)
            {
                throw new Exception("Error de red al obtener el ejercicio. Verificá tu conexión o intenta nuevamente.");
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
                throw new Exception($"Error inesperado al obtener el ejercicio, {ex.Message}");
            }
        }

        //Metodo de la interfaz para obtener todos los ejercicios
        public async Task<IReadOnlyList<EjercicioModel>> GetEjerciciosAsync()
        {
            //Try catch para manejar errores
            try
            {
                var response = await _httpClient.GetAsync("ejercicios");
                return await ProcesarRespuestaAsyncApi<List<EjercicioModel>>(response, "No se encontraron los ejercicios. Intentá de nuevo");
            }
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
            catch (Exception)
            {
                throw new Exception("Error inesperado al obtener los ejercicios");
            }
        }

        //Metodo de la interfaz para obtener ejercicios por grupo muscular
        public async Task<IReadOnlyList<EjercicioModel>> GetEjerciciosByGrupoMuscularAsync(string grupoMuscular)
        {
            //Try catch para manejar errores
            try
            {
                var response = await _httpClient.GetAsync($"ejercicios?grupoMuscular={grupoMuscular}");
                return await ProcesarRespuestaAsyncApi<List<EjercicioModel>>(response, "No se encontraron los ejercicios. Intentá de nuevo");
            }
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
                throw new Exception("Error inesperado al obtener los ejercicios" + ex.Message);
            }
        }

    //Task privada para reutilizar codigo
    private async Task<T> ProcesarRespuestaAsyncApi<T>(HttpResponseMessage response, string error404Mensaje)
        {
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound: // 404
                        throw new Exception(error404Mensaje);

                    case System.Net.HttpStatusCode.BadRequest: // 400
                        throw new Exception("Solicitud inválida. Verificá los parámetros enviados");

                    case System.Net.HttpStatusCode.Unauthorized: // 401
                        throw new Exception("No autorizado. Verificá tus credenciales");

                    case System.Net.HttpStatusCode.InternalServerError: // 500
                        throw new Exception("Error del servidor. Intenta nuevamente más tarde");

                    default: // Cualquier otro codigo http
                        throw new Exception($"Error inesperado. Intentá de nuevo");
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
