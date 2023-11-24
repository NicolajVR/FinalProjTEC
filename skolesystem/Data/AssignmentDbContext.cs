using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
	public class AssignmentDbContext : DbContext
	{
        // DbSet til at repræsentere tabeller i databasen
        public DbSet<Classe> Classe { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration af forholdet mellem Assignment og Classe
            modelBuilder.Entity<Assignment>()
            .HasOne(s => s.Classe)
            .WithMany(c => c.assignments)
            .HasForeignKey(s => s.class_id);

            // Konfiguration af forholdet mellem Assignment og Subjects
            modelBuilder.Entity<Assignment>()
            .HasOne(s => s.Subjects)
            .WithMany(c => c.assignments)
            .HasForeignKey(s => s.subject_id);

        }
    }
}

