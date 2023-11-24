using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
	public class EnrollmentDbContext : DbContext
	{
        // DbSet-egenskaber til at repræsentere tabeller i databasen
        public DbSet<Enrollments> enrollments { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<Classe> classe { get; set; }
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration af forholdet mellem Enrollments og Classe
            modelBuilder.Entity<Enrollments>()
            .HasOne(s => s.Classe)
            .WithMany(c => c.enrollments)
            .HasForeignKey(s => s.class_id);
            // Konfiguration af forholdet mellem Enrollments og Users
            modelBuilder.Entity<Enrollments>()
           .HasOne(s => s.User)
           .WithMany(c => c.enrollments)
           .HasForeignKey(s => s.user_id);
        }
    }
}

