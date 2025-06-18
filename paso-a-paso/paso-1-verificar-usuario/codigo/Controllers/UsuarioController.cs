using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VerificarUsuario.Models;

namespace VerificarUsuario.Controllers
{
    /// <summary>
    /// Controlador que maneja la verificación de usuarios
    /// Usa el mismo patrón que HomeController para conectar con Supabase
    /// </summary>
    public class UsuarioController : Controller
    {
        // Variables privadas para manejar la conexión con Supabase
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly ILogger<UsuarioController> _logger;

        /// <summary>
        /// Constructor: configura la conexión con Supabase
        /// IGUAL al patrón usado en HomeController
        /// </summary>
        public UsuarioController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<UsuarioController> logger)
        {
            _logger = logger;
            
            // Crear HttpClient usando el factory (mismo patrón que HomeController)
            _httpClient = httpClientFactory.CreateClient();
            
            // Obtener configuraciones de Supabase desde appsettings.json
            _supabaseUrl = configuration["Supabase:Url"] ?? string.Empty;
            _supabaseKey = configuration["Supabase:Key"] ?? string.Empty;
            
            // Configurar headers para Supabase (IGUAL que HomeController)
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
            
            _logger.LogInformation("UsuarioController configurado con Supabase URL: {Url}", _supabaseUrl);
        }

        /// <summary>
        /// GET: /Usuario/Verificar
        /// Muestra el formulario para verificar usuario
        /// </summary>
        [HttpGet]
        public IActionResult Verificar()
        {
            // Crear un modelo vacío para el formulario
            var model = new VerificarUsuarioViewModel();
            return View(model);
        }

        /// <summary>
        /// POST: /Usuario/Verificar
        /// Procesa el formulario y verifica si el usuario existe
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF
        public async Task<IActionResult> Verificar(VerificarUsuarioViewModel model)
        {
            // Verificar si el modelo es válido (validaciones de DataAnnotations)
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, mostrar el formulario nuevamente
                return View(model);
            }

            try
            {
                _logger.LogInformation("Verificando usuario: {Username}", model.Username);
                
                // Intentar buscar el usuario en Supabase
                var usuarioExiste = await VerificarUsuarioExisteAsync(model.Username);
                
                if (usuarioExiste)
                {
                    // ✅ Usuario encontrado
                    model.UsuarioEncontrado = true;
                    model.Mensaje = $"✅ ¡Usuario '{model.Username}' encontrado en el sistema!";
                    _logger.LogInformation("Usuario encontrado: {Username}", model.Username);
                }
                else
                {
                    // ❌ Usuario no existe
                    model.UsuarioEncontrado = false;
                    model.Mensaje = $"❌ Usuario '{model.Username}' no existe en el sistema.";
                    _logger.LogInformation("Usuario no encontrado: {Username}", model.Username);
                }

                // Pasar datos al resultado y redirigir
                TempData["Username"] = model.Username;
                TempData["UsuarioEncontrado"] = model.UsuarioEncontrado;
                TempData["Mensaje"] = model.Mensaje;
                
                return RedirectToAction("Resultado");
            }
            catch (Exception ex)
            {
                // Error del sistema: mostrar mensaje genérico
                _logger.LogError(ex, "Error al verificar usuario: {Username}", model.Username);
                model.Mensaje = "❌ Error del sistema. Intenta nuevamente.";
                return View(model);
            }
        }

        /// <summary>
        /// GET: /Usuario/Resultado
        /// Muestra el resultado de la verificación
        /// </summary>
        [HttpGet]
        public IActionResult Resultado()
        {
            // Verificar si tenemos datos de la verificación
            if (TempData["Username"] == null)
            {
                // Si no hay datos, redirigir al formulario
                return RedirectToAction("Verificar");
            }

            // Crear modelo con los datos del TempData
            var model = new VerificarUsuarioViewModel
            {
                Username = TempData["Username"]?.ToString() ?? "",
                UsuarioEncontrado = (bool)(TempData["UsuarioEncontrado"] ?? false),
                Mensaje = TempData["Mensaje"]?.ToString() ?? ""
            };
            
            return View(model);
        }

        /// <summary>
        /// MÉTODO PRIVADO: Verificar si el usuario existe en Supabase
        /// Usa HttpClient para consultar la API REST de Supabase
        /// </summary>
        private async Task<bool> VerificarUsuarioExisteAsync(string username)
        {
            try
            {
                // Construir URL para filtrar por username
                // eq significa "equal" (igual) en la API de Supabase
                string url = $"{_supabaseUrl}/rest/v1/users?username=eq.{username}";
                
                _logger.LogInformation("Consultando Supabase: {Url}", url);
                
                // Hacer petición GET a Supabase (igual que en HomeController)
                var response = await _httpClient.GetAsync(url);
                
                // Verificar si la petición fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    _logger.LogInformation("Respuesta de Supabase: {Response}", responseContent);
                    
                    // Deserializar JSON a array de UserModel
                    var users = JsonSerializer.Deserialize<UserModel[]>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower // Supabase usa snake_case
                    });
                    
                    // Si el array tiene elementos, el usuario existe
                    bool existe = users != null && users.Length > 0;
                    
                    _logger.LogInformation("Usuario {Username} existe: {Existe}", username, existe);
                    
                    return existe;
                }
                else
                {
                    _logger.LogWarning("Error en petición a Supabase: {StatusCode}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar Supabase para usuario: {Username}", username);
                return false;
            }
        }
    }
}
