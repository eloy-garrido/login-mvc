# ✅ CHECKLIST DE VALIDACIÓN - PASO 1

## 📋 Lista de Verificación Completa

Usa esta lista para asegurarte de que todo esté configurado correctamente antes de ejecutar el proyecto.

### 🚀 1. Configuración del Proyecto

#### Prerrequisitos:
- [ ] **VS Code instalado** con extensión "C# Dev Kit"
- [ ] **.NET SDK instalado:** `dotnet --version` funciona
- [ ] **Extensiones activas:** IntelliSense C# funcionando en VS Code

#### Proyecto:
- [ ] **Proyecto creado:** `dotnet new mvc -n VerificarUsuario`
- [ ] **Directorio correcto:** Estás dentro de la carpeta del proyecto
- [ ] **VS Code abierto:** `code .` ejecutado correctamente
- [ ] **Estructura MVC visible:** Carpetas Controllers/, Models/, Views/ creadas automáticamente

### 🗃️ 2. Archivos Copiados

Verifica que hayas copiado todos estos archivos con el contenido correcto:

#### Controllers/
- [ ] **UsuarioController.cs** - Copiado y sin errores de compilación
- [ ] **Namespace correcto:** `namespace VerificarUsuario.Controllers`

#### Models/
- [ ] **UserModel.cs** - Copiado con todas las propiedades
- [ ] **VerificarUsuarioViewModel.cs** - Copiado con validaciones
- [ ] **Namespace correcto:** `namespace VerificarUsuario.Models`

#### Views/
- [ ] **Carpeta Views/Usuario/ creada**
- [ ] **Views/Usuario/Verificar.cshtml** - Formulario completo copiado
- [ ] **Views/Usuario/Resultado.cshtml** - Vista de resultado copiada
- [ ] **@model referencias correctas** en ambas vistas

#### Configuración/
- [ ] **Program.cs** - Reemplazado con configuración personalizada
- [ ] **appsettings.json** - Actualizado con credenciales de Supabase

### 🔗 3. Configuración de Supabase

#### Credenciales:
- [ ] **URL configurada:** `"Url": "https://tu-proyecto-real.supabase.co"`
- [ ] **Key configurada:** `"Key": "tu-clave-real-aquí"`
- [ ] **Formato correcto:** JSON válido sin comas extra

#### Base de Datos:
- [ ] **Tabla `users` creada** en Supabase
- [ ] **Estructura correcta:** id, username, email, created_at, is_active
- [ ] **Datos de prueba insertados:** admin, maria, carlos, ana
- [ ] **Constrains configurados:** username UNIQUE, email UNIQUE

### 📝 4. Verificación de Sintaxis

Ejecuta en terminal para verificar que no hay errores:

```bash
# Verificar que compila sin errores
dotnet build
```

**Resultado esperado:** ✅ `Build succeeded. 0 Warning(s) 0 Error(s)`

**Si hay errores:**
- [ ] Verificar que todos los `using` statements estén correctos
- [ ] Verificar que los namespaces coincidan
- [ ] Verificar que las referencias a modelos estén correctas

### 🚀 5. Ejecución

```bash
# Ejecutar el proyecto
dotnet run
```

**Resultado esperado:**
- [ ] **Aplicación inicia sin errores**
- [ ] **URLs mostradas:** `http://localhost:5000` y `https://localhost:5001`
- [ ] **Logs en consola:** "🚀 Aplicación iniciada"

### 🌐 6. Pruebas en Navegador

#### Acceso inicial:
- [ ] **Navegar a:** `https://localhost:5001` (o puerto mostrado)
- [ ] **Página carga:** Formulario "Verificar Usuario" visible
- [ ] **Sin errores 500:** No aparece página de error

#### Prueba 1 - Usuario Existente:
- [ ] **Ingresar:** `admin` en el campo username
- [ ] **Enviar formulario:** Click en "🔍 Verificar Usuario"
- [ ] **Resultado esperado:** "✅ Usuario 'admin' encontrado en el sistema!"
- [ ] **URL cambia a:** `/Usuario/Resultado`

#### Prueba 2 - Usuario No Existente:
- [ ] **Volver al formulario:** Click en "🔍 Verificar Otro Usuario"
- [ ] **Ingresar:** `noexiste` en el campo username
- [ ] **Enviar formulario:** Click en "🔍 Verificar Usuario"
- [ ] **Resultado esperado:** "❌ Usuario 'noexiste' no existe en el sistema."

#### Prueba 3 - Validación de Formulario:
- [ ] **Volver al formulario**
- [ ] **Dejar campo vacío** y enviar
- [ ] **Resultado esperado:** Mensaje de error "El nombre de usuario es obligatorio"

### 🔍 7. Verificación de Logs

En la consola donde ejecutaste `dotnet run`, deberías ver:

```
✅ Logs esperados:
info: VerificarUsuario.Controllers.UsuarioController[0]
      UsuarioController configurado con Supabase URL: https://tu-proyecto.supabase.co
info: VerificarUsuario.Controllers.UsuarioController[0]
      Verificando usuario: admin
info: VerificarUsuario.Controllers.UsuarioController[0]
      Consultando Supabase: https://tu-proyecto.supabase.co/rest/v1/users?username=eq.admin
info: VerificarUsuario.Controllers.UsuarioController[0]
      Usuario admin existe: True
```

### 🐛 8. Solución de Problemas

#### Si la aplicación no inicia:
- [ ] **Verificar puerto ocupado:** Cambiar puerto o cerrar otras aplicaciones
- [ ] **Verificar firewall:** Permitir conexiones en puertos 5000/5001

#### Si aparece error 500:
- [ ] **Verificar appsettings.json:** Credenciales correctas de Supabase
- [ ] **Verificar logs:** Leer mensaje de error específico en consola
- [ ] **Verificar conexión:** Internet funcionando correctamente

#### Si "Usuario no encontrado" cuando debería existir:
- [ ] **Verificar tabla en Supabase:** Tabla `users` existe y tiene datos
- [ ] **Verificar nombres:** Username exactamente como en BD (case-sensitive)
- [ ] **Verificar permisos:** Tabla accesible con la API key

#### Si formulario no funciona:
- [ ] **Verificar JavaScript:** No hay errores en consola del navegador
- [ ] **Verificar formulario:** Atributos `asp-action` correctos
- [ ] **Verificar CSRF:** Token anti-falsificación incluido

### 🎯 9. Confirmación Final

Si todas las verificaciones anteriores pasan:

- [ ] **✅ Paso 1 completado exitosamente**
- [ ] **🎓 Conceptos dominados:** MVC, HttpClient, Supabase, ViewModels
- [ ] **🚀 Listo para Paso 2:** Agregar contraseñas y login real

### 📞 10. Si Necesitas Ayuda

Si algo no funciona:

1. **Lee los logs cuidadosamente** - La mayoría de errores están explicados ahí
2. **Verifica cada checkbox** de esta lista uno por uno
3. **Compara tu código** con los archivos en la carpeta `codigo/`
4. **Verifica Supabase** directamente en su interfaz web

### 🎉 ¡Felicitaciones!

Si completaste toda esta lista, has logrado:
- ✅ Configurar un proyecto ASP.NET Core MVC desde cero
- ✅ Conectar exitosamente con Supabase
- ✅ Implementar formularios con validación
- ✅ Manejar respuestas de APIs externas
- ✅ Crear una aplicación web funcional

**¡Estás listo para el Paso 2: Login Simple con Contraseñas!** 