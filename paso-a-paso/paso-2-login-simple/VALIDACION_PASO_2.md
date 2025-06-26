# ✅ CHECKLIST DE VALIDACIÓN - PASO 2

## 📋 Lista de Verificación Completa

Usa esta lista para asegurarte de que el Paso 2 (Login Simple) esté funcionando correctamente.

### 🔄 0. Prerrequisitos del Paso 1

Antes de empezar el Paso 2, verifica que completaste exitosamente el Paso 1:

- [ ] **Paso 1 funcionando:** El proyecto del Paso 1 verifica usuarios correctamente
- [ ] **VS Code configurado:** Con extensión C# Dev Kit funcionando
- [ ] **.NET SDK operativo:** `dotnet --version` muestra versión correcta
- [ ] **Supabase conectado:** Credenciales funcionando en el Paso 1

### 🗃️ 1. Actualización de Base de Datos

#### SQL Ejecutado en Supabase:
- [ ] **Columna password agregada:** `ALTER TABLE users ADD COLUMN password TEXT`
- [ ] **Contraseñas de prueba insertadas:** Todos los usuarios tienen password '123456'
- [ ] **Constraint agregado:** `ALTER TABLE users ALTER COLUMN password SET NOT NULL`
- [ ] **Verificación exitosa:** `SELECT username, password FROM users` muestra datos correctos

#### Estructura Final de Tabla:
- [ ] **id:** BIGSERIAL PRIMARY KEY ✅
- [ ] **username:** TEXT NOT NULL UNIQUE ✅
- [ ] **email:** TEXT NOT NULL UNIQUE ✅
- [ ] **password:** TEXT NOT NULL ✅ (NUEVA COLUMNA)
- [ ] **created_at:** TIMESTAMPTZ NOT NULL DEFAULT NOW() ✅
- [ ] **is_active:** BOOLEAN NOT NULL DEFAULT TRUE ✅

### 🚀 2. Configuración del Proyecto

#### Opción A - Modificar Proyecto del Paso 1:
- [ ] **Proyecto del Paso 1 abierto** en VS Code
- [ ] **Backup realizado** (opcional pero recomendado)

#### Opción B - Proyecto Nuevo:
- [ ] **Nuevo proyecto creado:** `dotnet new mvc -n LoginSimple`
- [ ] **VS Code abierto:** `code .` ejecutado correctamente
- [ ] **appsettings.json configurado** con credenciales de Supabase

### 🗃️ 3. Archivos Actualizados/Creados

#### Models/ (ACTUALIZADOS):
- [ ] **UserModel.cs** - Agregada propiedad `Password`
- [ ] **LoginViewModel.cs** - Agregada propiedad `Password` con validaciones
- [ ] **Namespace correcto:** Coincide con el nombre de tu proyecto

#### Controllers/ (REEMPLAZADO):
- [ ] **AuthController.cs** - Reemplaza UsuarioController.cs
- [ ] **Método ValidateCredentialsAsync** - Nueva lógica para validar username + password
- [ ] **Método Dashboard** - Nueva acción para usuarios logueados
- [ ] **Método Logout** - Nueva acción para cerrar sesión

#### Views/ (NUEVAS):
- [ ] **Carpeta Views/Auth/ creada** (en lugar de Views/Usuario/)
- [ ] **Views/Auth/Login.cshtml** - Formulario con campo password
- [ ] **Views/Auth/Dashboard.cshtml** - Página de bienvenida
- [ ] **@model LoginSimple.Models.LoginViewModel** - Referencias correctas

#### Configuración/ (ACTUALIZADA):
- [ ] **Program.cs** - Rutas actualizadas para AuthController
- [ ] **appsettings.json** - Igual que Paso 1 (sin cambios)

### 📝 4. Verificación de Sintaxis

```bash
# Verificar que compila sin errores
dotnet build
```

**Resultado esperado:** ✅ `Build succeeded. 0 Warning(s) 0 Error(s)`

