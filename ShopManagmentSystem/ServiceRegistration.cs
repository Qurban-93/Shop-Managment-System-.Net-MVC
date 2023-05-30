using Microsoft.AspNetCore.Identity;
using ShopManagmentSystem.BackgroundService;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.Service;

namespace ShopManagmentSystem
{
    public static class ServiceRegistration
    {
        public static void ShopManagerServiceRegistration(this IServiceCollection service)
        {
            service.AddControllersWithViews();
            service.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.SignIn.RequireConfirmedAccount = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            service.AddScoped<IDisplacementService, DisplacementService>();
            service.AddScoped<IDeleteDisplacementService, DeleteDisplacementService>();
            service.AddScoped<IBoxOfficeService, BoxOfficeService>();          
            service.AddSignalR();

        }
    }
}
