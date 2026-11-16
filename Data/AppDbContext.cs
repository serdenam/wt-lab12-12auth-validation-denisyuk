using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_denisyuk.Models;

namespace wt_lab8_8razor_postgresql_denisyuk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    
    public DbSet<Book> Books => Set<Book>();
}