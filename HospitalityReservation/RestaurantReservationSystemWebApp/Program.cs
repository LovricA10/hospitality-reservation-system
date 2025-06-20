using Dao.Services;
using Dao.Repositories.Users;
using Dao.Repositories.Log;
using Dao.Repositories.HospitalityVenues;
using Dao.Repositories.Menu;
using Dao.Repositories.Reservations;
using Dao.Models;
using Microsoft.EntityFrameworkCore;
using Dao.Repositories.VenueMenu;
using Microsoft.AspNetCore.Authentication.Cookies;
using Dao.Repositories.HospitalityTypes;
using RestaurantReservationSystemWebApp.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/Forbidden";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<HospitalityReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<HospitalityVenueService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<HospitalityTypeService>();


builder.Services.AddScoped<IHospitalityVenueRepository, HospitalityVenueRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IVenueMenuRepository, VenueMenuRepository>();
builder.Services.AddScoped<IHospitalityTypeRepository, HospitalityTypeRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
