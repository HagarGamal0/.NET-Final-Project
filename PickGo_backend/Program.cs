using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PickGo_backend;
using PickGo_backend.Configration;
using PickGo_backend.Context;
using PickGo_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DelieveryAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;               // No number required
    options.Password.RequiredLength = 6;                // Minimum length
    options.Password.RequireLowercase = false;          // Lowercase not required
    options.Password.RequireUppercase = false;          // Uppercase not required
    options.Password.RequireNonAlphanumeric = false;   // Special char not required
})
.AddEntityFrameworkStores<DelieveryAppContext>()
.AddDefaultTokenProviders();

builder.Services.AddAutoMapper(op =>
op.AddProfile<MapperConfig>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
