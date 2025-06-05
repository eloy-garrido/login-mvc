# 🛠️ Guía Intermedia: Login con Pseudocódigo y Lógica Detallada

## 🎯 Para Quién es Este Archivo
- Ya leíste `pistas_login.md` e intentaste implementar
- Necesitas más estructura y lógica detallada
- Quieres entender el flujo paso a paso

## 🏗️ Arquitectura del Sistema

### Archivos que Necesitas Crear:
```
📁 Models/
  └── UserModel.cs          (representa tabla users de Supabase)
  └── LoginViewModel.cs     (datos del formulario)

📁 Controllers/
  └── AuthController.cs     (maneja login - como HomeController)

📁 Views/Auth/
  └── Login.cshtml          (formulario de login)
  └── Dashboard.cshtml      (página de éxito)
```

## 🧠 Lógica Detallada del AuthController

### Constructor (Idéntico al HomeController)
```
CONSTRUCTOR AuthController:
    RECIBIR: IHttpClientFactory, IConfiguration
    CREAR: _httpClient usando factory
    OBTENER: _supabaseUrl desde configuration["Supabase:Url"]
    OBTENER: _supabaseKey desde configuration["Supabase:Key"]
    CONFIGURAR: headers apikey y Authorization en _httpClient
```

### Acción GET Login
```
ACCIÓN Login() GET:
    CREAR: modelo LoginViewModel vacío
    RETORNAR: View(modelo)
```

### Acción POST Login (La Más Importante)
```
ACCIÓN Login(LoginViewModel model) POST:
    SI modelo NO es válido:
        RETORNAR: View(model) con errores
    
    INTENTAR:
        usuario = LLAMAR ValidateUserAsync(model)
        
        SI usuario NO es null:
            // ✅ Login exitoso
            GUARDAR en TempData["Username"] = usuario.Username
            GUARDAR en TempData["WelcomeMessage"] = mensaje de bienvenida
            REDIRIGIR a Dashboard
        SINO:
            // ❌ Login fallido
            ASIGNAR model.ErrorMessage = "Usuario o contraseña incorrectos"
            RETORNAR View(model)
    
    CAPTURAR excepción:
        ASIGNAR model.ErrorMessage = "Error del sistema"
        RETORNAR View(model)
```

### Método Privado ValidateUserAsync (Corazón del Login)
```
MÉTODO PRIVADO ValidateUserAsync(LoginViewModel model):
    INTENTAR:
        // Paso 1: Buscar usuario en Supabase
        usuario = LLAMAR GetUserByUsernameAsync(model.Username)
        
        SI usuario es null:
            RETORNAR null  // Usuario no existe
        
        // Paso 2: Verificar contraseña (comparación simple)
        esContraseñaValida = (model.Password == usuario.Password)
        
        SI esContraseñaValida:
            RETORNAR usuario  // Login exitoso
        SINO:
            RETORNAR null     // Contraseña incorrecta
    
    CAPTURAR excepción:
        LOGUEAR error
        RETORNAR null
```

### Método Privado GetUserByUsernameAsync (Consulta a Supabase)
```
MÉTODO PRIVADO GetUserByUsernameAsync(string username):
    INTENTAR:
        // Construir URL con filtro
        url = "_supabaseUrl/rest/v1/users?username=eq." + username
        
        // Hacer petición GET (como en HomeController)
        response = LLAMAR _httpClient.GetAsync(url)
        
        SI response es exitosa:
            // Leer contenido JSON
            json = LEER response.Content
            
            // Deserializar a array de UserModel
            usuarios = JsonSerializer.Deserialize<UserModel[]>(json, opciones_snake_case)
            
            // Retornar primer usuario (debería ser único)
            RETORNAR usuarios.FirstOrDefault()
        SINO:
            RETORNAR null
    
    CAPTURAR excepción:
        LOGUEAR error
        RETORNAR null
```

### Acción Dashboard
```
ACCIÓN Dashboard() GET:
    SI TempData["Username"] es null:
        REDIRIGIR a Login  // No hay sesión válida
    
    PASAR TempData a ViewBag para la vista
    RETORNAR View()
```

## 🎨 Estructura de las Vistas

### Login.cshtml - Lógica del Formulario
```html
ESTRUCTURA Login.cshtml:

CONTENEDOR Bootstrap:
    TARJETA con:
        HEADER: "Iniciar Sesión"
        
        CUERPO:
            SI Model.ErrorMessage NO está vacío:
                MOSTRAR alerta de error
            
            FORMULARIO con asp-action="Login" method="post":
                TOKEN anti-falsificación
                
                CAMPO Username:
                    - label asp-for="Username"
                    - input asp-for="Username" tipo text
                    - span asp-validation-for="Username"
                
                CAMPO Password:
                    - label asp-for="Password"
                    - input asp-for="Password" tipo password
                    - span asp-validation-for="Password"
                
                BOTÓN submit "Iniciar Sesión"
            
            ENLACE: "Volver al inicio"

SCRIPTS de validación al final
```

### Dashboard.cshtml - Página de Éxito
```html
ESTRUCTURA Dashboard.cshtml:

CONTENEDOR Bootstrap:
    TARJETA de éxito:
        HEADER verde: "✅ Login Exitoso"
        
        CUERPO:
            ALERTA de éxito con ViewBag.WelcomeMessage
            
            INFORMACIÓN:
                - Usuario: ViewBag.Username
                - Fecha y hora actual
            
            BOTONES:
                - "🏠 Ir al Inicio"
                - "🚪 Cerrar Sesión"
    
    TARJETA informativa:
        Lista de logros conseguidos
```

