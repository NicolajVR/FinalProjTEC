using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
	public class UserSubmissionDbContext : DbContext
	{
        // DbSet-egenskaber til at repræsentere tabeller i databasen
        public DbSet<UserSubmission> user_submission { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<Assignment> assignments { get; set; }

        public UserSubmissionDbContext(DbContextOptions<UserSubmissionDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration af forholdet mellem UserSubmission og Assignment
            modelBuilder.Entity<UserSubmission>()
            .HasOne(s => s.Assignment)
            .WithMany(c => c.userSubmissions)
            .HasForeignKey(s => s.assignment_id);
            // Konfiguration af forholdet mellem UserSubmission og Users
            modelBuilder.Entity<UserSubmission>()
           .HasOne(s => s.User)
           .WithMany(c => c.userSubmissions)
           .HasForeignKey(s => s.user_id);
        }
    }
}

