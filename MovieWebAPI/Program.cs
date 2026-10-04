using Microsoft.EntityFrameworkCore;
using MovieWebAPI.Persistence;
using MovieWebAPI.Services;
using MovieWebAPI.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IMovieService, MovieService>();

var app = builder.Build();

await using(var serviceScope = app.Services.CreateAsyncScope())
await using(var dbContext = serviceScope.ServiceProvider.GetRequiredService<MovieDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!")
    .Produces(200, typeof(string));

app.MapMovieEndpoints();

app.Run();
