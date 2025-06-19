using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LoginSimple.Models;

namespace LoginSimple.Controllers
{
    /// <summary>
    /// Controlador de autenticación - EVOLUCIÓN del UsuarioController del Paso 1
    /// NUEVAS FUNCIONALIDADES: Validación de username + password, dashboard, logout
    /// </summary>
    public class AuthController : Controller
    {
        // Variables privadas (IGUALES al Paso 1)
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor: EXACTAMENTE IGUAL al Paso 1
        /// La configuración de Supabase no cambia
        /// </summary>
        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _logger = logger;
            
            // Crear HttpClient usando el factory (mismo patrón que Paso 1)
            _httpClient = httpClientFactory.CreateClient();
            
            // Obtener configuraciones de Supabase desde appsettings.json (IGUAL que Paso 1)
            _supabaseUrl = configuration["Supabase:Url"] ?? string.Empty;
            _supabaseKey = configuration["Supabase:Key"] ?? string.Empty;
            
            // Configurar headers para Supabase (IGUAL que Paso 1)
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
            
            _logger.LogInformation("AuthController configurado con Supabase URL: {Url}", _supabaseUrl);
        }

        /// <summary>
        /// GET: /Auth/Login
        /// EVOLUCIÓN de /Usuario/Verificar del Paso 1
        /// Ahora muestra formulario de login con username + password
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // Crear un modelo vacío para el formulario (IGUAL lógica que Paso 1)
            var model = new LoginViewModel();
            return View(model);
        }

        /// <summary>
        /// POST: /Auth/Login
        /// EVOLUCIÓN MAYOR: Ahora valida username + password (no solo existencia)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF (IGUAL que Paso 1)
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Verificar si el modelo es válido (IGUAL lógica que Paso 1)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _logger.LogInformation("Intentando login para usuario: {Username}", model.Username);
                
                // 🆕 NUEVA LÓGICA: Validar credenciales completas (username + password)
                var user = await ValidateCredentialsAsync(model);
                
                if (user != null)
                {
                    // ✅ Login exitoso: redirigir al dashboard
                    TempData["Username"] = user.Username;
                    TempData["Email"] = user.Email;
                    TempData["WelcomeMessage"] = $"¡Bienvenido de vuelta, {user.Username}!";
                    TempData["LoginTime"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    
                    _logger.LogInformation("Login exitoso para usuario: {Username}", model.Username);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // ❌ Credenciales incorrectas
                    model.ErrorMessage = "Usuario o contraseña incorrectos";
                    _logger.LogWarning("Login fallido para usuario: {Username}", model.Username);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Error del sistema
                _logger.LogError(ex, "Error durante login para usuario: {Username}", model.Username);
                model.ErrorMessage = "Error del sistema. Intenta nuevamente.";
                return View(model);
            }
        }

        /// <summary>
        /// GET: /Auth/Dashboard
        /// NUEVA FUNCIONALIDAD: Página de bienvenida para usuarios logueados
        /// Evolución de /Usuario/Resultado del Paso 1
        /// </summary>
        [HttpGet]
        public IActionResult Dashboard()
        {
            // Verificar si tenemos datos del usuario logueado (IGUAL lógica que Paso 1)
            if (TempData["Username"] == null)
            {
                // Si no hay datos, redirigir al login
                return RedirectToAction("Login");
            }

            // Pasar datos a la vista usando ViewBag (IGUAL patrón que Paso 1)
            ViewBag.Username = TempData["Username"];
            ViewBag.Email = TempData["Email"];
            ViewBag.WelcomeMessage = TempData["WelcomeMessage"];
            ViewBag.LoginTime = TempData["LoginTime"];
            
            return View();
        }

        /// <summary>
        /// GET: /Auth/Logout
        /// NUEVA FUNCIONALIDAD: Cerrar sesión y volver al login
        /// </summary>
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpiar todos los datos temporales
            TempData.Clear();
            
            // Agregar mensaje de logout exitoso
            TempData["LogoutMessage"] = "Has cerrado sesión correctamente.";
            
            _logger.LogInformation("Usuario cerró sesión");
            
            // Redirigir al login
            return RedirectToAction("Login");
        }

        /// <summary>
        /// MÉTODO PRIVADO: Validar credenciales completas (username + password)
        /// EVOLUCIÓN MAYOR del método del Paso 1 que solo verificaba existencia
        /// </summary>
        private async Task<UserModel?> ValidateCredentialsAsync(LoginViewModel model)
        {
            try
            {
                // 1. Buscar el usuario por username (IGUAL lógica que Paso 1)
                var user = await GetUserByUsernameAsync(model.Username);
                
                // 2. Si no se encuentra el usuario, retornar null (IGUAL que Paso 1)
                if (user == null)
                {
                    _logger.LogInformation("Usuario no encontrado: {Username}", model.Username);
                    return null;
                }

                // 3. 🆕 NUEVA LÓGICA: Verificar la contraseña
                // NOTA: En este paso usamos comparación de texto plano
                // En el Paso 3 implementaremos hashing seguro con BCrypt
                bool isPasswordValid = (model.Password == user.Password);
                
                _logger.LogInformation("Validación de contraseña para {Username}: {IsValid}", 
                    model.Username, isPasswordValid);
                
                // 4. Si username Y password son correctos, retornar el usuario
                if (isPasswordValid)
                {
                    return user;
                }

                // 5. Si la contraseña es incorrecta, retornar null
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar credenciales para usuario: {Username}", model.Username);
                return null;
            }
        }

        /// <summary>
        /// MÉTODO PRIVADO: Buscar usuario por username (EXACTAMENTE IGUAL al Paso 1)
        /// Esta lógica no cambia, solo agregamos el campo password al modelo
        /// </summary>
        private async Task<UserModel?> GetUserByUsernameAsync(string username)
        {
            try
            {
                // Construir URL con filtro (IGUAL que Paso 1)
                string url = $"{_supabaseUrl}/rest/v1/users?username=eq.{username}";
                
                _logger.LogInformation("Consultando Supabase: {Url}", url);
                
                // Hacer petición GET a Supabase (IGUAL que Paso 1)
                var response = await _httpClient.GetAsync(url);
                
                // Verificar si la petición fue exitosa (IGUAL que Paso 1)
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta (IGUAL que Paso 1)
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    _logger.LogInformation("Respuesta de Supabase: {Response}", responseContent);
                    
                    // Deserializar JSON a array de UserModel (IGUAL que Paso 1)
                    // NOTA: El modelo ahora incluye el campo password automáticamente
                    var users = JsonSerializer.Deserialize<UserModel[]>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower // Supabase usa snake_case
                    });
                    
                    // Retornar el primer usuario encontrado (IGUAL que Paso 1)
                    var foundUser = users?.FirstOrDefault();
                    
                    if (foundUser != null)
                    {
                        _logger.LogInformation("Usuario encontrado: {Username}", foundUser.Username);
                    }
                    
                    return foundUser;
                }
                else
                {
                    _logger.LogWarning("Error en petición a Supabase: {StatusCode}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar Supabase para usuario: {Username}", username);
                return null;
            }
        }
    }
}
