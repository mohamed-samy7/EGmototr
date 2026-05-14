using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EGYmotor.Models;

namespace EGYmotor.Data
{
    public class EGYmotorContext : DbContext
    {
        public EGYmotorContext(DbContextOptions<EGYmotorContext> options)
            : base(options)
        {
        }

        public DbSet<EGYmotor.Models.RegisterUser> RegisterUser { get; set; }
        public DbSet<EGYmotor.Models.Request> Requests { get; set; }
        public DbSet<EGYmotor.Models.Feedback> Feedback { get; set; }
        public DbSet<EGYmotor.Models.Payment> Payments { get; set; }
        public DbSet<EGYmotor.Models.Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feedback>()
                .HasIndex(f => f.UserId)
                .IsUnique(false);

            modelBuilder.Entity<Request>()
                .HasIndex(r => r.UserId)
                .IsUnique(false);
        }
    }
}