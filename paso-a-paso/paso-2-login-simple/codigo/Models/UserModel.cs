using System.ComponentModel.DataAnnotations;

namespace LoginSimple.Models
{
    /// <summary>
    /// Modelo que representa la estructura de la tabla 'users' en Supabase
    /// ACTUALIZADO: Ahora incluye password para autenticación
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
        
        // 🆕 NUEVA: Corresponde a la columna 'password' en Supabase
        // NOTA: En este paso usamos texto plano para simplicidad
        // En el Paso 3 implementaremos hashing seguro
        [Required]
        public string Password { get; set; } = string.Empty;
        
        // Corresponde a la columna 'created_at' en Supabase
        public DateTime CreatedAt { get; set; }
        
        // Corresponde a la columna 'is_active' en Supabase
        public bool IsActive { get; set; } = true;
    }
}