**Si hay errores comunes:**
- [ ] **Namespaces:** Verificar que coincidan con el nombre del proyecto
- [ ] **Referencias:** `@model LoginSimple.Models.LoginViewModel` correcta
- [ ] **Propiedades:** `Password` agregada a UserModel y LoginViewModel

### 🚀 5. Ejecución

```bash
# Ejecutar el proyecto
dotnet run
```

**Resultado esperado:**
- [ ] **Aplicación inicia sin errores**
- [ ] **URLs mostradas:** Puertos disponibles
- [ ] **Logs del Paso 2:** "🚀 Aplicación del Paso 2 iniciada"
- [ ] **Rutas listadas:** /login, /dashboard, /logout

### 🌐 6. Pruebas en Navegador

#### Acceso Inicial:
- [ ] **Navegar a:** `https://localhost:5001` (o puerto mostrado)
- [ ] **Página carga:** Formulario "Login con Username + Password"
- [ ] **Campos visibles:** Username + Password + botón "Iniciar Sesión"
- [ ] **Sin errores 500:** No aparece página de error

#### Prueba 1 - Login Exitoso:
- [ ] **Ingresar:** Username: `admin`, Password: `123456`
- [ ] **Enviar formulario:** Click en "🔐 Iniciar Sesión"
- [ ] **URL cambia a:** `/Auth/Dashboard`
- [ ] **Resultado esperado:** "✅ Autenticación Exitosa" + Dashboard
- [ ] **Datos mostrados:** Username, email, hora de login

#### Prueba 2 - Password Incorrecta:
- [ ] **Volver al login:** Click en "🔍 Probar Otro Login"
- [ ] **Ingresar:** Username: `admin`, Password: `wrong123`
- [ ] **Resultado esperado:** "❌ Error: Usuario o contraseña incorrectos"
- [ ] **Permanecer en:** Página de login con mensaje de error

#### Prueba 3 - Usuario No Existe:
- [ ] **Ingresar:** Username: `noexiste`, Password: `123456`
- [ ] **Resultado esperado:** "❌ Error: Usuario o contraseña incorrectos"

#### Prueba 4 - Campos Vacíos:
- [ ] **Dejar ambos campos vacíos** y enviar
- [ ] **Resultado esperado:** Mensajes de validación para ambos campos

#### Prueba 5 - Logout:
- [ ] **Desde Dashboard:** Click en "🚪 Cerrar Sesión"
- [ ] **URL cambia a:** `/Auth/Login`
- [ ] **Resultado esperado:** Mensaje "Has cerrado sesión correctamente"

#### Prueba 6 - Acceso Directo a Dashboard:
- [ ] **Sin estar logueado:** Navegar a `/Auth/Dashboard`
- [ ] **Resultado esperado:** Redirección automática a `/Auth/Login`

### 🔍 7. Verificación de Logs

En la consola donde ejecutaste `dotnet run`, deberías ver:

```
✅ Logs esperados para login exitoso:
info: LoginSimple.Controllers.AuthController[0]
      AuthController configurado con Supabase URL: https://tu-proyecto.supabase.co
info: LoginSimple.Controllers.AuthController[0]
      Intentando login para usuario: admin
info: LoginSimple.Controllers.AuthController[0]
      Consultando Supabase: https://tu-proyecto.supabase.co/rest/v1/users?username=eq.admin
info: LoginSimple.Controllers.AuthController[0]
      Usuario encontrado: admin
info: LoginSimple.Controllers.AuthController[0]
      Validación de contraseña para admin: True
info: LoginSimple.Controllers.AuthController[0]
      Login exitoso para usuario: admin
```

```
✅ Logs esperados para login fallido:
info: LoginSimple.Controllers.AuthController[0]
      Intentando login para usuario: admin
info: LoginSimple.Controllers.AuthController[0]
      Usuario encontrado: admin
info: LoginSimple.Controllers.AuthController[0]
      Validación de contraseña para admin: False
warn: LoginSimple.Controllers.AuthController[0]
      Login fallido para usuario: admin
```

### 🐛 8. Solución de Problemas

