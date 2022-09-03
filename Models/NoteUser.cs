using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
    public class NoteUser : IdentityUser
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
