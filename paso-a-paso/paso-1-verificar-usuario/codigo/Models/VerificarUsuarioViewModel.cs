using System.ComponentModel.DataAnnotations;

namespace VerificarUsuario.Models
{
    /// <summary>
    /// ViewModel para el formulario de verificación de usuario
    /// Solo contiene el campo username que necesitamos verificar
    /// </summary>
    public class VerificarUsuarioViewModel
    {
        // Campo para ingresar el nombre de usuario a verificar
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        // Propiedad para mostrar mensajes de error o información
        public string Mensaje { get; set; } = string.Empty;

        // Indica si el usuario fue encontrado o no
        public bool UsuarioEncontrado { get; set; } = false;
    }
}
