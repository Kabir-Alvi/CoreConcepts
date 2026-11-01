using CoreConcepts.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreConcepts.Data;

public static class IdentitySeed
{
    public const string AdminRole = "Admin";
    public const string UserRole = "Normal User";

    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { AdminRole, UserRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                EnsureSucceeded(roleResult, $"creating the {role} role");
            }
        }

        var adminUsername = configuration["AdminSeed:Username"];
        var adminEmail = configuration["AdminSeed:Email"];
        var adminPassword = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(adminUsername) ||
            string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Admin seed configuration is missing. Set AdminSeed__Username, AdminSeed__Email, and AdminSeed__Password.");
        }

        var admin = await userManager.FindByNameAsync(adminUsername);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                FullName = "Core Concepts Administrator",
                EmailConfirmed = true
            };

            var userResult = await userManager.CreateAsync(admin, adminPassword);
            EnsureSucceeded(userResult, "creating the Admin user");
        }

        if (!await userManager.IsInRoleAsync(admin, AdminRole))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, AdminRole);
            EnsureSucceeded(roleResult, "assigning the Admin role");
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string operation)
    {
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Identity error while {operation}: {errors}");
        }
    }
}
