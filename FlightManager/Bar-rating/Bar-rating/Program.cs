using Bar_rating.Data.Entities;
using Bar_rating.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bar_rating
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            {
                var builder = WebApplication.CreateBuilder(args);

                var connectionString = builder.Configuration.GetConnectionString("DbConnnectionString");


                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                builder.Services.AddIdentity<AppUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredUniqueChars = 0;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                    )
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


                // Add services to the container.
                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                using (var scope = app.Services.CreateScope())
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    var roles = new[] { "Admin", "Member" };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }

                using (var scope = app.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                    string email = "admin@admin.com";
                    string password = "admin1234";

                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new AppUser()
                        {
                            Name = "Admin",
                            LastName = "Admin",
                            UserName = email,
                            Email = email,
                        };

                        await userManager.CreateAsync(user, password);

                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }


                app.Run();
            }
        }
    }
}
