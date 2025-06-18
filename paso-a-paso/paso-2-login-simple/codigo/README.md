# 🔐 PASO 2: Login Simple - Archivos de Código

## 📁 Descripción de Archivos

Este directorio contiene todos los archivos de código para implementar el **Paso 2: Login Simple con Username + Password**.

### 📋 Lista de Archivos

```
codigo/
├── Controllers/
│   └── AuthController.cs           # Controlador de autenticación (evolución de UsuarioController)
├── Models/
│   ├── UserModel.cs                # Modelo de BD actualizado (+ propiedad Password)
│   └── LoginViewModel.cs           # ViewModel del formulario (+ campo Password)
├── Views/
│   └── Auth/
│       ├── Login.cshtml            # Formulario de login (evolución de Verificar.cshtml)
│       └── Dashboard.cshtml        # Dashboard del usuario (evolución de Resultado.cshtml)
├── Program.cs                      # Configuración con rutas actualizadas
├── appsettings.json               # Configuración (igual que Paso 1)
└── README.md                      # Este archivo - explicación detallada
```

## 🚀 Cómo Usar Estos Archivos

### Opción 1: Evolucionar desde el Paso 1 (Recomendado)

1. **Partir del proyecto del Paso 1** funcionando
2. **Actualizar BD:** Ejecutar SQL para agregar columna password
3. **Reemplazar archivos:** Usar el código de esta carpeta
4. **Ejecutar:** `dotnet run`

### Opción 2: Proyecto Nuevo

1. **Crear proyecto:** `dotnet new mvc -n LoginSimple`
2. **Copiar archivos:** Todo el contenido de esta carpeta
3. **Configurar appsettings.json:** Con tus credenciales de Supabase
4. **Ejecutar:** `dotnet run`

## 🔧 Dependencias Requeridas

Este código requiere:

- ✅ ASP.NET Core 6.0 o superior
- ✅ System.Text.Json (incluido por defecto)
- ✅ Microsoft.AspNetCore.Mvc (incluido en template MVC)
- ✅ Conexión a internet (para Supabase)
- ✅ Tabla `users` en Supabase con columna `password`

**NO requiere paquetes adicionales** - Solo usa lo que viene por defecto en ASP.NET Core MVC.

## 🗃️ Estructura de Base de Datos Requerida

Tu tabla `users` en Supabase debe tener esta estructura:

```sql
CREATE TABLE public.users (
    id BIGSERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,          -- 🆕 NUEVA COLUMNA
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);
```

Con datos de prueba:
```sql
INSERT INTO users (username, email, password) VALUES 
('admin', 'admin@test.com', '123456'),
('maria', 'maria@test.com', '123456'),
('carlos', 'carlos@test.com', '123456'),
('ana', 'ana@test.com', '123456');
```

## 🔍 Explicación Detallada de Cada Archivo

### AuthController.cs
**EVOLUCIÓN:** De UsuarioController → AuthController

#### Cambios principales:
- **Nuevo método:** `ValidateCredentialsAsync()` - Valida username + password
- **Nuevo método:** `Dashboard()` - Página de bienvenida para usuarios logueados
- **Nuevo método:** `Logout()` - Cerrar sesión
- **Mejorado:** `Login()` POST - Ahora valida credenciales completas

#### Flujo de autenticación:
1. **Usuario envía** formulario con username + password
2. **`ValidateCredentialsAsync()`** busca usuario y compara password
3. **Si correcto:** Guardar datos en TempData → Redirigir a Dashboard
4. **Si incorrecto:** Mostrar error en mismo formulario

#### Patrones mantenidos del Paso 1:
- ✅ Mismo constructor con HttpClient + configuración
- ✅ Mismos headers de Supabase
- ✅ Mismo patrón de consulta a API REST
- ✅ Misma deserialización JSON

### UserModel.cs
**EVOLUCIÓN:** Agregada propiedad `Password`

#### Cambios:
```csharp
// 🆕 NUEVA propiedad
[Required]
public string Password { get; set; } = string.Empty;
```

#### Propósito:
- Mapear la nueva columna `password` de la tabla users
- Permitir deserialización automática desde JSON de Supabase
- Mantener consistencia con estructura de BD

