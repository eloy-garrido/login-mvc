using System.ComponentModel.DataAnnotations;

namespace VerificarUsuario.Models
{
    /// <summary>
    /// Modelo que representa la estructura de la tabla 'users' en Supabase
    /// Solo las propiedades básicas necesarias para verificar existencia
    /// </summary>
    public class UserModel
    {
        // Corresponde a la columna 'id' en Supabase
        public int Id { get; set; }
        
        // Corresponde a la columna 'username' en Supabase
        [Required]
        public string Username { get; set; } = string.Empty;
        
        // Corresponde a la columna 'email' en Supabase
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        // Corresponde a la columna 'created_at' en Supabase
        public DateTime CreatedAt { get; set; }
        
        // Corresponde a la columna 'is_active' en Supabase
        public bool IsActive { get; set; } = true;
    }
}