#### Si aparece "Column 'password' does not exist":
- [ ] **Verificar SQL:** Ejecutar nuevamente el ALTER TABLE en Supabase
- [ ] **Verificar tabla:** Confirmar que la columna aparece en Table Editor
- [ ] **Revisar conexión:** Asegurar que estás conectado a la BD correcta

#### Si "Login always fails" (siempre falla):
- [ ] **Verificar passwords en BD:** `SELECT username, password FROM users`
- [ ] **Verificar comparación:** Logs muestran "Validación de contraseña: False"
- [ ] **Case sensitivity:** Probar exactamente "123456"
- [ ] **Espacios:** Verificar que no hay espacios extra en BD

#### Si "Page not found" (página no encontrada):
- [ ] **Verificar Program.cs:** Rutas apuntan a AuthController
- [ ] **Verificar controlador:** Se llama exactamente AuthController
- [ ] **Verificar vistas:** Carpeta Views/Auth/ existe

#### Si "Model binding error":
- [ ] **Verificar @model:** Referencias al namespace correcto
- [ ] **Verificar propiedades:** Password existe en LoginViewModel
- [ ] **Verificar form:** asp-action="Login" correcto

### 🎯 9. Pruebas Completas de Flujo

#### Flujo Completo Exitoso:
- [ ] **1.** Acceder a aplicación → Formulario de login
- [ ] **2.** Login con credenciales correctas → Dashboard
- [ ] **3.** Ver información del usuario logueado
- [ ] **4.** Logout → Volver al login con mensaje de confirmación
- [ ] **5.** Intentar acceder a dashboard → Redirección a login

#### Flujo de Errores:
- [ ] **1.** Login con password incorrecta → Error sin redirección
- [ ] **2.** Login con usuario inexistente → Error sin redirección
- [ ] **3.** Campos vacíos → Validación del lado cliente
- [ ] **4.** Acceso directo a rutas protegidas → Redirección a login

### 🎯 10. Comparación con Paso 1

Verifica que efectivamente evolucionaste desde el Paso 1:

| Aspecto | Paso 1 | Paso 2 | ✅ |
|---------|--------|---------|---|
| **Formulario** | Solo username | Username + password | [ ] |
| **Validación** | ¿Usuario existe? | ¿Credenciales correctas? | [ ] |
| **Base de Datos** | Sin password | Con password | [ ] |
| **Resultado** | "Encontrado/No encontrado" | Dashboard de usuario | [ ] |
| **Funcionalidad** | Solo verificación | Login + logout completo | [ ] |
| **URL final** | /Usuario/Resultado | /Auth/Dashboard | [ ] |

### 🎯 11. Confirmación Final

Si todas las verificaciones anteriores pasan:

- [ ] **✅ Paso 2 completado exitosamente**
- [ ] **🎓 Conceptos dominados:** Autenticación web, validación de credenciales, manejo de estados
- [ ] **🔐 Login funcional:** Sistema completo username + password
- [ ] **🚀 Listo para Paso 3:** Implementar hashing seguro

### 📞 12. Si Necesitas Ayuda

Si algo no funciona:

1. **Revisa logs paso a paso** - Te dicen exactamente qué está fallando
2. **Compara con Paso 1** - Asegúrate de que funcionaba antes
3. **Verifica cada checkbox** de esta lista uno por uno
4. **Compara tu código** con los archivos en la carpeta `codigo/`
5. **Verifica Supabase** directamente en su interfaz web

### 🎉 ¡Felicitaciones!

Si completaste toda esta lista, has logrado:

- ✅ **Evolucionar exitosamente** desde verificación de usuario a login real
- ✅ **Implementar autenticación** con username + password
- ✅ **Manejar estados de sesión** básicos con TempData
- ✅ **Crear UX completa** con login, dashboard y logout
- ✅ **Dominar conceptos** de desarrollo web moderno

**¡Estás listo para el Paso 3: Login Seguro con Hashing de Contraseñas!** 🔒

---

*¿Algún problema? Revisa los logs, verifica cada punto del checklist, y compara con el código de ejemplo. ¡Vas excelente!* 🎯