### LoginViewModel.cs
**EVOLUCIÓN:** De VerificarUsuarioViewModel → LoginViewModel

#### Cambios principales:
```csharp
// 🆕 NUEVA propiedad para contraseña
[Required(ErrorMessage = "La contraseña es obligatoria")]
[DataType(DataType.Password)]
[Display(Name = "Contraseña")]
public string Password { get; set; } = string.Empty;

// 🔄 RENOMBRADO para mayor claridad
public bool LoginExitoso { get; set; } = false; // Era: UsuarioEncontrado
```

#### Validaciones agregadas:
- `[Required]` - Campo obligatorio
- `[DataType(DataType.Password)]` - Input tipo password en HTML
- `[StringLength]` - Longitud mínima y máxima

### Login.cshtml
**EVOLUCIÓN:** De Verificar.cshtml → Login.cshtml

#### Características nuevas:
- **Campo password:** Con tipo `password` para ocultar texto
- **Credenciales de prueba:** Mostradas en la página para facilitar testing
- **Mejor UX:** Información sobre qué cambió desde el Paso 1
- **Validación mejorada:** Para ambos campos username y password

#### Elementos importantes:
```html
<!-- Campo password -->
<input asp-for="Password" 
       class="form-control form-control-lg" 
       placeholder="Ingresa tu contraseña"
       autocomplete="current-password" />

<!-- Ayuda visual -->
<small>💡 Contraseña de prueba: <code>123456</code></small>
```

### Dashboard.cshtml
**EVOLUCIÓN:** De Resultado.cshtml → Dashboard.cshtml

#### Nuevas funcionalidades:
- **Información del usuario:** Username, email, hora de login
- **Acciones disponibles:** Probar otro login, logout, recargar
- **Comparación educativa:** Tabla que muestra diferencias con Paso 1
- **Preparación para Paso 3:** Información sobre próximas mejoras

#### Datos mostrados:
```csharp
ViewBag.Username = user.Username;
ViewBag.Email = user.Email;
ViewBag.WelcomeMessage = "¡Bienvenido de vuelta!";
ViewBag.LoginTime = DateTime.Now.ToString();
```

### Program.cs
**EVOLUCIÓN:** Rutas actualizadas

#### Cambios principales:
```csharp
// Paso 1: {controller=Usuario}/{action=Verificar}
// Paso 2: {controller=Auth}/{action=Login}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
```

#### Rutas adicionales:
- `/login` → `Auth/Login`
- `/dashboard` → `Auth/Dashboard`
- `/logout` → `Auth/Logout`

## 🎯 Flujo de Ejecución Completo

### Flujo de Login Exitoso:
1. **Usuario accede** a `/` → `AuthController.Login()` GET
2. **Se muestra** `Login.cshtml` con formulario username + password
3. **Usuario completa** credenciales y envía formulario
4. **POST** a `AuthController.Login()` con `LoginViewModel`
5. **Validación del modelo:** Campos requeridos, longitudes, etc.
6. **`ValidateCredentialsAsync()`:** Busca usuario en Supabase
7. **`GetUserByUsernameAsync()`:** Consulta API REST con filtro
8. **Comparación de password:** `model.Password == user.Password`
9. **Si coincide:** Datos en TempData → `RedirectToAction("Dashboard")`
10. **`Dashboard()`:** Verifica TempData → Muestra `Dashboard.cshtml`

### Flujo de Login Fallido:
1-8. **Igual que flujo exitoso**
9. **Si no coincide:** `model.ErrorMessage` → `View(model)`
10. **Se muestra** `Login.cshtml` con mensaje de error

### Flujo de Logout:
1. **Usuario click** en "Cerrar Sesión"
2. **`AuthController.Logout()`:** `TempData.Clear()`
3. **Redirección** a `Login` con mensaje de confirmación

## 🔄 Comparación con Paso 1

