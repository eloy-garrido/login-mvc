// Program.cs - Configuración de la aplicación ASP.NET Core MVC
// Este archivo configura los servicios y el pipeline de la aplicación

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================================

// Agregar servicios necesarios para MVC
builder.Services.AddControllersWithViews();

// Agregar HttpClient para realizar peticiones HTTP a Supabase
// Esto permite que nuestro controlador use IHttpClientFactory
builder.Services.AddHttpClient();

// Configurar logging (opcional, para debugging)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ============================================================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ============================================================================

var app = builder.Build();

// ============================================================================
// CONFIGURACIÓN DEL PIPELINE HTTP
// ============================================================================

// Configurar el manejo de errores según el entorno
if (!app.Environment.IsDevelopment())
{
    // En producción, usar página de error personalizada
    app.UseExceptionHandler("/Home/Error");
    // Comentario: En desarrollo, se muestran errores detallados automáticamente
}
else
{
    // En desarrollo, mostrar página de errores para desarrolladores
    app.UseDeveloperExceptionPage();
}

// Middleware para servir archivos estáticos (CSS, JS, imágenes)
app.UseStaticFiles();

// Middleware de enrutamiento
app.UseRouting();

// Middleware de autorización (no lo usamos en este paso, pero está aquí para el futuro)
// app.UseAuthorization();

// ============================================================================
// CONFIGURACIÓN DE RUTAS
// ============================================================================

// Configurar la ruta por defecto para que apunte a nuestro UsuarioController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Verificar}/{id?}");

// Rutas adicionales específicas para nuestro controlador (opcional)
app.MapControllerRoute(
    name: "verificar",
    pattern: "verificar/{action=Verificar}",
    defaults: new { controller = "Usuario" });

// ============================================================================
// INICIAR LA APLICACIÓN
// ============================================================================

// Mostrar información en la consola sobre dónde está corriendo la aplicación
app.Logger.LogInformation("🚀 Aplicación iniciada");
app.Logger.LogInformation("📱 Para acceder: https://localhost:5001 o http://localhost:5000");
app.Logger.LogInformation("🔍 Página principal: Usuario/Verificar");

// Ejecutar la aplicación
app.Run();

// ============================================================================
// NOTAS TÉCNICAS
// ============================================================================

/*
 * ¿Qué hace cada parte?
 * 
 * 1. builder.Services.AddControllersWithViews()
 *    - Registra todos los servicios necesarios para MVC
 *    - Permite usar controladores, vistas, modelos
 *    - Incluye validación, binding de modelos, etc.
 * 
 * 2. builder.Services.AddHttpClient()
 *    - Registra IHttpClientFactory como servicio
 *    - Permite inyectar HttpClient en controladores
 *    - Maneja automáticamente el lifecycle de HttpClient
 * 
 * 3. app.UseStaticFiles()
 *    - Sirve archivos desde wwwroot/
 *    - CSS, JavaScript, imágenes, etc.
 * 
 * 4. app.UseRouting()
 *    - Habilita el sistema de enrutamiento
 *    - Permite que las URLs se mapeen a controladores/acciones
 * 
 * 5. MapControllerRoute
 *    - Define cómo se mapean las URLs a controladores
 *    - {controller=Usuario} significa que por defecto usa UsuarioController
 *    - {action=Verificar} significa que por defecto usa la acción Verificar
 * 
 * RESULTADO: Al visitar "/" se carga UsuarioController.Verificar()
 */
