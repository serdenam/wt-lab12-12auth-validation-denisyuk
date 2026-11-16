using Microsoft.EntityFrameworkCore;
using Npgsql;
using wt_lab8_8razor_postgresql_denisyuk.Data;

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
        
        // Если порт не указан, используем стандартный 5432
        int dbPort = uri.Port > 0 ? uri.Port : 5432;

        var dbBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = dbPort,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = Uri.UnescapeDataString(userInfo[1]),
            SslMode = SslMode.Disable // Важно для внутренней сети Render
        };
        connectionString = dbBuilder.ConnectionString;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка парсинга DATABASE_URL: {ex.Message}");
    }
}

// Регистрируем сервисы
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// ==========================================
// 2. Автоматическое применение миграций при запуске
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ==========================================
// 3. Настройка HTTP pipeline (Middleware)
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// ==========================================
// 4. Запуск приложения на порту Render
// ==========================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");