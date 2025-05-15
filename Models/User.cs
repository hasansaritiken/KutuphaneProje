using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 