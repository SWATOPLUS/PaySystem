using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaySystem.Models;
using PaySystem.Models.BusinessModels;
using System;

namespace PaySystem.Data
{
    public class ForeignDbContext : DbContext
    {
        public ForeignDbContext(DbContextOptions<ForeignDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
            //throw new InvalidOperationException("This context is read-only.");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<PaySystem.Models.BusinessModels.GlobalWorker> Worker { get; set; }
    }
}
