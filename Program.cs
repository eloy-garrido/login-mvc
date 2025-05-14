// Este es el punto de entrada principal de una aplicación ASP.NET Core minimalista
// que demuestra la conexión a Supabase

var builder = WebApplication.CreateBuilder(args);

// Registramos solo los servicios esenciales
builder.Services.AddControllersWithViews();  // Necesario para MVC
builder.Services.AddHttpClient();            // Necesario para HttpClient (conexión REST a Supabase)

var app = builder.Build();

// Configuración minimalista del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Middleware esencial
app.UseStaticFiles();   // Para servir archivos CSS/JS
app.UseRouting();       // Para el enrutamiento

// Ruta por defecto (apunta a HomeController)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia la aplicación
app.Run();