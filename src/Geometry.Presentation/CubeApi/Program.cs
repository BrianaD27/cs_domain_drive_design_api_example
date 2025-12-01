using Geometry.Application;
using Geometry.Domain.CubeModel;
using Geometry.Domain.CylinderInterface;
using Geometry.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger/OpenAPI UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Cube API",
        Version = "v1",
        Description = "A REST API for managing cube geometric entities.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Geometry API Support"
        }
    });
});

// Configure database context
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper();
if (environment == "DEVELOPMENT")
{
    // For development, use PostgreSQL:
    builder.Services.AddDbContext<GeometryDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // For production, use SQL Server:
    builder.Services.AddDbContext<GeometryDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Register repository implementations
builder.Services.AddScoped<ICubeRepository, CubeRepository>();
builder.Services.AddScoped<ICylinderRepository, CylinderRepository>();

// Register application services
builder.Services.AddScoped<CubeService>();
builder.Services.AddScoped<CylinderService>();

var app = builder.Build();

// Apply migrations in Development mode
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<GeometryDbContext>();
            // Try to apply migrations, if migrations don't exist, ensure database is created
            try
            {
                context.Database.Migrate();
            }
            catch
            {
                // If migrations don't exist, ensure database is created
                context.Database.EnsureCreated();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while setting up the database.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Enable Swagger UI for API documentation
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cube API v1");
        c.RoutePrefix = "swagger"; // Swagger UI will be available at /swagger
    });
}

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();

app.Run();
