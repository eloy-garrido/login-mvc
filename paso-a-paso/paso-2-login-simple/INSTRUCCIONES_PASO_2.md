# 🔐 PASO 2: Login Simple con Contraseñas

## 📋 Objetivo de Este Paso
Agregar validación de contraseñas al proyecto del Paso 1. Ahora el formulario pedirá **username + password** y los validará contra Supabase usando **contraseñas en texto plano** (simple para aprender).

## 🎯 Lo Que Vamos a Lograr
- Formulario de login con username + password
- Tabla de Supabase modificada para incluir contraseñas
- Validación real: si username Y password coinciden → Login exitoso
- Página de "Bienvenido" tras login exitoso
- Funcionalidad básica de logout

## 🔄 Evolucionando desde el Paso 1

**¿Completaste el Paso 1?** Este paso asume que tienes funcionando la verificación de usuarios del Paso 1.

### Lo que agregaremos al Paso 1:
- ✅ Campo password en el formulario
- ✅ Columna `password` en tabla Supabase  
- ✅ Validación username + password juntos
- ✅ Página de dashboard tras login exitoso
- ✅ Sistema básico de logout

## 🗃️ Modificaciones a la Base de Datos

### 1. Agregar Columna Password a Supabase

Ejecuta este SQL en Supabase (Table Editor > SQL Editor):

```sql
-- Agregar columna password a la tabla users existente
ALTER TABLE public.users 
ADD COLUMN IF NOT EXISTS password TEXT;

-- Actualizar usuarios existentes con contraseñas de prueba (texto plano)
UPDATE users SET password = '123456' WHERE username = 'admin';
UPDATE users SET password = '123456' WHERE username = 'maria';
UPDATE users SET password = '123456' WHERE username = 'carlos';
UPDATE users SET password = '123456' WHERE username = 'ana';

-- Agregar constraint para que password sea obligatorio en nuevos registros
ALTER TABLE public.users 
ALTER COLUMN password SET NOT NULL;

-- Verificar que todo está correcto
SELECT username, email, password, created_at 
FROM users 
ORDER BY username;
```

**🔥 IMPORTANTE:** Usamos contraseñas en texto plano SOLO para aprender. En producción se usan funciones de hash.

### 2. Verificar Estructura Final de la Tabla

Tu tabla `users` ahora debe tener esta estructura:

```sql
-- Estructura final esperada:
id            | BIGSERIAL PRIMARY KEY
username      | TEXT NOT NULL UNIQUE  
email         | TEXT NOT NULL UNIQUE
password      | TEXT NOT NULL          ← NUEVA COLUMNA
created_at    | TIMESTAMPTZ NOT NULL DEFAULT NOW()
is_active     | BOOLEAN NOT NULL DEFAULT TRUE
```

## 🚀 Configuración del Proyecto

### Opción A: Modificar tu Proyecto del Paso 1 (Recomendado)

Si tienes el proyecto del Paso 1 funcionando:

```bash
# Usar el mismo proyecto del Paso 1
cd tu-carpeta-paso-1/VerificarUsuario

# Abrir en VS Code
code .
```

### Opción B: Crear Proyecto Nuevo

Si quieres empezar desde cero:

```bash
# Crear nuevo proyecto
cd E:\login-mvc\paso-a-paso\paso-2-login-simple
dotnet new mvc -n LoginSimple
cd LoginSimple
code .
```

## 📁 Archivos a Modificar/Crear

### Desde el Paso 1, vamos a:

1. **MODIFICAR** modelos existentes:
   - `Models/UserModel.cs` → Agregar propiedad Password
   - `Models/VerificarUsuarioViewModel.cs` → Renombrar a LoginViewModel

2. **MODIFICAR** controlador existente:
   - `Controllers/UsuarioController.cs` → Renombrar a AuthController y agregar validación de password

3. **MODIFICAR** vistas existentes:
   - `Views/Usuario/Verificar.cshtml` → Cambiar a Login.cshtml con campo password
   - `Views/Usuario/Resultado.cshtml` → Cambiar a Dashboard.cshtml para usuarios logueados

4. **CONSERVAR** sin cambios:
   - `Program.cs` → Solo actualizar rutas
   - `appsettings.json` → Mantener igual

## 🔧 Implementación Paso a Paso

### Paso 1: Actualizar UserModel.cs

Modifica `Models/UserModel.cs` para incluir la propiedad password:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LoginSimple.Models  // 🔄 Cambia por el nombre de tu proyecto
{
    /// <summary>
    /// Modelo que representa la estructura de la tabla 'users' en Supabase
    /// ACTUALIZADO: Ahora incluye password para autenticación
    /// </summary>
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
        
        // 🆕 NUEVA: Corresponde a la columna 'password' en Supabase
        [Required]
        public string Password { get; set; } = string.Empty;
        
        // Corresponde a la columna 'created_at' en Supabase
        public DateTime CreatedAt { get; set; }
        
        // Corresponde a la columna 'is_active' en Supabase
        public bool IsActive { get; set; } = true;
    }
}
```

### Paso 2: Crear LoginViewModel.cs

Reemplaza `VerificarUsuarioViewModel.cs` con `LoginViewModel.cs`:

```csharp
using System.ComponentModel.DataAnnotations;

