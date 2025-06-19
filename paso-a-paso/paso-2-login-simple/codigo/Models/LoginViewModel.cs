using System.ComponentModel.DataAnnotations;

namespace LoginSimple.Models
{
    /// <summary>
    /// ViewModel para el formulario de login (username + password)
    /// EVOLUCIÓN: Paso 1 solo tenía username, ahora agregamos password
    /// </summary>
    public class LoginViewModel
    {
        // Campo para el nombre de usuario (IGUAL que Paso 1)
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        // 🆕 NUEVO: Campo para la contraseña
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]  // Esto hace que el campo se muestre como password en HTML
        [Display(Name = "Contraseña")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        // Propiedad para mostrar mensajes de error personalizados (IGUAL que Paso 1)
        public string ErrorMessage { get; set; } = string.Empty;

        // 🔄 ACTUALIZADO: Cambio de nombre para ser más específico
        // Paso 1: UsuarioEncontrado → Paso 2: LoginExitoso
        public bool LoginExitoso { get; set; } = false;
    }
}
