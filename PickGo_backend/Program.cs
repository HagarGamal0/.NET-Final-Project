using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PickGo_backend;
using PickGo_backend.Configration;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Services;

using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container
builder.Services.AddControllers();
builder.Services.AddScoped<UnitOfWork>();

// Database
builder.Services.AddDbContext<DelieveryAppContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<DelieveryAppContext>()
.AddDefaultTokenProviders();

// configure cors policy

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// AutoMapper
builder.Services.AddAutoMapper(op => op.AddProfile<MapperConfig>());

// JWT Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])
            ),
        };
    });

builder.Services.AddHostedService<CourierTrackingService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<IGraphHopperService, GraphHopperService>();
builder.Services.AddSignalR();
builder.Services.AddScoped<OrderNotificationService>();






//builder.Services.AddHangfire(configuration => configuration
//    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//    .UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
//    {
//        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
//        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
//        QueuePollInterval = TimeSpan.FromSeconds(15),
//        UseRecommendedIsolationLevel = true,
//        DisableGlobalLocks = true
//    })
//);
//builder.Services.AddHangfireServer();









builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT like this: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();



app.UseCors("AllowAll");
// Middleware pipeline
// (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PickGo API V1");
        c.RoutePrefix = "swagger"; 
    });
//}

app.UseHttpsRedirection();
app.UseCors();
// ✅ Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<CourierLocationHub>("/hubs/courier");

    app.Run();
