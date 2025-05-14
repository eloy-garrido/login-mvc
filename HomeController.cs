using calculadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            
            // Configuramos el cliente HTTP con las credenciales de Supabase
            _httpClient = httpClientFactory.CreateClient();
            
            // Obtenemos la URL y la clave API de Supabase desde appsettings.json
            string url = _configuration["Supabase:Url"] ?? "";
            string key = _configuration["Supabase:Key"] ?? "";
            
            // Si la URL no termina con /, la agregamos para asegurar que las rutas se formen correctamente
            if (!string.IsNullOrEmpty(url) && !url.EndsWith("/"))
            {
                url += "/";
            }
            
            // Configuramos el HttpClient con la URL base y los encabezados de autenticación
            if (!string.IsNullOrEmpty(url)) {
                _httpClient.BaseAddress = new Uri(url);
            } else {
                _logger.LogWarning("La URL de Supabase no está configurada en appsettings.json");
            }
            _httpClient.DefaultRequestHeaders.Add("apikey", key);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Intentando verificar la conexión a Supabase");
                
                // Verificar si la URL base está configurada
                if (_httpClient.BaseAddress == null)
                {
                    ViewBag.Error = "Error: La URL de Supabase no está configurada correctamente";
                    return View();
                }
                
                // Hacemos una solicitud simple a la API REST de Supabase
                // La ruta '/rest/v1/' es un endpoint público que debe estar disponible si Supabase está funcionando
                var response = await _httpClient.GetAsync("rest/v1/");
                
                if (response.IsSuccessStatusCode)
                {
                    // Si la respuesta es exitosa (código 200-299), la conexión está funcionando
                    _logger.LogInformation("Conexión exitosa a Supabase");
                    ViewBag.ConexionExitosa = "Conexión a Supabase establecida con éxito";
                    ViewBag.StatusCode = response.StatusCode;
                }
                else
                {
                    // Si recibimos un código de error, registramos la información y la mostramos al usuario
                    _logger.LogWarning($"Error al conectar a Supabase: {response.StatusCode}");
                    ViewBag.Error = $"Error al conectar a Supabase: {response.StatusCode}";
                    ViewBag.StatusCode = response.StatusCode;
                    
                    // Intentamos leer más detalles sobre el error
                    var content = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorDetail = content;
                }
            }
            catch (Exception ex)
            {
                // Capturamos cualquier excepción que pueda ocurrir (problemas de red, URL incorrecta, etc.)
                _logger.LogError(ex, "Error al conectar a Supabase");
                ViewBag.Error = "Error al conectar: " + ex.Message;
            }

            // Renderizamos la vista Index con la información del estado de la conexión
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}