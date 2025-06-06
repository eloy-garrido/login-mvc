# 🔧 Solución Completa: Sistema de Login (Solo Validación)

## 📋 Introducción
Esta guía contiene la implementación completa paso a paso para crear un sistema de login que valide usuarios existentes en Supabase, usando el mismo patrón que ya conoces del HomeController.

## 🏗️ IMPORTANTE!!!!!!!!!!
Si no lo intentaste al menos por 5 horas y estas leyendo esto, estare algo decepcionado y al menos espero que en el siguiente proyecto seas lo mejor de lo mejor.
En esta solución no implemento ninguna librería... se puede hacer más sencillo con algunas, pero al mismo tiempo da más complejidad al proyecto, lo dejo a su juicio.

## 🏗️ Estructura Final del Proyecto
```
tu-proyecto/
├── Controllers/
│   ├── HomeController.cs (ya existe)
│   └── AuthController.cs (nuevo - similar al HomeController)
├── Models/
│   ├── UserModel.cs (nuevo)
│   └── LoginViewModel.cs (nuevo)
└── Views/
    └── Auth/
        ├── Login.cshtml (nuevo)
        └── Dashboard.cshtml (nuevo)
```

## 🚀 Paso 1: Crear el Modelo de Usuario

**Archivo: `Models/UserModel.cs`**

```csharp
// Este modelo representa la estructura de la tabla users en Supabase
using System.ComponentModel.DataAnnotations;

namespace tu_proyecto.Models  // 🔄 Cambia "tu_proyecto" por el nombre real de tu proyecto
{
    public class UserModel
    {
        // Corresponde a la columna 'id' en Supabase
        public int Id { get; set; }
        
        // Corresponde a la columna 'username' en Supabase
        [Required]
        public string Username { get; set; } = string.Empty;
        
        // Corresponde a la columna 'email' en Supabase
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        // Corresponde a la columna 'password_hash' en Supabase
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        // Corresponde a la columna 'created_at' en Supabase
        public DateTime CreatedAt { get; set; }
        
        // Corresponde a la columna 'last_login' en Supabase
        public DateTime? LastLogin { get; set; }
        
        // Corresponde a la columna 'is_active' en Supabase
        public bool IsActive { get; set; } = true;
    }
}
```

## 🚀 Paso 2: Crear el Modelo del Formulario de Login

**Archivo: `Models/LoginViewModel.cs`**

```csharp
// Este modelo representa los datos que el usuario ingresará en el formulario
using System.ComponentModel.DataAnnotations;

namespace tu_proyecto.Models
{
    public class LoginViewModel
    {
        // Campo para el nombre de usuario
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; } = string.Empty;

        // Campo para la contraseña
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]  // Esto hace que el campo se muestre como password
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        // Propiedad para mostrar mensajes de error personalizados
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
```

## 🚀 Paso 3: Crear el Controlador de Autenticación (Patrón Similar al HomeController)

**Archivo: `Controllers/AuthController.cs`**

