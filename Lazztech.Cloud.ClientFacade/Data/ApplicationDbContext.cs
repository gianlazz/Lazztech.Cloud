using Lazztech.Events.Dao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventMentor>()
                .HasKey(x => new { x.EventId, x.MentorId });
            modelBuilder.Entity<EventMentor>()
                .HasOne(x => x.Event)
                .WithMany(x => x.EventMentors)
                .HasForeignKey(x => x.EventId);
            modelBuilder.Entity<EventMentor>()
                .HasOne(x => x.Mentor)
                .WithMany(x => x.EventMentors)
                .HasForeignKey(x => x.MentorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventMentor> EventMentors { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Events.Dao.Sms> SmsMessages { get; set; }
    }
}