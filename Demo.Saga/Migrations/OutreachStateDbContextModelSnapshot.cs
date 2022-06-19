﻿// <auto-generated />
using System;
using Demo.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace QES.Demo.Saga.Migrations
{
    [DbContext(typeof(OutreachStateDbContext))]
    partial class OutreachStateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachEmailAttempt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateAttempted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.Property<Guid>("OutreachStateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OutreachStateId");

                    b.ToTable("OutreachEmailAttempt");
                });

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachPhoneAttempt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateAttempted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.Property<Guid>("OutreachStateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OutreachStateId");

                    b.ToTable("OutreachPhoneAttempt");
                });

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasMaxLength(256)
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AttestationCompleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CompleteDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrentState")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<int>("ProviderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CorrelationId");

                    b.ToTable("OutreachState");
                });

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachEmailAttempt", b =>
                {
                    b.HasOne("QES.Demo.Saga.Model.OutreachState", "OutreachState")
                        .WithMany("OutreachEmailAttempts")
                        .HasForeignKey("OutreachStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OutreachState");
                });

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachPhoneAttempt", b =>
                {
                    b.HasOne("QES.Demo.Saga.Model.OutreachState", "OutreachState")
                        .WithMany("OutreachPhoneAttempts")
                        .HasForeignKey("OutreachStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OutreachState");
                });

            modelBuilder.Entity("QES.Demo.Saga.Model.OutreachState", b =>
                {
                    b.Navigation("OutreachEmailAttempts");

                    b.Navigation("OutreachPhoneAttempts");
                });
#pragma warning restore 612, 618
        }
    }
}
