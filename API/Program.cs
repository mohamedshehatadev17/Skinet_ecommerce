using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository,Infrastructure.Repositories.ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await SeedDataContext.SeedDataAsync(context);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
}
app.Run();
