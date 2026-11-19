using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_denisyuk.Models;

namespace wt_lab12_12auth_validation_denisyuk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    
    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>(); // <-- ДОБАВЛЯЕМ ЭТУ СТРОКУ
}