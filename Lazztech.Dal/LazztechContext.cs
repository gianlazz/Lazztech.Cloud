using Lazztech.Dal.DBModels;
using System;
using System.Data.Entity;

namespace Lazztech.Dal
{
    public class LazztechContext : DbContext
    {
        DbSet<VenueRoom> VenueRooms { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Mentor> Mentors { get; set; }
        DbSet<Judge> Judges { get; set; }
    }
}