```csharp
// Controlador que maneja las acciones relacionadas con autenticación
// Usa el mismo patrón que HomeController para conectar con Supabase
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using tu_proyecto.Models;

namespace tu_proyecto.Controllers
{
    public class AuthController : Controller
    {
        // Variables privadas (igual que en HomeController)
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;

        // Constructor: inyección de dependencia (IGUAL que HomeController)
        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            // Crear HttpClient usando el factory (mismo patrón)
            _httpClient = httpClientFactory.CreateClient();
            
            // Obtener configuraciones de Supabase desde appsettings.json (mismo patrón)
            _supabaseUrl = configuration["Supabase:Url"] ?? string.Empty;
            _supabaseKey = configuration["Supabase:Key"] ?? string.Empty;
            
            // Configurar headers para Supabase (IGUAL que HomeController)
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
        }

        // GET: /Auth/Login
        // Muestra el formulario de login
        [HttpGet]
        public IActionResult Login()
        {
            // Crear un modelo vacío para el formulario
            var model = new LoginViewModel();
            return View(model);
        }

        // POST: /Auth/Login
        // Procesa el formulario de login enviado
        [HttpPost]
        [ValidateAntiForgeryToken]  // Protección contra ataques CSRF
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Verificar si el modelo es válido (validaciones de DataAnnotations)
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, mostrar el formulario nuevamente
                return View(model);
            }

            try
            {
                // Intentar validar las credenciales
                var user = await ValidateUserAsync(model);
                
                if (user != null)
                {
                    // ✅ Credenciales correctas: redirigir al dashboard
                    // Pasamos el nombre de usuario al dashboard
                    TempData["Username"] = user.Username;
                    TempData["WelcomeMessage"] = $"¡Bienvenido, {user.Username}!";
                    
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // ❌ Credenciales incorrectas: mostrar error
                    model.ErrorMessage = "Usuario o contraseña incorrectos";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Error del sistema: mostrar mensaje genérico
                Console.WriteLine($"Error en Login: {ex.Message}");
                model.ErrorMessage = "Error del sistema. Intenta nuevamente.";
                return View(model);
            }
        }

        // GET: /Auth/Dashboard
        // Página de bienvenida después del login exitoso
        [HttpGet]
        public IActionResult Dashboard()
        {
            // Verificar si tenemos datos del usuario logueado
            if (TempData["Username"] == null)
            {
                // Si no hay datos, redirigir al login
                return RedirectToAction("Login");
            }

            // Mantener los datos para la vista
            ViewBag.Username = TempData["Username"];
            ViewBag.WelcomeMessage = TempData["WelcomeMessage"];
            
            return View();
        }

        // GET: /Auth/Logout
        // Cerrar sesión (simple redirección por ahora)
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpiar datos temporales
            TempData.Clear();
            
            // Redirigir al home o login
            return RedirectToAction("Index", "Home");
        }

        // MÉTODO PRIVADO: Validar credenciales (toda la lógica de Supabase aquí)
        private async Task<UserModel?> ValidateUserAsync(LoginViewModel model)
        {
            try
            {
                // 1. Buscar el usuario por username en Supabase
                var user = await GetUserByUsernameAsync(model.Username);
                
                // 2. Si no se encuentra el usuario, retornar null
                if (user == null)
                {
                    return null;
                }

                // 3. Verificar la contraseña usando BCrypt
                // BCrypt.Verify compara la contraseña en texto plano con el hash guardado
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
                
                // 4. Si la contraseña es correcta, retornar el usuario
                if (isPasswordValid)
                {
                    return user;
                }

                // 5. Si la contraseña es incorrecta, retornar null
                return null;
            }
            catch (Exception ex)
            {
                // En caso de error, loguear y retornar null
                Console.WriteLine($"Error en ValidateUserAsync: {ex.Message}");
                return null;
            }
        }

        // MÉTODO PRIVADO: Buscar usuario por username (usando HttpClient como HomeController)
        private async Task<UserModel?> GetUserByUsernameAsync(string username)
        {
            try
            {
                // Construir URL para filtrar por username
                // eq significa "equal" (igual) en la API de Supabase
                string url = $"{_supabaseUrl}/rest/v1/users?username=eq.{username}";
                
                // Hacer petición GET a Supabase (igual que en HomeController)
                var response = await _httpClient.GetAsync(url);
                
                // Verificar si la petición fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Deserializar JSON a array de UserModel
                    var users = JsonSerializer.Deserialize<UserModel[]>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower // Supabase usa snake_case
                    });
                    
                    // Retornar el primer usuario encontrado (debería ser único)
                    return users?.FirstOrDefault();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUserByUsernameAsync: {ex.Message}");
                return null;
            }
        }
    }
}
```

## 🚀 Paso 4: Crear las Vistas

### Vista de Login

**Archivo: `Views/Auth/Login.cshtml`**

