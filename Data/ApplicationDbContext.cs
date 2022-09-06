using Microsoft.EntityFrameworkCore;
using NotesApp.Models;

namespace NotesApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteUser>().HasMany<NoteItem>(ni => ni.NoteItems).WithOne(nu => nu.NoteUser).HasForeignKey("NoteUserId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<NoteItem>? NoteItem { get; set; }
        public DbSet<NoteUser>? NoteUser { get; set; }
    }
}