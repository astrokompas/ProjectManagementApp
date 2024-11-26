using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Project> Projects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Status)
                .IsRequired();

            modelBuilder.Entity<Equipment>()
                .Property(e => e.Status)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Project)
                .WithMany(p => p.AssignedEmployees)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Project)
                .WithMany(p => p.AssignedEquipment)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
