using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVC_Blog.Data;
using MVC_Blog.Models;
using MVC_Blog.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, Role>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true; // Require email confirmation
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();

        // Register email settings and email service
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddSingleton<IEmailService, EmailService>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

        var app = builder.Build();

        // Seed roles and admin user
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedRolesAndAdminUser(roleManager, userManager);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication(); // Identity authentication middleware
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    // Method to seed roles and create a default admin user
    public static async Task SeedRolesAndAdminUser(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager)
    {
        // Seed roles
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new Role { Name = "User" });

        if (!await roleManager.RoleExistsAsync("Author"))
            await roleManager.CreateAsync(new Role { Name = "Author" });

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new Role { Name = "Admin" });

        // Create a default admin user
        var adminUser = new ApplicationUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Admin",
        };

        string adminPassword = "Admin@123";
        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
