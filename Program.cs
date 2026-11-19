using Microsoft.EntityFrameworkCore;
using Npgsql;
using wt_lab12_12auth_validation_denisyuk.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Подключение к базе данных (PostgreSQL)
// ==========================================
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        int dbPort = uri.Port > 0 ? uri.Port : 5432;

        var dbBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = dbPort,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = Uri.UnescapeDataString(userInfo[1]),
            SslMode = SslMode.Disable
        };
        connectionString = dbBuilder.ConnectionString;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка парсинга DATABASE_URL: {ex.Message}");
    }
}

// СТРАХОВКА: Если строка подключения всё ещё пуста (локальный запуск), берем её напрямую
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = "Host=localhost;Port=5432;Database=wt_lab12;Username=postgres;Password=dashaden22"; // <-- Убедись, что пароль верный!
}

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");