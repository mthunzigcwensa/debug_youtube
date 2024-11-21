using youtube.Application.Common.Interfaces;
using youtube.Domain.Entities;
using youtube.Infrastrcture.Data;
using youtube.Infrastrcture.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using youtube.Application.Services.Interfaces;
using youtube.Application.Services.Implementation;
using youtube.web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("youtube.Infrastrcture")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LoginPath = "/Account/Login";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IChannelService, ChannelService>(); 
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Index}/{id?}");

app.Run();