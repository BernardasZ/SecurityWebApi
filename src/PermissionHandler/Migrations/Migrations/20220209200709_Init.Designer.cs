﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migrations.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220209200709_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Models.Action", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActionUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ControllerId")
                        .HasColumnType("bigint");

                    b.Property<string>("HttpType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ControllerId");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("Domain.Models.Controller", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ControllerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ControllerUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ServerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Controllers");
                });

            modelBuilder.Entity("Domain.Models.Property", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ActionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("NavigationId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReferenceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("NavigationId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("Domain.Models.Server", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApiName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApiUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Domain.Models.Action", b =>
                {
                    b.HasOne("Domain.Models.Controller", "Controller")
                        .WithMany("Actions")
                        .HasForeignKey("ControllerId");

                    b.Navigation("Controller");
                });

            modelBuilder.Entity("Domain.Models.Controller", b =>
                {
                    b.HasOne("Domain.Models.Server", "Server")
                        .WithMany("Controllers")
                        .HasForeignKey("ServerId");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Domain.Models.Property", b =>
                {
                    b.HasOne("Domain.Models.Action", "Action")
                        .WithMany("Properties")
                        .HasForeignKey("ActionId");

                    b.HasOne("Domain.Models.Property", "Navigation")
                        .WithMany("Properties")
                        .HasForeignKey("NavigationId");

                    b.Navigation("Action");

                    b.Navigation("Navigation");
                });

            modelBuilder.Entity("Domain.Models.Action", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("Domain.Models.Controller", b =>
                {
                    b.Navigation("Actions");
                });

            modelBuilder.Entity("Domain.Models.Property", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("Domain.Models.Server", b =>
                {
                    b.Navigation("Controllers");
                });
#pragma warning restore 612, 618
        }
    }
}
