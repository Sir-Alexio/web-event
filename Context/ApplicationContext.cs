using Microsoft.EntityFrameworkCore;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events{ get; set; } = null!;
        public DbSet<Parameter> Parameters { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=WebEvent;Trusted_Connection=True;TrustServerCertificate=True;encrypt=false");
        }

        protected override void OnModelCreating(ModelBuilder model)
        {


            model.Entity<Parameter>()
                .HasOne(x=>x.Event)
                .WithMany(x=>x.Parameters)
                .HasForeignKey(k=> k.EventId);

            model.Entity<User>()
                .HasMany(m => m.CreatedEvents)
                .WithMany(x => x.RegistedUsers)
                .UsingEntity(j => j.ToTable("UserEvent"));
        }
    }
}
