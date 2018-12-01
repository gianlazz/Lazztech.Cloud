using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HackathonManager.DTO;
using HackathonManager;
using HackathonManager.PocoModels;

namespace Lazztech.ObsidianPresences.ClientFacade.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HackathonManager.DTO.Mentor> Mentor { get; set; }
        public DbSet<HackathonManager.Judge> Judge { get; set; }
        public DbSet<HackathonManager.PocoModels.Team> Team { get; set; }
    }
}
