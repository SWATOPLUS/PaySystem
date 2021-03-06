﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaySystem.Models;
using PaySystem.Models.BusinessModels;

namespace PaySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<PaySystem.Models.BusinessModels.WorkLog> WorkLog { get; set; }

        public DbSet<PaySystem.Models.BusinessModels.Worker> Worker { get; set; }

        public DbSet<PaySystem.Models.BusinessModels.Card> Card { get; set; }

        public DbSet<PaySystem.Models.BusinessModels.FeeInfo> FeeInfo { get; set; }

        public DbSet<PaySystem.Models.BusinessModels.Check> Check { get; set; }
    }
}
