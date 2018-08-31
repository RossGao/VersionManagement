﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VersionManagement.Repositories;

namespace VersionManagement.Migrations
{
    [DbContext(typeof(VersionContext))]
    [Migration("20180829035022_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("VersionManagement.Models.VersionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Applicant")
                        .IsRequired();

                    b.Property<string>("CommitIds")
                        .IsRequired();

                    b.Property<string>("DetailNote");

                    b.Property<string>("Iteration")
                        .IsRequired();

                    b.Property<string>("TaskTitle")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.Property<Guid?>("VersionId");

                    b.HasKey("Id");

                    b.HasIndex("VersionId");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("VersionManagement.Models.VersionInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<int>("Department");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("ReleaseNote");

                    b.Property<int>("Status");

                    b.Property<string>("VersionNumber")
                        .IsRequired();

                    b.Property<string>("VersionTitle")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("VersionManagement.Models.VersionDetail", b =>
                {
                    b.HasOne("VersionManagement.Models.VersionInfo", "Version")
                        .WithMany("Detailes")
                        .HasForeignKey("VersionId");
                });
#pragma warning restore 612, 618
        }
    }
}