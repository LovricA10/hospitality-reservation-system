using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Dao.Models;
using Microsoft.EntityFrameworkCore;
using Dao.Services;
using WebApp.AutoMapper;
using Dao.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register your services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<HospitalityVenueService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<LogService>();

// Register generic repository
builder.Services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));

builder.Services.AddDbContext<HospitalityReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// service for manipulate db

// Configure JWT security services
var secureKey = builder.Configuration["Jwt:SecureKey"];
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => {
        var Key = Encoding.UTF8.GetBytes(secureKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1",
        new OpenApiInfo { Title = "Web APP", Version = "v1" });

    option.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter valid JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
