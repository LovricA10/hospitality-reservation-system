using MVC.AutoMapper;
using Dao.Services;
using Dao.Repositories.Users;
using Dao.Repositories.Log;
using Dao.Repositories.HospitalityVenues;
using Dao.Repositories.Menu;
using Dao.Repositories.Reservations;
using Dao.Models;
using Microsoft.EntityFrameworkCore;
using Dao.Repositories.VenueMenu;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add DbContext
builder.Services.AddDbContext<HospitalityReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<HospitalityVenueService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LogService>();

// Register repositories
builder.Services.AddScoped<IHospitalityVenueRepository, HospitalityVenueRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IVenueMenuRepository, VenueMenuRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
