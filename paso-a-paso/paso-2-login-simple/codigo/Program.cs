// Program.cs - Configuración de la aplicación ASP.NET Core MVC
// EVOLUCIÓN del Paso 1: Cambio de rutas para AuthController

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS (IGUAL que Paso 1)
// ============================================================================

// Agregar servicios necesarios para MVC
builder.Services.AddControllersWithViews();

// Agregar HttpClient para realizar peticiones HTTP a Supabase
builder.Services.AddHttpClient();

// Configurar logging (igual que Paso 1)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ============================================================================
// CONSTRUCCIÓN DE LA APLICACIÓN (IGUAL que Paso 1)
// ============================================================================

var app = builder.Build();

// ============================================================================
// CONFIGURACIÓN DEL PIPELINE HTTP (IGUAL que Paso 1)
// ============================================================================

// Configurar el manejo de errores según el entorno
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

// Middleware para servir archivos estáticos (CSS, JS, imágenes)
app.UseStaticFiles();

// Middleware de enrutamiento
app.UseRouting();

// ============================================================================
// CONFIGURACIÓN DE RUTAS (ACTUALIZADO para Paso 2)
// ============================================================================

// 🔄 CAMBIO PRINCIPAL: Ruta por defecto ahora apunta a AuthController.Login
// Paso 1: {controller=Usuario}/{action=Verificar}
// Paso 2: {controller=Auth}/{action=Login}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Rutas adicionales específicas para nuestro sistema de autenticación
app.MapControllerRoute(
    name: "auth",
    pattern: "auth/{action=Login}",
    defaults: new { controller = "Auth" });

// Rutas específicas para el flujo de login
app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Auth", action = "Login" });

app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard",
    defaults: new { controller = "Auth", action = "Dashboard" });

app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "Auth", action = "Logout" });

// ============================================================================
// INICIAR LA APLICACIÓN
// ============================================================================

// Mostrar información actualizada en la consola
app.Logger.LogInformation("🚀 Aplicación del Paso 2 iniciada");
app.Logger.LogInformation("📱 Para acceder: https://localhost:5001 o http://localhost:5000");
app.Logger.LogInformation("🔐 Página principal: Auth/Login");
app.Logger.LogInformation("🎯 URLs disponibles:");
app.Logger.LogInformation("   - / → Login");
app.Logger.LogInformation("   - /login → Login");
app.Logger.LogInformation("   - /dashboard → Dashboard (requiere login)");
app.Logger.LogInformation("   - /logout → Cerrar sesión");

// Ejecutar la aplicación
app.Run();

// ============================================================================
// NOTAS TÉCNICAS - CAMBIOS DEL PASO 1
// ============================================================================

/*
 * ¿Qué cambió desde el Paso 1?
 * 
 * 1. RUTAS ACTUALIZADAS:
 *    - Paso 1: {controller=Usuario}/{action=Verificar}
 *    - Paso 2: {controller=Auth}/{action=Login}
 * 
 * 2. CONTROLADOR RENOMBRADO:
 *    - Paso 1: UsuarioController
 *    - Paso 2: AuthController
 * 
 * 3. NUEVAS RUTAS AGREGADAS:
 *    - /login → Para fácil acceso al login
 *    - /dashboard → Para usuarios logueados
 *    - /logout → Para cerrar sesión
 * 
 * 4. LO QUE NO CAMBIÓ:
 *    - Configuración de servicios (igual)
 *    - Pipeline HTTP (igual)
 *    - Configuración de logging (igual)
 *    - Configuración de Supabase (igual)
 * 
 * RESULTADO: 
 * - Al visitar "/" se carga AuthController.Login() 
 * - Flujo completo: Login → Dashboard → Logout
 */
