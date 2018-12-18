﻿// <auto-generated />
using System;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lazztech.ObsidianPresences.ClientFacade.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181218224443_MadeMentorPhoneNumberRequiredTest")]
    partial class MadeMentorPhoneNumberRequiredTest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTime");

                    b.Property<int?>("LocationId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Judge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId");

                    b.Property<int?>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("PersonId");

                    b.ToTable("Judges");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Mentor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId");

                    b.Property<int?>("PersonId");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("PersonId");

                    b.ToTable("Mentors");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Description");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<string>("LastName");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId");

                    b.Property<int?>("VenueRoomId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("VenueRoomId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.VenueRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("VenueRooms");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Volunteer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId");

                    b.Property<int?>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("PersonId");

                    b.ToTable("Volunteer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event", b =>
                {
                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Judge", b =>
                {
                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event")
                        .WithMany("Judges")
                        .HasForeignKey("EventId");

                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Mentor", b =>
                {
                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event")
                        .WithMany("Mentors")
                        .HasForeignKey("EventId");

                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Team", b =>
                {
                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event")
                        .WithMany("Teams")
                        .HasForeignKey("EventId");

                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.VenueRoom", "VenueRoom")
                        .WithMany()
                        .HasForeignKey("VenueRoomId");
                });

            modelBuilder.Entity("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Volunteer", b =>
                {
                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Event")
                        .WithMany("Volunteers")
                        .HasForeignKey("EventId");

                    b.HasOne("Lazztech.ObsidianPresences.ClientFacade.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