namespace LoginSimple.Models
{
    /// <summary>
    /// ViewModel para el formulario de login (username + password)
    /// ACTUALIZADO: Ahora incluye campo de contraseña
    /// </summary>
    public class LoginViewModel
    {
        // Campo para el nombre de usuario
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        // 🆕 NUEVO: Campo para la contraseña
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]  // Esto hace que el campo se muestre como password
        [Display(Name = "Contraseña")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        // Propiedad para mostrar mensajes de error personalizados
        public string ErrorMessage { get; set; } = string.Empty;

        // Indica si el login fue exitoso
        public bool LoginExitoso { get; set; } = false;
    }
}
```

### Paso 3: Actualizar AuthController.cs

Reemplaza `UsuarioController.cs` con `AuthController.cs` (copia el código de la carpeta `codigo/Controllers/`).

### Paso 4: Crear Vistas Nuevas

Crea las vistas en `Views/Auth/`:
- `Login.cshtml` (evolución de Verificar.cshtml)
- `Dashboard.cshtml` (evolución de Resultado.cshtml)
- `Logout.cshtml` (nueva)

### Paso 5: Actualizar Program.cs

Modifica las rutas en `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios necesarios (IGUAL que Paso 1)
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Para conectar con Supabase

var app = builder.Build();

// Configurar pipeline (IGUAL que Paso 1)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// 🔄 ACTUALIZADO: Cambiar ruta por defecto a AuthController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
```

## 🧪 Probar la Aplicación

### Credenciales de Prueba:

Todos los usuarios tienen la contraseña: `123456`

- **Usuario:** `admin` | **Contraseña:** `123456`
- **Usuario:** `maria` | **Contraseña:** `123456`  
- **Usuario:** `carlos` | **Contraseña:** `123456`
- **Usuario:** `ana` | **Contraseña:** `123456`

### Flujo de Pruebas:

1. **Acceder:** `https://localhost:5001` → Formulario de login
2. **Login exitoso:** admin + 123456 → Dashboard de bienvenida
3. **Login fallido:** admin + wrongpass → Mensaje de error
4. **Logout:** Desde dashboard → Volver al login

## 🎯 Lo Que Está Pasando (Explicación Técnica)

### Flujo Completo del Login:
1. **Usuario ingresa** username + password en formulario
2. **AuthController** recibe datos del POST
3. **Se busca usuario** en Supabase por username (igual que Paso 1)
4. **🆕 Se compara password:** `inputPassword == user.Password` (texto plano)
5. **Si coinciden:** Guardar datos en TempData y redirigir a Dashboard
6. **Si no coinciden:** Mostrar mensaje de error

### Diferencias con el Paso 1:
- ✅ **Agregamos validación de password:** No solo verificar existencia
- ✅ **Formulario más completo:** Username + Password
- ✅ **Lógica más robusta:** Validar ambos campos
- ✅ **UX mejorada:** Dashboard de usuario logueado

### Preparando para el Paso 3:
- En el siguiente paso cambiaremos password en texto plano por hashing seguro
- Agregaremos mejor manejo de sesiones
- Implementaremos registro de usuarios

## 🔍 Solución de Problemas

### Error: "Column 'password' does not exist"
- Verifica que ejecutaste el SQL para agregar la columna password
- Revisa que la tabla se actualizada correctamente

### Error: "Login always fails"
- Verifica que los usuarios tengan password = '123456'
- Asegúrate de que la comparación sea case-sensitive
- Revisa logs en consola para más detalles

### Error: "Page not found"
- Verifica que cambiaste las rutas en Program.cs
- Asegúrate de que el controlador se llame AuthController

## 🎉 ¡Éxito! ¿Qué Has Logrado?

Si todo funciona, has conseguido:

✅ **Login real funcional** con username + password
✅ **Validación de credenciales** contra base de datos
✅ **Manejo de estados:** Login exitoso vs fallido  
✅ **UX completa:** Formulario → Validación → Dashboard → Logout
✅ **Base sólida** para agregar seguridad real en Paso 3

## 🚀 ¿Qué Sigue?

En el **Paso 3** vamos a:
- Reemplazar contraseñas en texto plano por hashing seguro (BCrypt)
- Agregar sistema de registro de usuarios
- Implementar mejor manejo de errores
- Mejorar la seguridad general

## 💡 Conceptos Aprendidos

- **Evolución de código:** Cómo agregar features sin romper lo existente
- **Autenticación básica:** Validación de credenciales
- **Manejo de formularios:** Campos múltiples con validaciones
- **Flujo de autenticación:** Login → Dashboard → Logout
- **Preparación para seguridad:** Base para implementar hashing

¡Excelente progreso! Ahora tienes un sistema de login funcional que es la base perfecta para agregar seguridad real. 🔐
