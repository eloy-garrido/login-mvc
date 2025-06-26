# Proyecto de Autenticación con Supabase

Este proyecto es un ejercicio educativo que demuestra cómo implementar una conexión a Supabase desde una aplicación ASP.NET Core MVC usando C#. La aplicación permite verificar la conectividad con Supabase como base para desarrollar un sistema de inicio de sesión.

## Autoevaluación
https://eloy-garrido.github.io/autoevaluacion/

## Objetivo del Ejercicio

El objetivo principal de este ejercicio es aprender a:

1. Configurar una aplicación ASP.NET Core MVC para trabajar con Supabase
2. Implementar la conexión a Supabase utilizando HttpClient
3. Verificar la conectividad con la base de datos
4. Estructurar el código siguiendo el patrón MVC (Model-View-Controller)
5. Desarrollar un sistema de autenticación completo (registro, inicio de sesión, cierre de sesión)

## Estructura del Proyecto

El proyecto sigue el patrón de diseño MVC:

- **Modelo (Model)**: Representa los datos y la lógica de negocio (a implementar)
- **Vista (View)**: Interface de usuario (Index.cshtml)
- **Controlador (Controller)**: Maneja las solicitudes y coordina las respuestas (HomeController.cs)

Los archivos incluidos son:

- `appsettings.json`: Contiene la configuración de la aplicación, incluyendo las credenciales de Supabase
- `HomeController.cs`: Controlador principal que maneja la verificación de la conexión
- `Index.cshtml`: Vista que muestra el estado de la conexión
- `Program.cs`: Punto de entrada de la aplicación y configuración del pipeline HTTP
- `spinner.css`: Estilos CSS para la animación de carga

## Requisitos Previos

Para trabajar con este proyecto necesitas:

