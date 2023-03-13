using CodeNotion.Academy.OrderScheduling.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "data source=db5eb13a5470;initial catalog=master;user id=sa;password=myPassword7941;TrustServerCertificate=True;");
    }
}