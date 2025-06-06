# 🧩 Pistas para Implementar Login (Solo Validación) - Piensa Primero

## 🎯 Objetivo
Crear un sistema que permita a un usuario existente iniciar sesión con username y password, y mostrar una página de bienvenida si las credenciales son correctas.

## 📋 Prerrequisitos
- Ya tienes usuarios creados en tu tabla de Supabase (puedes crearlos manualmente)
- La tabla `users` ya existe con la estructura correcta
- Ya sabes cómo funciona el HomeController con Supabase

## 🤔 Preguntas Guía para Reflexionar

### 1. ¿Qué necesito crear en mi proyecto?
**Piensa:** Si solo quiero validar un login, ¿qué archivos mínimos necesito?
- **Pista 1:** Un modelo para el formulario de login
- **Pista 2:** Un controlador nuevo (AuthController) similar al HomeController
- **Pista 3:** Vistas para mostrar el formulario y la página de éxito
- **Pista 4:** Lógica de validación dentro del controlador

### 2. Estructura de Carpetas
**Reflexiona:** ¿Cómo organizo mi código siguiendo el patrón MVC?
```
¿Qué carpetas necesito crear?
├── Models/ (¿qué modelos?)
├── Controllers/ (¿qué controlador?)
└── Views/ (¿qué vistas?)
```

### 3. El Modelo de Login
**Pregunta:** ¿Qué datos necesito recolectar del usuario para hacer login?
- **Pista:** Solo los datos mínimos para identificar al usuario
- **Reflexión:** ¿Qué validaciones debería tener cada campo?

### 4. Conexión con Supabase (Igual que HomeController)
**Pregunta:** ¿Cómo me conecto a Supabase desde mi nuevo controlador?
- **Pista 1:** Usa el mismo patrón que ya tienes en HomeController
- **Pista 2:** Inyecta `IHttpClientFactory` e `IConfiguration` en tu constructor
- **Pista 3:** Configura los headers de Supabase igual que en HomeController
- **Reflexión:** ¿Qué configuraciones necesito del appsettings.json?

### 5. La Lógica de Validación
**Pregunta:** ¿Cómo verifico si las credenciales son correctas?

**Proceso a pensar:**
1. Recibo username y password del formulario
2. Busco el usuario en Supabase por username (usando HttpClient)
3. Comparo la contraseña ingresada con la guardada
4. ¿Qué hago si coinciden? ¿Y si no coinciden?

**Reflexión importante:** ¿Cómo comparo contraseñas si están hasheadas en la base de datos?

### 6. Flujo de la Aplicación
**Piensa en el flujo:**
1. Usuario accede a `/Auth/Login`
2. Se muestra formulario de login
3. Usuario completa y envía formulario
4. Sistema valida credenciales
5. Si son correctas → página de bienvenida
6. Si son incorrectas → mensaje de error

### 7. Consulta a Supabase
**Pregunta:** ¿Cómo busco un usuario específico en Supabase?
- **Pista 1:** Usar la API REST de Supabase (igual que en HomeController)
- **Pista 2:** Filtrar por username en la URL
- **Investigar:** ¿Cómo se hace un filtro en la URL de Supabase? (eq.valor)

### 8. Manejo de Contraseñas
**Reflexión crítica:** Para este ejercicio, ¿cómo comparo las contraseñas?
- **Pista:** Usaremos comparación directa para simplicidad (texto plano)
- **En producción:** Se usarían funciones de hash como BCrypt
- **Para aprender:** Nos enfocamos en MVC y Supabase, no en seguridad avanzada

## 🔍 Investigación Recomendada

### APIs de Supabase (Ya lo sabes del HomeController)
- ¿Cómo filtrar resultados? (`?campo=eq.valor`)
- ¿Qué headers necesito enviar? (apikey, Authorization)
- ¿Cómo manejo las respuestas JSON?

### Comparación de Contraseñas Simplificada
- ¿Cómo comparo contraseñas en este ejercicio?
- **Versión simple:** Comparación directa de strings (`password1 == password2`)
- **En producción:** Funciones de hash seguras

## 💡 Tips para Empezar

1. **Copia el patrón del HomeController:** Usa la misma estructura de constructor e inyección
2. **Comienza simple:** Primero haz que funcione con contraseñas en texto plano, luego mejora
3. **Testa paso a paso:** Verifica cada parte por separado
4. **Usa usuarios de prueba:** Crea usuarios manualmente en Supabase para probar

## 🎯 Entregables Esperados

Al final deberías tener:
- AuthController similar al HomeController
- Formulario de login funcional
- Validación de credenciales contra Supabase
- Página de éxito para usuarios válidos
- Mensajes de error para credenciales incorrectas

## 🤝 ¿Cuándo usar la Guía Completa?

Usa `solucion_login_completa.md` cuando:
- Ya hayas intentado implementarlo por tu cuenta
- Tengas dudas específicas sobre algún paso
- Quieras verificar tu implementación
- Necesites ver ejemplos de código concretos

¡Recuerda: El mejor aprendizaje viene de intentarlo primero! 🚀