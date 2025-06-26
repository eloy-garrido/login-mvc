# 🎯 PASO 1: Verificar si un Usuario Existe

## 📋 Objetivo de Este Paso
Crear un formulario simple que permita ingresar un nombre de usuario y verificar si existe en la base de datos de Supabase. **SIN contraseñas, SIN login**, solo verificar existencia.

## 🎯 Lo Que Vamos a Lograr
- Un formulario donde el usuario ingresa solo su username
- El sistema consulta Supabase para ver si ese usuario existe
- Muestra un mensaje: "✅ Usuario encontrado" o "❌ Usuario no existe"
- Base perfecta para el siguiente paso (agregar contraseñas)

## 🚀 Configuración Inicial en VS Code

### 0. Configurar VS Code para ASP.NET Core (IMPORTANTE)

Antes de empezar, asegúrate de tener las extensiones necesarias:

#### Extensiones OBLIGATORIAS:
1. **C# Dev Kit** (Microsoft)
   - Ve a Extensions (Ctrl+Shift+X)
   - Busca "C# Dev Kit"
   - Instala (incluye C# + IntelliSense + Debugging)
   - Se instalarán automáticamente las dependencias

#### Verificar .NET SDK:
```bash
# Verificar que .NET esté instalado
dotnet --version
# Debería mostrar: 6.0.xxx, 7.0.xxx, o 8.0.xxx
```

**Si no tienes .NET:** Descarga desde https://dotnet.microsoft.com/download

### 1. Crear Nuevo Proyecto ASP.NET Core MVC

**¡El comando `dotnet new mvc` crea TODA la estructura MVC automáticamente!**
- ✅ Carpetas Controllers/, Models/, Views/
- ✅ HomeController y vistas básicas
- ✅ Bootstrap y archivos estáticos
- ✅ Program.cs configurado

Abre VS Code y ejecuta en la terminal:

```bash
# Navegar a tu carpeta de trabajo
cd E:\login-mvc\paso-a-paso\paso-1-verificar-usuario

# Crear nuevo proyecto MVC
dotnet new mvc -n VerificarUsuario

# Entrar al proyecto
cd VerificarUsuario

# Abrir en VS Code
code .
```

### 2. Configurar appsettings.json

Reemplaza el contenido de `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Supabase": {
    "Url": "https://tu-proyecto-id.supabase.co",
    "Key": "tu-clave-api-anónima"
  }
}
```

**🔥 IMPORTANTE:** Reemplaza con tus credenciales reales de Supabase.

### 3. Verificar que Tienes Usuarios en Supabase

Ejecuta este SQL en Supabase (Table Editor > SQL Editor):

```sql
-- Crear tabla si no existe
CREATE TABLE IF NOT EXISTS public.users (
    id BIGSERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- Insertar usuarios de prueba (solo username y email, sin password aún)
INSERT INTO users (username, email, is_active) 
VALUES 
('admin', 'admin@test.com', true),
('maria', 'maria@test.com', true),
('carlos', 'carlos@test.com', true),
('ana', 'ana@test.com', true)
ON CONFLICT (username) DO NOTHING;
```

## 📁 Estructura de Archivos a Crear

Dentro de tu proyecto `VerificarUsuario`, vas a crear:

```
VerificarUsuario/
├── Controllers/
│   └── UsuarioController.cs       (nuevo - copia y pega el código)
├── Models/
│   ├── UserModel.cs               (nuevo - copia y pega el código)
│   └── VerificarUsuarioViewModel.cs (nuevo - copia y pega el código)
├── Views/
│   └── Usuario/
│       ├── Verificar.cshtml       (nuevo - copia y pega el código)
│       └── Resultado.cshtml       (nuevo - copia y pega el código)
└── Program.cs                     (modificar - copia y pega el código)
```

## 🔧 Paso a Paso: Implementación

### Paso 1: Modificar Program.cs

Reemplaza el contenido de `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios necesarios
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Para conectar con Supabase

var app = builder.Build();

// Configurar pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Ruta personalizada para nuestro controlador Usuario
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Verificar}/{id?}");

app.Run();
```

### Paso 2: Crear UserModel.cs

Crea el archivo `Models/UserModel.cs` (copia el código de la carpeta `codigo/Models/`).

### Paso 3: Crear VerificarUsuarioViewModel.cs

Crea el archivo `Models/VerificarUsuarioViewModel.cs` (copia el código de la carpeta `codigo/Models/`).

### Paso 4: Crear UsuarioController.cs

Crea el archivo `Controllers/UsuarioController.cs` (copia el código de la carpeta `codigo/Controllers/`).

### Paso 5: Crear Vistas

Crea las vistas en `Views/Usuario/`:
- `Verificar.cshtml` (copia el código de la carpeta `codigo/Views/Usuario/`)
- `Resultado.cshtml` (copia el código de la carpeta `codigo/Views/Usuario/`)

## 🚀 Ejecutar el Proyecto

En la terminal de VS Code:

```bash
# Compilar el proyecto
dotnet build

# Ejecutar el proyecto
dotnet run
```

O simplemente presiona `F5` en VS Code para ejecutar en modo debug.

## 🧪 Probar la Aplicación

1. Ve a: `https://localhost:5001` (o el puerto que te muestre)
2. Deberías ver un formulario simple con un campo "Nombre de Usuario"
3. Prueba con:
   - `admin` → Debería mostrar "✅ Usuario encontrado"
   - `noexiste` → Debería mostrar "❌ Usuario no existe"
   - `maria` → Debería mostrar "✅ Usuario encontrado"

## 🎯 Lo Que Está Pasando (Explicación Técnica)

### Flujo de la Aplicación:
1. **Usuario ingresa** un nombre de usuario en el formulario
2. **UsuarioController** recibe los datos del formulario
3. **Se consulta Supabase** usando HttpClient (igual que HomeController)
4. **Se verifica** si el array de usuarios devuelto tiene elementos
5. **Se muestra el resultado** con un mensaje claro

### Conexión con Supabase:
- Usamos la misma técnica que en HomeController
- Headers: `apikey` y `Authorization`
- URL con filtro: `/rest/v1/users?username=eq.nombreusuario`
- Deserialización JSON a array de UserModel

### Sin Complejidad Innecesaria:
- ❌ Sin contraseñas
- ❌ Sin hashing
- ❌ Sin sesiones
- ✅ Solo verificar existencia
- ✅ Entender el patrón MVC
- ✅ Dominar HttpClient + Supabase

## 🔍 Solución de Problemas

### Error: "No se puede conectar a Supabase"
- Verifica las credenciales en `appsettings.json`
- Asegúrate de que el proyecto de Supabase esté activo

### Error: "Usuario no encontrado" (cuando debería existir)
- Verifica que insertaste los usuarios de prueba
- Revisa que la tabla se llame exactamente `users`
- Verifica que el campo se llame exactamente `username`

### Error: Página no se carga
- Asegúrate de que el proyecto esté corriendo (`dotnet run`)
- Verifica que estés accediendo a la URL correcta

## 🎉 ¡Éxito! ¿Qué Has Logrado?

Si todo funciona, has conseguido:

✅ **Dominar la conexión con Supabase** usando HttpClient
✅ **Crear formularios MVC** con ViewModels
✅ **Manejar redirecciones** entre acciones
✅ **Deserializar JSON** de APIs externas
✅ **Validar datos** del lado del servidor
✅ **Estructura base** para el siguiente paso

## 🚀 ¿Qué Sigue?

En el **Paso 2** vamos a:
- Agregar un campo de contraseña al formulario
- Modificar la tabla para incluir passwords (texto plano)
- Validar username + password
- Crear un verdadero "login" simple

## 💡 Conceptos Aprendidos

- **Patrón MVC**: Separación clara de responsabilidades
- **HttpClient**: Consumir APIs REST externas
- **ViewModels**: Modelos específicos para formularios
- **Data Annotations**: Validación de formularios
- **TempData**: Pasar datos entre acciones
- **Supabase REST API**: Filtros y consultas

¡Excelente trabajo! Ahora tienes la base perfecta para agregar autenticación real en el siguiente paso. 🎯
