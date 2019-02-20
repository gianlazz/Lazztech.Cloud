using Lazztech.Events.Dal.Dao;
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
            //modelBuilder.Entity<EventMentor>()
            //    .HasKey(x => new { x.EventId, x.MentorId });
            //modelBuilder.Entity<EventMentor>()
            //    .HasOne(x => x.Event)
            //    .WithMany(x => x.EventMentors)
            //    .HasForeignKey(x => x.EventId);
            //modelBuilder.Entity<EventMentor>()
            //    .HasOne(x => x.Mentor)
            //    .WithMany(x => x.EventMentors)
            //    .HasForeignKey(x => x.MentorId);

            modelBuilder.Entity<MentorInvite>()
                .HasOne(x => x.Event)
                .WithMany(x => x.MentorInvites)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Mentor>()
                .HasOne(x => x.MentorInvite)
                .WithOne(x => x.Mentor)
                .HasForeignKey<MentorInvite>(x => x.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Events.Dal.Dao.Event> Events { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Events.Dal.Dao.Sms> SmsMessages { get; set; }
        public DbSet<MentorRequest> MentorRequests { get; set; }
        public DbSet<MentorInvite> MentorInvites { get; set; }
        public DbSet<AudioUpload> AudioUploads { get; set; }
    }
}