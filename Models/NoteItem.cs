using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Models
{
    public class NoteItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NoteUser")]
        public String UserId { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
