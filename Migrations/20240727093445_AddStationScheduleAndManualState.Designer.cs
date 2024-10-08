﻿// <auto-generated />
using System;
using CarWashManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarWashManagementSystem.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240727093445_AddStationScheduleAndManualState")]
    partial class AddStationScheduleAndManualState
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CarWashManagementSystem.Models.FiscSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TurnOffTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("TurnOnTime")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("FiscSchedule");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsExcluded")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ManualFiscState")
                        .HasColumnType("tinyint(1)");

                    b.Property<short>("StationNumber")
                        .HasColumnType("smallint");

                    b.Property<int>("StationTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationTypeId");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.StationAllowedIp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId")
                        .IsUnique();

                    b.ToTable("StationAllowedIps");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.StationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StationTypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("StationTypes");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionSourceId")
                        .HasColumnType("int");

                    b.Property<short>("Value")
                        .HasColumnType("smallint");

                    b.Property<bool>("WasFiscalized")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("TransactionSourceId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.TransactionSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ShouldBeFiscalized")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SourceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TransactionSources");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.Station", b =>
                {
                    b.HasOne("CarWashManagementSystem.Models.StationType", "StationType")
                        .WithMany("Stations")
                        .HasForeignKey("StationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StationType");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.StationAllowedIp", b =>
                {
                    b.HasOne("CarWashManagementSystem.Models.Station", "Station")
                        .WithOne("AllowedIp")
                        .HasForeignKey("CarWashManagementSystem.Models.StationAllowedIp", "StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.Transaction", b =>
                {
                    b.HasOne("CarWashManagementSystem.Models.Station", "Station")
                        .WithMany("Transactions")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarWashManagementSystem.Models.TransactionSource", "TransactionSource")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");

                    b.Navigation("TransactionSource");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.Station", b =>
                {
                    b.Navigation("AllowedIp")
                        .IsRequired();

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.StationType", b =>
                {
                    b.Navigation("Stations");
                });

            modelBuilder.Entity("CarWashManagementSystem.Models.TransactionSource", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
