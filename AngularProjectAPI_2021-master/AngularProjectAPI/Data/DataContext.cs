using AngularProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<Group> Groups { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<GroupUser>().ToTable("GroupUser");
            modelBuilder.Entity<Group>().ToTable("Group");
            
        }
    }
}