## 📊 Modelos y Sus Propósitos

### UserModel - Representa Tabla Supabase
```
CLASE UserModel:
    PROPIEDADES que coinciden con columnas de Supabase:
        int Id
        string Username       (con validación Required)
        string Email         (con validación Required, EmailAddress)
        string Password      (con validación Required) // Texto plano para simplicidad
        DateTime CreatedAt
        DateTime? LastLogin  (nullable)
        bool IsActive
```

### LoginViewModel - Datos del Formulario
```
CLASE LoginViewModel:
    PROPIEDADES para el formulario:
        string Username (Required, Display "Nombre de Usuario")
        string Password (Required, DataType.Password, Display "Contraseña")
        string ErrorMessage (para mostrar errores personalizados)
```

## 🔄 Flujo Completo del Sistema

### Flujo Exitoso:
```
1. Usuario navega a /Auth/Login
2. Se muestra formulario vacío (GET Login)
3. Usuario completa username y password
4. Se envía formulario (POST Login)
5. Sistema busca usuario en Supabase por username
6. Sistema verifica contraseña con BCrypt
7. Si coincide: guardar datos en TempData y redirigir a Dashboard
8. Dashboard muestra mensaje de bienvenida
```

### Flujo con Error:
```
1-4. Igual que flujo exitoso
5. Sistema busca usuario en Supabase
6. Usuario no existe O contraseña no coincide
7. Asignar mensaje de error al modelo
8. Mostrar formulario nuevamente con mensaje de error
```

## 🛠️ Configuraciones Necesarias

### appsettings.json (Ya lo tienes)
```json
{
  "Supabase": {
    "Url": "tu-url-de-supabase",
    "Key": "tu-clave-api"
  }
}
```

### BCrypt Package
```bash
Install-Package BCrypt.Net-Next
```

## 🔍 Detalles Técnicos Importantes

### Filtro en URL de Supabase:
```
Para buscar usuario por username:
URL: "{supabaseUrl}/rest/v1/users?username=eq.{username}"

El "eq" significa "equal" (igual)
```

### Configuración de JsonSerializer:
```
Opciones necesarias:
- PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
- Porque Supabase usa snake_case (created_at) y C# usa PascalCase (CreatedAt)
```

### Headers de Supabase:
```
Necesarios en HttpClient:
- "apikey": tu_clave_api
- "Authorization": "Bearer " + tu_clave_api
```

### Validación de Contraseña con BCrypt:
```
Para verificar:
BCrypt.Net.BCrypt.Verify(contraseña_texto_plano, hash_guardado_en_bd)

Retorna: true si coincide, false si no coincide
```

## 🧪 Testing y Datos de Prueba

### Usuarios de Prueba en Supabase:
```sql
Crear con este SQL:
INSERT INTO users (username, email, password, created_at, is_active)
VALUES ('admin', 'admin@test.com', '123456', NOW(), true);

Contraseña: '123456' (texto plano - solo para aprendizaje)
```

### URLs para Probar:
```
GET /Auth/Login       -> Muestra formulario
POST /Auth/Login      -> Procesa login
GET /Auth/Dashboard   -> Página de éxito
GET /Auth/Logout      -> Limpia sesión
```

## 🚨 Manejo de Errores

### Tipos de Error a Manejar:
```
1. Validación de formulario (ModelState.IsValid)
2. Usuario no encontrado (user == null)
3. Contraseña incorrecta (BCrypt.Verify == false)
4. Errores de conexión con Supabase (try-catch)
5. Acceso a Dashboard sin login (TempData["Username"] == null)
```

### Estrategia de Mensajes:
```
- Error específico: "Usuario o contraseña incorrectos"
- Error genérico: "Error del sistema. Intenta nuevamente."
- Nunca revelar si el usuario existe o no (seguridad)
```

## 🎯 Checklist de Implementación

### Antes de Empezar:
- [ ] Usuarios de prueba creados en Supabase
- [ ] appsettings.json configurado
- [ ] Tabla 'users' con campo 'password' (texto plano)

### Modelos:
- [ ] UserModel con propiedades correctas
- [ ] LoginViewModel con validaciones
- [ ] Namespace correcto en ambos

### Controlador:
- [ ] Constructor igual al HomeController
- [ ] Acción GET Login
- [ ] Acción POST Login con validación
- [ ] Método privado ValidateUserAsync
- [ ] Método privado GetUserByUsernameAsync
- [ ] Acción Dashboard
- [ ] Acción Logout

### Vistas:
- [ ] Carpeta Views/Auth/ creada
- [ ] Login.cshtml con formulario completo
- [ ] Dashboard.cshtml con mensaje de éxito
- [ ] Validación scripts incluidos

### Testing:
- [ ] Aplicación compila sin errores
- [ ] GET /Auth/Login muestra formulario
- [ ] POST con credenciales correctas redirige a Dashboard
- [ ] POST con credenciales incorrectas muestra error
- [ ] Dashboard sin login redirige a Login

## 🤝 ¿Cuándo Usar la Solución Completa?

Usa `solucion_login_completa.md` cuando:
- Implementaste todo según esta guía pero tienes errores
- Quieres ver el código específico de algún método
- Necesitas comparar tu implementación con la solución
- Quieres ver ejemplos concretos de sintaxis

¡Con esta guía deberías poder implementar el login paso a paso! 🚀