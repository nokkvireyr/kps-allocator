﻿// <auto-generated />
using KPSAllocator.Modules.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KPSAllocator.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20240115113727_InitDatabase")]
    partial class InitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.15");

            modelBuilder.Entity("KPSAllocator.Modules.Database.PlayerDatabase", b =>
                {
                    b.Property<ulong>("steamID")
                        .HasColumnType("bigint");

                    b.Property<bool>("enableSniperCounterTerrorist")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("enableSniperTerrorist")
                        .HasColumnType("INTEGER");

                    b.Property<string>("primaryWeaponCounterTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("primaryWeaponTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("secondaryWeaponCounterTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("secondaryWeaponTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("smgWeaponCounterTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("smgWeaponTerrorist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("steamID");

                    b.ToTable("kps_allocator_players");
                });
#pragma warning restore 612, 618
        }
    }
}
