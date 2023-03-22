using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using TreeStructure.Models;

public class TreeContext : DbContext
{
    public DbSet<Node> Nodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb; database=NodeTree; integrated security=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Node>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(25);
    }
}