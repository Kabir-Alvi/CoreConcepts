using CoreConcepts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Data;

public static class IdentitySeed
{
    public const string AdminRole = "Admin";
    public const string UserRole = "Normal User";

    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // 1. Roles
        foreach (var role in new[] { AdminRole, UserRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                EnsureSucceeded(roleResult, $"creating the {role} role");
            }
        }

        // 2. Admin User
        var adminUsername = configuration["AdminSeed:Username"] ?? "admin";
        var adminEmail = configuration["AdminSeed:Email"] ?? "admin@coreconcepts.com";
        var adminPassword = configuration["AdminSeed:Password"] ?? "Admin@CoreConcepts2026!";

        var admin = await userManager.FindByNameAsync(adminUsername);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                FullName = "CoreConcepts Administrator",
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

        // 3. Seed Sample Data if empty
        await SeedInitialDataAsync(dbContext);
    }

    private static async Task SeedInitialDataAsync(ApplicationDbContext db)
    {
        // Seed Leadership Members if empty
        if (!await db.CompanyLeaders.AnyAsync())
        {
            db.CompanyLeaders.AddRange(
                new CompanyLeader
                {
                    FullName = "Tanvir Ahmed",
                    Designation = "Chief Executive Officer (CEO)",
                    HierarchyRank = 1,
                    Speech = "At CoreConcepts, we believe that world-class software is engineered when passionate young minds meet robust architectural standards. Our mission is to solve real-world industry problems while cultivating high-impact software leaders.",
                    Email = "ceo@coreconcepts.com",
                    LinkedInUrl = "https://linkedin.com",
                    IsActive = true
                },
                new CompanyLeader
                {
                    FullName = "Shahriar Kabir",
                    Designation = "Managing Director (MD)",
                    HierarchyRank = 2,
                    Speech = "Innovation and execution are the core pillars of CC. Whether providing custom enterprise CRM platforms or mentoring interns across software and creative graphics, we hold ourselves to the highest code of conduct and excellence.",
                    Email = "md@coreconcepts.com",
                    LinkedInUrl = "https://linkedin.com",
                    IsActive = true
                },
                new CompanyLeader
                {
                    FullName = "Nafis Fuad",
                    Designation = "Executive Director & Head of Engineering",
                    HierarchyRank = 3,
                    Speech = "Technology is evolving rapidly. Our team ensures that our engineering stack and our internship curricula reflect the latest cloud, AI, and secure architecture patterns.",
                    Email = "engineering@coreconcepts.com",
                    LinkedInUrl = "https://linkedin.com",
                    IsActive = true
                }
            );
        }

        // Seed Categories
        if (!await db.JobCategories.AnyAsync())
        {
            var catSoftware = new JobCategory { Name = "Software Engineering", Description = "Web, Backend, API & Full-Stack Development roles" };
            var catGraphics = new JobCategory { Name = "Graphics & Creative Design", Description = "UI/UX, Visual Branding, Motion & Creative Graphics" };
            var catQA = new JobCategory { Name = "Quality Assurance & Testing", Description = "Manual & Automated Software Quality Assurance" };
            var catMarketing = new JobCategory { Name = "Digital Strategy & IT Support", Description = "Growth, Marketing & Operations" };

            db.JobCategories.AddRange(catSoftware, catGraphics, catQA, catMarketing);
            await db.SaveChangesAsync();

            // Seed Job Openings
            db.JobOpenings.AddRange(
                new JobOpening
                {
                    Title = "Software Developer Intern (.NET / React)",
                    JobCategoryId = catSoftware.Id,
                    Type = "Internship",
                    Location = "Dhaka (Hybrid)",
                    Description = "Opportunity for passionate tech enthusiasts to work on real-world enterprise applications with .NET 10, MySQL, and modern front-end frameworks.",
                    Requirements = "Basic understanding of C#, OOP, SQL, and Git. Strong willingness to learn.",
                    SalaryOrStipend = "12,000 - 15,000 BDT / month",
                    Deadline = DateTime.Today.AddDays(30),
                    IsActive = true
                },
                new JobOpening
                {
                    Title = "Junior Graphics & UI/UX Designer",
                    JobCategoryId = catGraphics.Id,
                    Type = "Full-Time",
                    Location = "Dhaka (On-site)",
                    Description = "Create modern web/app UI prototypes in Figma, and design digital brand assets, marketing visuals, and presentations.",
                    Requirements = "Proficiency in Figma, Adobe Illustrator, and Photoshop. A portfolio link is required.",
                    SalaryOrStipend = "20,000 - 30,000 BDT / month",
                    Deadline = DateTime.Today.AddDays(25),
                    IsActive = true
                },
                new JobOpening
                {
                    Title = "Graphics Design Intern",
                    JobCategoryId = catGraphics.Id,
                    Type = "Internship",
                    Location = "Remote",
                    Description = "Hands-on internship designing creative banners, social assets, and iconography under senior designer guidance.",
                    Requirements = "Familiarity with Figma/Photoshop/Illustrator. Creativity and keen eye for aesthetics.",
                    SalaryOrStipend = "8,000 - 10,000 BDT / month",
                    Deadline = DateTime.Today.AddDays(40),
                    IsActive = true
                }
            );
        }

        // Seed Hero Banners
        if (!await db.HeroBanners.AnyAsync())
        {
            db.HeroBanners.AddRange(
                new HeroBanner
                {
                    Title = "Empowering Digital Evolution with CoreConcepts (CC)",
                    Subtitle = "We build cutting-edge software solutions, nurture emerging tech talent, and power high-impact digital experiences.",
                    ButtonText = "Explore Career & Internships",
                    ButtonUrl = "/Career",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new HeroBanner
                {
                    Title = "Custom Software & Enterprise Engineering",
                    Subtitle = "From cloud architectures to scalable web platforms, explore how CoreConcepts delivers next-gen software.",
                    ButtonText = "View Our Projects",
                    ButtonUrl = "/Projects",
                    DisplayOrder = 2,
                    IsActive = true
                }
            );
        }

        // Seed Feature Cards
        if (!await db.HomeFeatureCards.AnyAsync())
        {
            db.HomeFeatureCards.AddRange(
                new HomeFeatureCard
                {
                    Title = "Software Development",
                    Description = "Scalable enterprise web applications, high-performance APIs, and robust database architectures.",
                    IconClass = "bi-code-slash",
                    LinkUrl = "/Projects",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new HomeFeatureCard
                {
                    Title = "Graphics & UI/UX Design",
                    Description = "User-centric interface designs, engaging brand identities, and visually stunning digital assets.",
                    IconClass = "bi-palette",
                    LinkUrl = "/Projects",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new HomeFeatureCard
                {
                    Title = "Internship Programs",
                    Description = "Structured industrial training and real-client mentorship for ambitious students and fresh grads.",
                    IconClass = "bi-mortarboard",
                    LinkUrl = "/Career",
                    DisplayOrder = 3,
                    IsActive = true
                },
                new HomeFeatureCard
                {
                    Title = "Quality Assurance & Cloud",
                    Description = "Ensuring high stability, continuous deployment, and security for mission-critical software systems.",
                    IconClass = "bi-shield-check",
                    LinkUrl = "/Projects",
                    DisplayOrder = 4,
                    IsActive = true
                }
            );
        }

        // Seed Projects
        if (!await db.CompanyProjects.AnyAsync())
        {
            db.CompanyProjects.AddRange(
                new CompanyProject
                {
                    Title = "Auction Management System",
                    Category = "Software",
                    Description = "A complete multi-vendor online bidding and live auction platform with buyer dashboards, escrow payment tracking, and automated bid syncing.",
                    Technologies = "ASP.NET Core, MySQL, Bootstrap 5, SignalR",
                    Status = "Completed",
                    IsFeatured = true
                },
                new CompanyProject
                {
                    Title = "Tradelink CRM & ERP Suite",
                    Category = "Software",
                    Description = "Customer relationship management and enterprise order tracking system designed for commercial distribution networks.",
                    Technologies = "C#, Entity Framework Core, RESTful APIs",
                    Status = "Running",
                    IsFeatured = true
                },
                new CompanyProject
                {
                    Title = "Modern Brand Identity & UI Kit",
                    Category = "Graphics Design",
                    Description = "Comprehensive design system, UI components, typography, and illustration assets for modern SaaS products.",
                    Technologies = "Figma, Adobe Illustrator, Design System",
                    Status = "Completed",
                    IsFeatured = true
                }
            );
        }

        await db.SaveChangesAsync();
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
