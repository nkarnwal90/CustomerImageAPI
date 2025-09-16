using CustomerImageAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CustomerImageAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ImageEntry> Images => Set<ImageEntry>();
}
