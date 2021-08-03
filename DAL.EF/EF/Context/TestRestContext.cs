using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DAL.EF.EF.Entities;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DAL.EF.EF.Context
{
    public partial class TestRestContext : DbContext
    {
        public TestRestContext()
        {
        }

        public TestRestContext(DbContextOptions<TestRestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicPerformance> AcademicPerformances { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<AcademicPerformance>(entity =>
            {
                entity.ToTable("AcademicPerformance");

                entity.Property(e => e.code).HasColumnType("character varying");

                entity.Property(e => e.description).HasColumnType("character varying");

                entity.Property(e => e.name).HasColumnType("character varying");
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.ToTable("Sex");

                entity.Property(e => e.code).HasColumnType("character varying");

                entity.Property(e => e.description).HasColumnType("character varying");

                entity.Property(e => e.name).HasColumnType("character varying");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.firstName).HasColumnType("character varying");

                entity.Property(e => e.secondName).HasColumnType("character varying");

                entity.Property(e => e.surName).HasColumnType("character varying");

                entity.HasOne(d => d.idAcademicPerformanceNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.idAcademicPerformance)
                    .HasConstraintName("R_255");

                entity.HasOne(d => d.idSexNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.idSex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_254");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
