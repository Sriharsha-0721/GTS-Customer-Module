using Microsoft.EntityFrameworkCore;
using GTS.Domain.Entities;

namespace GTS.Infrastructure.Data
{
    public class GTSDbContext : DbContext
    {
        public GTSDbContext(DbContextOptions<GTSDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tell EF Core this table already exists — don't create it
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(e => e.CustId);

                entity.Property(e => e.rowguid)
                    .HasDefaultValueSql("newsequentialid()");

                entity.Property(e => e.OSSFlag)
                    .HasDefaultValue(false);

                entity.Property(e => e.MonSeq).HasDefaultValue(0);
                entity.Property(e => e.TueSeq).HasDefaultValue(0);
                entity.Property(e => e.WedSeq).HasDefaultValue(0);
                entity.Property(e => e.ThuSeq).HasDefaultValue(0);
                entity.Property(e => e.FriSeq).HasDefaultValue(0);
                entity.Property(e => e.SatSeq).HasDefaultValue(0);
                entity.Property(e => e.SunSeq).HasDefaultValue(0);
            });
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}