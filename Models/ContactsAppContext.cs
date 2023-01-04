using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Models
{
   public class ContactsAppContext : DbContext
    {
        public ContactsAppContext (DbContextOptions<ContactsAppContext> options)
            : base(options)
        {
        }

        public DbSet<ContactsApp.Models.Contact> Contact { get; set; } = default!;
        public DbSet<ContactsApp.Models.EditHistory> EditHistories { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Contact>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