- Visual Studio 2022 o posterior
- .NET 6.0 o posterior
- Una cuenta en [Supabase](https://supabase.io)
- Un proyecto creado en Supabase con sus correspondientes credenciales

## Configuración Inicial

### 1. Configuración de Supabase

Antes de comenzar a trabajar con el código:

1. Crea una cuenta en [Supabase](https://supabase.com) si aún no tienes una
2. Crea un nuevo proyecto en Supabase
3. Una vez creado el proyecto, obtén:
   - La URL del proyecto (`https://tu-proyecto-id.supabase.co`)
   - La clave API anónima (`Settings > API > anon/public`)

### 2. Creación de la Tabla de Usuarios en Supabase

Para que este ejercicio funcione correctamente, primero debes crear una tabla en Supabase:

1. Ve a la sección "Table Editor" en el panel de Supabase
2. Crea una nueva tabla llamada "users" con las siguientes columnas:
   - id (tipo int8, primary key)
   - username (tipo text, not null, unique)
   - email (tipo text, not null, unique)
   - password_hash (tipo text, not null)
   - created_at (tipo timestamptz, not null, default: now())
   - last_login (tipo timestamptz)
   - is_active (tipo boolean, not null, default: true)

También puedes crear la tabla con SQL:

```sql
CREATE TABLE public.users (
    id BIGSERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    last_login TIMESTAMPTZ,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);
```

### 3. Configuración del Proyecto en Visual Studio

1. Abre el archivo `appsettings.json`
2. Verifica que las siguientes configuraciones estén correctamente establecidas:
   ```json
   "ConnectionStrings": {
     "SupabasePostgres": "Host=db.tu-proyecto-id.supabase.co;Database=postgres;Username=postgres;Password=tu-contraseña;Port=5432;sslmode=Require;Trust Server Certificate=true;"
   },
   "Supabase": {
     "Url": "https://tu-proyecto-id.supabase.co",
     "Key": "tu-clave-api-anónima"
   }
   ```
3. Reemplaza los valores de ejemplo con tus propias credenciales de Supabase

## Cómo Ejecutar el Proyecto

1. Abre la solución en Visual Studio
2. Compila el proyecto (Build > Build Solution o presiona F6)
3. Ejecuta la aplicación (Debug > Start Debugging o presiona F5)
4. En el navegador se abrirá la página principal que mostrará si la conexión con Supabase fue exitosa

## Tareas del Ejercicio

A continuación, te presentamos una serie de tareas para completar este ejercicio:

### Tarea 1: Asegurar la Conexión

- Ejecuta la aplicación y verifica que la conexión a Supabase sea exitosa
- Si hay errores, revisa la configuración en `appsettings.json`
- Asegúrate de que la tabla "users" se haya creado correctamente en Supabase

### Tarea 2: Crear Modelo de Usuario

- Crea una carpeta `Models` si no existe
- Implementa una clase `UserModel.cs` con propiedades que correspondan a la tabla creada en Supabase:
  ```csharp
  public class UserModel
  {
      public int Id { get; set; }
      public string Username { get; set; }
      public string Email { get; set; }
      public string PasswordHash { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime? LastLogin { get; set; }
      public bool IsActive { get; set; }
  }
  ```

### Tarea 3: Crear Servicios para Autenticación

- Crea una carpeta `Services` para contener la lógica de autenticación
- Implementa una interfaz `IAuthService.cs` con métodos para:
  - Registrar un nuevo usuario
  - Iniciar sesión
  - Cerrar sesión
  - Obtener información del usuario actual
  - Verificar si un usuario está autenticado

### Tarea 4: Implementar el Servicio de Autenticación

- Crea una clase `AuthService.cs` que implemente la interfaz
- Utiliza HttpClient para realizar operaciones con la API REST de Supabase
- Ejemplo de método para registrar un usuario:
  ```csharp
  public async Task<UserModel> RegisterUser(string username, string email, string password)
  {
      // Crear un hash seguro de la contraseña
      string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
      
      // Crear objeto de usuario para enviar a Supabase
      var newUser = new UserModel
      {
          Username = username,
          Email = email,
          PasswordHash = passwordHash,
          CreatedAt = DateTime.UtcNow,
          IsActive = true
      };
      
      // Configurar la solicitud a la API REST de Supabase
      var response = await _httpClient.PostAsJsonAsync("rest/v1/users", newUser);
      
      // Verificar si la solicitud fue exitosa
      response.EnsureSuccessStatusCode();
      
      // Leer y devolver la respuesta
      var createdUser = await response.Content.ReadFromJsonAsync<UserModel>();
      return createdUser;
  }
  ```

### Tarea 5: Crear un Controlador de Autenticación

- Crea un nuevo controlador `AuthController.cs`
- Inyecta el servicio de autenticación en el constructor
- Implementa acciones para:
  - Mostrar el formulario de registro
  - Procesar el registro
  - Mostrar el formulario de inicio de sesión
  - Procesar el inicio de sesión
  - Cerrar sesión
  - Mostrar perfil de usuario

### Tarea 6: Crear Vistas para Autenticación

- Crea las siguientes vistas:
  - `Register.cshtml`: Formulario de registro
  - `Login.cshtml`: Formulario de inicio de sesión
  - `Profile.cshtml`: Página de perfil del usuario
  - `AccessDenied.cshtml`: Página para accesos no autorizados
- Usa Bootstrap para el diseño responsivo

### Tarea 7: Implementar Middleware de Autenticación

- Crea un middleware personalizado o utiliza las opciones integradas de ASP.NET Core
- Configura las rutas que requieren autenticación
- Implementa redirecciones para usuarios no autenticados

## Seguridad Importante

Al trabajar con sistemas de autenticación, ten en cuenta las siguientes consideraciones de seguridad:

1. **Nunca almacenes contraseñas en texto plano** - Siempre utiliza funciones de hash como BCrypt
2. **Implementa validación de entrada** - Valida todos los datos recibidos de los formularios
3. **Protege contra ataques CSRF** - Utiliza tokens anti-falsificación en formularios
4. **Configura HTTPS** - Asegúrate de que la aplicación utilice conexiones seguras
5. **Implementa límites de intentos** - Protege contra ataques de fuerza bruta

## Recursos Adicionales

- [Documentación oficial de Supabase](https://supabase.com/docs)
- [Supabase Auth API](https://supabase.com/docs/reference/javascript/auth-signup)
- [Tutorial de ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Uso de HttpClient en ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests)
- [Seguridad en ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/)

## Solución de Problemas Comunes

### Error de Conexión

- Verifica que las credenciales en `appsettings.json` sean correctas
- Asegúrate de que el proyecto de Supabase esté activo
- Revisa si hay problemas de red o firewall

### Error con la Autenticación

- Verifica que la tabla "users" exista en Supabase y tenga la estructura correcta
- Asegúrate de que los campos en tu modelo coincidan con los de la tabla
- Revisa los permisos de la tabla en Supabase (Row Level Security)
- Comprueba que las contraseñas se estén hasheando correctamente

## Extensiones del Proyecto

Una vez completado el sistema básico de autenticación, puedes ampliar el proyecto con:

1. **Recuperación de contraseña** - Implementa un flujo para restablecer contraseñas olvidadas
2. **Autenticación con proveedores externos** - Integra inicio de sesión con Google, Facebook, etc.
3. **Gestión de perfiles** - Permite a los usuarios actualizar su información
4. **Roles y permisos** - Implementa diferentes niveles de acceso
5. **Autenticación de dos factores** - Añade una capa extra de seguridad

## Conclusión

Este ejercicio te proporciona una base sólida para entender cómo integrar Supabase con una aplicación ASP.NET Core MVC para implementar un sistema de autenticación completo. Una vez completadas todas las tareas, tendrás un sistema funcional de registro e inicio de sesión que utiliza una base de datos remota y sigue las mejores prácticas de seguridad.

¡Buena suerte con el ejercicio!