| Aspecto | Paso 1 | Paso 2 | Explicación |
|---------|--------|---------|-------------|
| **Controlador** | `UsuarioController` | `AuthController` | Mejor nombre para autenticación |
| **Método principal** | `Verificar()` | `Login()` | Más claro el propósito |
| **Validación** | Solo existencia | Username + password | Autenticación real |
| **Vista principal** | `Verificar.cshtml` | `Login.cshtml` | Formulario completo |
| **Resultado** | `Resultado.cshtml` | `Dashboard.cshtml` | Experiencia de usuario logueado |
| **Funcionalidad** | Solo verificar | Login + logout | Sistema completo |
| **Base de datos** | Sin password | Con password | Datos de autenticación |

## 🧪 Testing y Validación

### Credenciales de Prueba:
```
✅ Logins exitosos:
admin / 123456
maria / 123456
carlos / 123456
ana / 123456

❌ Para probar errores:
admin / wrongpass (password incorrecta)
noexiste / 123456 (usuario inexistente)
```

### Casos de Prueba:
1. **Login exitoso** → Dashboard con información del usuario
2. **Password incorrecta** → Error sin redirección
3. **Usuario inexistente** → Error sin redirección
4. **Campos vacíos** → Validación del lado cliente
5. **Logout** → Vuelta a login con mensaje
6. **Acceso directo a dashboard** → Redirección a login

## 🐛 Debugging y Logs

### Logs Importantes:
```
# Login exitoso
info: AuthController[0] Intentando login para usuario: admin
info: AuthController[0] Usuario encontrado: admin
info: AuthController[0] Validación de contraseña para admin: True
info: AuthController[0] Login exitoso para usuario: admin

# Login fallido
warn: AuthController[0] Login fallido para usuario: admin
```

### Debugging Tips:
- **Logs detallados:** Cada paso del proceso está logueado
- **Puntos de interrupción:** En `ValidateCredentialsAsync()` para ver flujo
- **Network tab:** Para ver peticiones a Supabase
- **Consola navegador:** Para errores de JavaScript

## ⚠️ Notas Importantes de Seguridad

### Para Aprendizaje (Paso 2):
- ✅ **Contraseñas en texto plano** - Solo para simplicidad educativa
- ✅ **Comparación directa** - `password1 == password2`
- ✅ **TempData para sesión** - Método simple de manejo de estado

### Para Producción (Paso 3):
- 🔒 **Hashing con BCrypt** - Nunca texto plano
- 🔒 **Salt automático** - Protección contra ataques
- 🔒 **Sesiones reales** - ASP.NET Core Identity o cookies seguras

## 🎓 Conceptos Aprendidos

Al usar estos archivos aprenderás:

### Técnicos:
- ✅ **Evolución de código** - Cómo agregar features sin romper
- ✅ **Autenticación web** - Conceptos fundamentales
- ✅ **Validación de formularios** - Múltiples campos con reglas
- ✅ **Manejo de estados** - TempData y redirecciones
- ✅ **Rutas personalizadas** - Configuración de routing

### Arquitecturales:
- ✅ **Separación de responsabilidades** - Controller → View → Model
- ✅ **ViewModels específicos** - Datos de formulario vs BD
- ✅ **Patrón de redirección** - POST → Redirect → GET
- ✅ **Configuración centralizada** - appsettings.json

### UX/UI:
- ✅ **Flujo de autenticación** - Login → Dashboard → Logout
- ✅ **Manejo de errores** - Mensajes claros y específicos
- ✅ **Feedback visual** - Estados de éxito y error
- ✅ **Navegación intuitiva** - Enlaces y botones lógicos

## 🚀 Preparación para Paso 3

Este código te prepara perfectamente para el Paso 3 porque:

- ✅ **Estructura sólida** - Patrones establecidos para expandir
- ✅ **Separación clara** - Fácil cambiar validación sin tocar UI
- ✅ **Logging completo** - Debugging preparado para cambios
- ✅ **Base funcional** - Sistema working que mejorar, no construir desde cero

En el Paso 3 cambiaremos principalmente:
- `ValidateCredentialsAsync()` - Para usar BCrypt
- Base de datos - Passwords hasheadas
- Registro de usuarios - Crear cuentas nuevas

¡Perfecto para seguir construyendo sobre fundamentos sólidos! 🎯
