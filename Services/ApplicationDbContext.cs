using Microsoft.EntityFrameworkCore;
using Services.Entity;

namespace Service
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}