using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Set the one-to-many relationship between Person and Skill
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>()
                .HasMany(p => p.skills)
                .WithOne(s => s.person)
                .IsRequired();
        }
    }
}
