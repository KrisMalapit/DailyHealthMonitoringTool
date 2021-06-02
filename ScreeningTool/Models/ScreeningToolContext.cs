using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScreeningTool.Models;

namespace ScreeningTool.Models
{
    public class ScreeningToolContext : DbContext
    {
        public ScreeningToolContext(DbContextOptions<ScreeningToolContext> options) : base(options)
        {

        }

      
        public DbSet<ScreenLogs> ScreenLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentHead> DepartmentHeads { get; set; }
        public DbSet<QREntry> QREntry { get; set; }
        public DbSet<QurantineDetector> QurantineDetectors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasIndex(p => new { p.Username,p.Status })
            //    .IsUnique();





        }

       



    }
}
