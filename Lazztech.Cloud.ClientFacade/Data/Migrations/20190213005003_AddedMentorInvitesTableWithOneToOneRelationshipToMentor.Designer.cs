﻿// <auto-generated />
using System;
using Lazztech.Cloud.ClientFacade.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190213005003_AddedMentorInvitesTableWithOneToOneRelationshipToMentor")]
    partial class AddedMentorInvitesTableWithOneToOneRelationshipToMentor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateOfEvent");

                    b.Property<string>("Name");

                    b.Property<int>("OrganizationId");

                    b.HasKey("EventId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Mentor", b =>
                {
                    b.Property<int>("MentorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<int?>("EventId");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<bool>("IsAvailable");

                    b.Property<bool>("IsPresent");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("ProfessionalTitle");

                    b.HasKey("MentorId");

                    b.HasIndex("EventId");

                    b.ToTable("Mentors");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.MentorInvite", b =>
                {
                    b.Property<int>("MentorInviteId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Accepted");

                    b.Property<DateTime>("DateTimeWhenCreated");

                    b.Property<DateTime?>("DateTimeWhenViewed");

                    b.Property<string>("InviteLink");

                    b.Property<int?>("MentorId");

                    b.HasKey("MentorInviteId");

                    b.HasIndex("MentorId");

                    b.ToTable("MentorInvites");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.MentorRequest", b =>
                {
                    b.Property<int>("MentorRequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTimeOfRequest");

                    b.Property<DateTime?>("DateTimeWhenProcessed");

                    b.Property<int?>("InboundSmsSmsId");

                    b.Property<int>("MentorId");

                    b.Property<TimeSpan>("MentoringDuration");

                    b.Property<int?>("OutboundSmsSmsId");

                    b.Property<bool>("RequestAccepted");

                    b.Property<TimeSpan>("RequestTimeout");

                    b.Property<bool>("TimedOut");

                    b.Property<string>("UniqueRequesteeId");

                    b.HasKey("MentorRequestId");

                    b.HasIndex("InboundSmsSmsId");

                    b.HasIndex("MentorId");

                    b.HasIndex("OutboundSmsSmsId");

                    b.ToTable("MentorRequests");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Organization", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Sms", b =>
                {
                    b.Property<int>("SmsId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateTimeWhenProcessed");

                    b.Property<string>("FromPhoneNumber");

                    b.Property<string>("MessageBody");

                    b.Property<string>("Sid");

                    b.Property<string>("ToPhoneNumber");

                    b.HasKey("SmsId");

                    b.ToTable("SmsMessages");
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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Event", b =>
                {
                    b.HasOne("Lazztech.Events.Dal.Dao.Organization", "Organization")
                        .WithMany("Events")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.Mentor", b =>
                {
                    b.HasOne("Lazztech.Events.Dal.Dao.Event", "Event")
                        .WithMany("Mentors")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.MentorInvite", b =>
                {
                    b.HasOne("Lazztech.Events.Dal.Dao.Mentor", "Mentor")
                        .WithMany()
                        .HasForeignKey("MentorId");
                });

            modelBuilder.Entity("Lazztech.Events.Dal.Dao.MentorRequest", b =>
                {
                    b.HasOne("Lazztech.Events.Dal.Dao.Sms", "InboundSms")
                        .WithMany()
                        .HasForeignKey("InboundSmsSmsId");

                    b.HasOne("Lazztech.Events.Dal.Dao.Mentor", "Mentor")
                        .WithMany()
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lazztech.Events.Dal.Dao.Sms", "OutboundSms")
                        .WithMany()
                        .HasForeignKey("OutboundSmsSmsId");
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
