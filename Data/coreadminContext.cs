using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using api.Models;
namespace api.Data
{
    public partial class coreadminContext : DbContext
    {
        public coreadminContext()
        {
        }

        public coreadminContext(DbContextOptions<coreadminContext> options)
            : base(options)
        {
        }

       
        public virtual DbSet<Employee> Employee { get; set; }
    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // optionsBuilder.UseSqlServer("Server=.;Database=coreadmin;Trusted_Connection=True;Connect Timeout=1200;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
