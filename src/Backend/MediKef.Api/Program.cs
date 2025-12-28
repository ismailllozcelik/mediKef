using Microsoft.EntityFrameworkCore;
using MediKef.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "MediKef LBYS API",
        Version = "v1",
        Description = "Laboratuvar Bilgi Yönetim Sistemi - LisBox Entegrasyonu"
    });
});

// PostgreSQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MediKefDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediKef LBYS API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Database migration on startup (development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MediKefDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("✅ Database migration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database migration failed: {ex.Message}");
    }
}

app.Run();
