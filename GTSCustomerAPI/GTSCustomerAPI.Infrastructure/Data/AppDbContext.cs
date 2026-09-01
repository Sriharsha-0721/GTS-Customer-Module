using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GTSCustomerAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustId);
            entity.Property(e => e.CustId).ValueGeneratedOnAdd();
            entity.Property(e => e.GID).IsRequired().HasMaxLength(9);
            entity.Property(e => e.rowguid).HasDefaultValueSql("NEWID()");
            entity.ToTable("Customers", "dbo");
        });
    }
}