```html
@model tu_proyecto.Models.LoginViewModel

@{
    ViewData["Title"] = "Iniciar Sesión";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3 class="text-center">Iniciar Sesión</h3>
                </div>
                <div class="card-body">
                    
                    @* Mostrar mensaje de error si existe *@
                    @if (!string.IsNullOrEmpty(Model.ErrorMessage))
                    {
                        <div class="alert alert-danger" role="alert">
                            @Model.ErrorMessage
                        </div>
                    }

                    @* Formulario de login *@
                    <form asp-action="Login" method="post">
                        @* Token anti-falsificación *@
                        @Html.AntiForgeryToken()
                        
                        <div class="mb-3">
                            @* Campo Username *@
                            <label asp-for="Username" class="form-label">Nombre de Usuario</label>
                            <input asp-for="Username" class="form-control" placeholder="Ingresa tu nombre de usuario" />
                            <span asp-validation-for="Username" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            @* Campo Password *@
                            <label asp-for="Password" class="form-label">Contraseña</label>
                            <input asp-for="Password" class="form-control" placeholder="Ingresa tu contraseña" />
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>

                        <div class="d-grid">
                            @* Botón de envío *@
                            <button type="submit" class="btn btn-primary">Iniciar Sesión</button>
                        </div>
                    </form>

                    @* Enlaces adicionales *@
                    <div class="text-center mt-3">
                        <a asp-controller="Home" asp-action="Index" class="text-decoration-none">
                            ← Volver al inicio
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@* Scripts para validación del lado del cliente *@
@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

### Vista de Dashboard

**Archivo: `Views/Auth/Dashboard.cshtml`**

```html
@{
    ViewData["Title"] = "Panel de Usuario";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header bg-success text-white">
                    <h3 class="text-center mb-0">✅ Login Exitoso</h3>
                </div>
                <div class="card-body text-center">
                    
                    @* Mensaje de bienvenida *@
                    <div class="alert alert-success" role="alert">
                        <h4 class="alert-heading">@ViewBag.WelcomeMessage</h4>
                        <p>Has iniciado sesión correctamente en el sistema.</p>
                    </div>

                    @* Información del usuario *@
                    <div class="mb-4">
                        <h5>Información de la Sesión:</h5>
                        <p><strong>Usuario:</strong> @ViewBag.Username</p>
                        <p><strong>Fecha y Hora:</strong> @DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")</p>
                    </div>

                    @* Acciones disponibles *@
                    <div class="d-flex justify-content-center gap-3">
                        <a asp-controller="Home" asp-action="Index" class="btn btn-primary">
                            🏠 Ir al Inicio
                        </a>
                        <a asp-action="Logout" class="btn btn-outline-secondary">
                            🚪 Cerrar Sesión
                        </a>
                    </div>
                </div>
            </div>

            @* Información adicional (opcional) *@
            <div class="card mt-4">
                <div class="card-body">
                    <h5 class="card-title">🎉 ¡Felicitaciones!</h5>
                    <p class="card-text">
                        Has implementado exitosamente un sistema de login funcional que:
                    </p>
                    <ul class="list-unstyled">
                        <li>✅ Conecta con Supabase</li>
                        <li>✅ Valida credenciales de usuario</li>
                        <li>✅ Maneja errores de forma segura</li>
                        <li>✅ Usa contraseñas hasheadas</li>
                        <li>✅ Sigue el patrón MVC
- ✅ Perfecto para aprender los conceptos fundamentales

## 🚀 Próximos Pasos (Futuras Mejoras)

Una vez que domines este login básico, podrías agregar:
1. **Sistema de registro de usuarios**
2. **Sesiones persistentes con cookies**
3. **Contraseñas hasheadas (BCrypt)**
4. **Recuperación de contraseñas**
5. **Validación de email**
6. **Roles y permisos**
7. **Remember me functionality**
8. **Logout con limpieza completa de sesión**
9. **Protección contra ataques de fuerza bruta**

## 💡 Consejos para el Futuro

### Para Seguridad en Producción:
```csharp
// Instalar BCrypt: Install-Package BCrypt.Net-Next
// Hashear contraseña al registrar:
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

// Verificar contraseña al login:
bool isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
```

### Para Proyectos Más Grandes:
1. **Separar lógica en servicios:** Crear `IAuthService` y `AuthService`
2. **Implementar logging:** Usar `ILogger<AuthController>`
3. **Agregar rate limiting:** Proteger contra ataques de fuerza bruta
4. **Usar Identity:** Framework de autenticación de ASP.NET Core

### Para Mejor UX:
1. **Indicadores de carga:** Spinner mientras se valida
2. **Mensajes más específicos:** Diferenciar entre usuario no existe vs contraseña incorrecta
3. **Recordar último usuario:** Guardar username en localStorage
4. **Redirección inteligente:** Volver a la página que intentaba acceder

## 🔒 Nota Importante sobre Seguridad

**⚠️ ADVERTENCIA:** Este ejercicio usa contraseñas en texto plano únicamente para fines educativos. En cualquier aplicación real:

1. **NUNCA** almacenes contraseñas en texto plano
2. **SIEMPRE** usa funciones de hash como BCrypt, Argon2, o PBKDF2
3. **IMPLEMENTA** validación de entrada robusta
4. **USA** HTTPS en producción
5. **PROTEGE** contra ataques de fuerza bruta
6. **CONSIDERA** autenticación de dos factores

## 🎓 Lo Que Has Aprendido

Con este ejercicio has dominado:

### Conceptos de ASP.NET Core MVC:
- ✅ Creación de controladores
- ✅ Inyección de dependencias
- ✅ Manejo de formularios POST
- ✅ Validación de modelos
- ✅ Redirecciones entre acciones
- ✅ Uso de TempData y ViewBag

### Integración con Supabase:
- ✅ Configuración de HttpClient
- ✅ Headers de autenticación
- ✅ Consultas con filtros
- ✅ Deserialización de JSON
- ✅ Manejo de errores de API

### Mejores Prácticas:
- ✅ Separación de responsabilidades (MVC)
- ✅ ViewModels para formularios
- ✅ Validación tanto del servidor como del cliente
- ✅ Manejo apropiado de errores
- ✅ Protección CSRF con tokens

## 🚀 ¡Excelente Trabajo!

¡Has implementado exitosamente tu primer sistema de autenticación! Ahora tienes una base sólida para:

- Entender cómo funciona la autenticación web
- Integrar bases de datos externas con aplicaciones MVC
- Manejar formularios y validaciones
- Seguir patrones de desarrollo profesionales

**Próximo desafío:** ¿Te animas a agregar un sistema de registro de usuarios? 😄

---

*¿Tienes dudas o errores? Revisa el troubleshooting o consulta con tu profesor. ¡Sigue practicando!* 🎉</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
```

## 🚀 Paso 5: Instalar el Paquete BCrypt

En la consola del Administrador de Paquetes de Visual Studio, ejecuta:

```bash
Install-Package BCrypt.Net-Next
```

O si usas .NET CLI:

```bash
dotnet add package BCrypt.Net-Next
```

## 🚀 Paso 6: Crear Usuarios de Prueba en Supabase

Para probar el login, necesitas usuarios en tu base de datos. Puedes crearlos ejecutando este SQL en Supabase:

```sql
-- Crear usuarios de prueba
-- La contraseña será "123456" hasheada
INSERT INTO users (username, email, password_hash, created_at, is_active)
VALUES 
('admin', 'admin@test.com', '$2a$11$8wXGYQQqL6uy0M0DFXEXi.YLJbZD7jnbXTF6P8HrXJxoG5cZBpY5i', NOW(), true),
('usuario1', 'usuario1@test.com', '$2a$11$8wXGYQQqL6uy0M0DFXEXi.YLJbZD7jnbXTF6P8HrXJxoG5cZBpY5i', NOW(), true);

-- Contraseña para ambos usuarios: "123456"
```

**Nota:** El hash `$2a$11$8wXGYQQqL6uy0M0DFXEXi.YLJbZD7jnbXTF6P8HrXJxoG5cZBpY5i` corresponde a la contraseña "123456" hasheada con BCrypt.

## 🧪 Paso 7: Probar la Aplicación

1. **Ejecuta la aplicación** (F5 en Visual Studio)
2. **Navega a:** `https://localhost:xxxx/Auth/Login`
3. **Prueba con:**
   - Usuario: `admin`
   - Contraseña: `123456`
4. **Deberías ver:** La página de dashboard con mensaje de bienvenida

## 🔧 Troubleshooting (Solución de Problemas)

### Problema: "No se puede conectar a Supabase"
**Solución:** Verifica que las credenciales en `appsettings.json` sean correctas.

### Problema: "Usuario no encontrado"
**Solución:** 
- Asegúrate de haber insertado usuarios de prueba en la base de datos
- Verifica que la tabla se llame exactamente `users`
- Revisa que el campo `username` sea único

### Problema: "Error de contraseña"
**Solución:** 
- Verifica que el hash de la contraseña sea correcto
- Asegúrate de que BCrypt esté instalado correctamente
- Prueba creando un nuevo usuario con contraseña hasheada

### Problema: "Error 404 en /Auth/Login"
**Solución:** Asegúrate de haber creado el controlador `AuthController` correctamente.

### Problema: "JsonException al deserializar"
**Solución:** 
- Verifica que los nombres de propiedades en `UserModel` coincidan con la estructura de Supabase
- Asegúrate de usar `JsonNamingPolicy.SnakeCaseLower`

## 📝 Explicación de Conceptos Clave

### 1. ¿Por qué BCrypt?
BCrypt es una función de hash segura diseñada específicamente para contraseñas. A diferencia de MD5 o SHA, BCrypt:
- Es lento por diseño (protege contra ataques de fuerza bruta)
- Incluye salt automáticamente
- Permite ajustar la complejidad

### 2. ¿Qué es TempData?
`TempData` es un diccionario que permite pasar datos entre acciones. Los datos se mantienen solo para la siguiente petición, perfecto para mensajes después de redirecciones.

### 3. ¿Por qué usar ViewModels?
Los ViewModels (como `LoginViewModel`) separan los datos del formulario del modelo de base de datos, permitiendo:
- Validaciones específicas para la vista
- Mayor seguridad (no exponemos el modelo completo)
- Flexibilidad en la presentación

### 4. Patrón de Inyección de Dependencias
El patrón usado en el constructor es inyección de dependencias:
```csharp
public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
```
ASP.NET Core automáticamente proporciona estas dependencias.

## 🎯 ¡Felicitaciones!

Si seguiste todos los pasos, ahora tienes un sistema de login funcional que:

- ✅ Valida usuarios contra Supabase usando el patrón familiar del HomeController
- ✅ Maneja contraseñas de forma segura con BCrypt  
- ✅ Muestra mensajes de error apropiados
- ✅ Redirige a una página de éxito tras login correcto
- ✅ Sigue las mejores prácticas de MVC
- ✅ No introduce complejidad innecesaria con capas de servicios

## 🚀 Próximos Pasos (Futuras Mejoras)

Una vez que domines este login básico, podrías agregar:
1. **Sistema de registro de usuarios**
2. **Sesiones persistentes con cookies**
3. **Recuperación de contraseñas**
4. **Validación de email**
5. **Roles y permisos**
6. **Remember me functionality**
7. **Logout con limpieza completa de sesión**
8. **Protección contra ataques de fuerza bruta**

## 💡 Consejos para el Futuro

1. **Para proyectos más grandes:** Considera separar la lógica en servicios
2. **Para producción:** Implementa logging apropiado
3. **Para seguridad:** Agrega rate limiting en login
4. **Para UX:** Agrega indicadores de carga y mejor feedback

¡Excelente trabajo implementando tu primer sistema de autenticación! 🎉