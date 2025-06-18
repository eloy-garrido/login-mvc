# 🎯 PASO 1: Verificar Usuario - Archivos de Código

## 📁 Descripción de Archivos

Este directorio contiene todos los archivos de código necesarios para implementar el **Paso 1: Verificar si un Usuario Existe**.

### 📋 Lista de Archivos

```
codigo/
├── Controllers/
│   └── UsuarioController.cs         # Controlador principal (similar a HomeController)
├── Models/
│   ├── UserModel.cs                 # Modelo que representa tabla users de Supabase
│   └── VerificarUsuarioViewModel.cs # ViewModel para el formulario
├── Views/
│   └── Usuario/
│       ├── Verificar.cshtml         # Formulario para ingresar username
│       └── Resultado.cshtml         # Página que muestra el resultado
├── Program.cs                       # Configuración de la aplicación
└── appsettings.json                 # Configuración con credenciales de Supabase
```

## 🚀 Cómo Usar Estos Archivos

### Opción 1: Proyecto Nuevo (Recomendado)

1. **Crear proyecto nuevo:**
   ```bash
   dotnet new mvc -n VerificarUsuario
   cd VerificarUsuario
   ```

2. **Copiar archivos:**
   - Reemplaza el contenido de cada archivo con el código correspondiente
   - Copia exactamente como se muestra en cada archivo

3. **Configurar appsettings.json:**
   - Reemplaza las credenciales de Supabase con las tuyas

4. **Ejecutar:**
   ```bash
   dotnet run
   ```

### Opción 2: Proyecto Existente

Si ya tienes un proyecto MVC:

1. **Agregar el controlador:** Copia `UsuarioController.cs` a tu carpeta `Controllers/`
2. **Agregar modelos:** Copia los archivos de `Models/` a tu carpeta `Models/`
3. **Agregar vistas:** Crea carpeta `Views/Usuario/` y copia las vistas
4. **Modificar Program.cs:** Agrega la configuración de rutas
5. **Actualizar appsettings.json:** Agregar sección de Supabase

## 🔧 Dependencias Requeridas

Este código requiere:

- ✅ ASP.NET Core 6.0 o superior
- ✅ System.Text.Json (incluido por defecto)
- ✅ Microsoft.AspNetCore.Mvc (incluido en template MVC)
- ✅ Conexión a internet (para Supabase)

**NO requiere paquetes adicionales** - Solo usa lo que viene por defecto en ASP.NET Core MVC.

## 📊 Estructura de Base de Datos Requerida

Tu tabla `users` en Supabase debe tener esta estructura:

```sql
CREATE TABLE public.users (
    id BIGSERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);
```

Con datos de prueba:
```sql
INSERT INTO users (username, email) VALUES 
('admin', 'admin@test.com'),
('maria', 'maria@test.com'),
('carlos', 'carlos@test.com'),
('ana', 'ana@test.com');
```

## 🔍 Explicación de Cada Archivo

### UsuarioController.cs
- **Propósito:** Maneja las peticiones HTTP y lógica de verificación
- **Patrón:** Igual al HomeController (HttpClient + Supabase)
- **Métodos principales:**
  - `Verificar()` GET: Muestra formulario
  - `Verificar()` POST: Procesa formulario
  - `Resultado()` GET: Muestra resultado
  - `VerificarUsuarioExisteAsync()`: Consulta Supabase

### UserModel.cs
- **Propósito:** Representa la estructura de la tabla `users` en Supabase
- **Características:**
  - Propiedades coinciden con columnas de BD
  - Data Annotations para validación
  - Preparado para deserialización JSON

### VerificarUsuarioViewModel.cs
- **Propósito:** Modelo específico para el formulario de verificación
- **Características:**
  - Solo propiedades necesarias para la vista
  - Validaciones específicas del formulario
  - Separado del modelo de BD (buena práctica)

### Verificar.cshtml
- **Propósito:** Vista del formulario donde usuario ingresa username
- **Características:**
  - Bootstrap para styling
  - Validación del lado cliente
  - Formulario POST con token anti-falsificación
  - Ayudas visuales y usuarios de prueba

### Resultado.cshtml
- **Propósito:** Vista que muestra si el usuario fue encontrado o no
- **Características:**
  - Diseño responsivo
  - Mensajes claros de éxito/error
  - Información técnica sobre la consulta
  - Enlaces para continuar

### Program.cs
- **Propósito:** Configura la aplicación y servicios
- **Características:**
  - Registra servicios MVC
  - Configura HttpClient
  - Define rutas por defecto
  - Configuración de logging

### appsettings.json
- **Propósito:** Configuración de la aplicación
- **Características:**
  - Credenciales de Supabase
  - Configuración de logging
  - Comentarios explicativos

## 🎯 Flujo de Ejecución

1. **Usuario accede** a la aplicación (por defecto `/`)
2. **Ruta por defecto** lleva a `UsuarioController.Verificar()`
3. **Se muestra** el formulario `Verificar.cshtml`
4. **Usuario ingresa** un nombre de usuario y envía
5. **POST** se procesa en `UsuarioController.Verificar(model)`
6. **Se consulta Supabase** usando `VerificarUsuarioExisteAsync()`
7. **Resultado** se guarda en TempData
8. **Redirección** a `UsuarioController.Resultado()`
9. **Se muestra** `Resultado.cshtml` con el resultado

## 🔄 Ciclo de Datos

```
[Formulario] 
    ↓ (POST)
[UsuarioController] 
    ↓ (HttpClient)
[Supabase API] 
    ↓ (JSON Response)
[Deserialización] 
    ↓ (UserModel[])
[Verificación Existencia] 
    ↓ (TempData)
[Vista Resultado]
```

## 🐛 Debugging y Troubleshooting

### Problemas Comunes:

1. **Error 500 - No se puede conectar a Supabase**
   - Verificar credenciales en appsettings.json
   - Verificar que el proyecto de Supabase esté activo

2. **Usuario no encontrado (cuando debería existir)**
   - Verificar que la tabla se llame exactamente `users`
   - Verificar datos de prueba en Supabase
   - Revisar logs en consola

3. **Error de compilación**
   - Verificar namespace en archivos
   - Asegurar que todas las referencias estén correctas

### Para Debug:
- Los logs aparecen en la consola cuando ejecutas `dotnet run`
- Usa puntos de interrupción en UsuarioController
- Verifica respuestas de Supabase en Network tab del navegador

## 🎓 Conceptos Aprendidos

Al usar estos archivos aprenderás:

- ✅ **Patrón MVC** completo con ejemplo real
- ✅ **HttpClient** para consumir APIs REST
- ✅ **Inyección de dependencias** en ASP.NET Core
- ✅ **ViewModels** vs Models de BD
- ✅ **Data Annotations** para validación
- ✅ **TempData** para pasar datos entre acciones
- ✅ **Routing** personalizado en ASP.NET Core
- ✅ **JSON deserialization** con System.Text.Json
- ✅ **Bootstrap** para UI responsiva

¡Perfecto para entender los fundamentos antes de agregar autenticación real! 🚀
