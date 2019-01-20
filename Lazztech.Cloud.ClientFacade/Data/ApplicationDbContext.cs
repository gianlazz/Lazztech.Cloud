using Lazztech.Cloud.ClientFacade.Data.Entities;
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
            modelBuilder.Entity<MentorEntity>()
                .Property(b => b.PhoneNumber)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        private DbSet<VenueRoomEntity> VenueRooms { get; set; }
        private DbSet<EventEntity> Events { get; set; }
        private DbSet<PersonEntity> People { get; set; }
        private DbSet<LocationEntity> Locations { get; set; }
        private DbSet<MentorEntity> Mentors { get; set; }
        private DbSet<JudgeEntity> Judges { get; set; }
    }
}