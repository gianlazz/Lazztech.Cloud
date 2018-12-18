using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HackathonManager.PocoModels;
using Lazztech.ObsidianPresences.ClientFacade.Data.Entities;

namespace Lazztech.ObsidianPresences.ClientFacade.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mentor>()
                .Property(b => b.PhoneNumber)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        DbSet<VenueRoom> VenueRooms { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Mentor> Mentors { get; set; }
        DbSet<Judge> Judges { get; set; }
    }
}
