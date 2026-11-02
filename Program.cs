using CoreConcepts.Data;
using CoreConcepts.Models;
using CoreConcepts.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.User.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

// Configure Microsoft SQL Server (SQLEXPRESS)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<SiteSettingsService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var creator = dbContext.Database.GetService<IRelationalDatabaseCreator>();
    
    // Create database and any missing tables automatically!
    if (!creator.Exists())
    {
        creator.Create();
    }
    
    // Ensure all tables exist using raw DDL generation if tables are missing
    try
    {
        creator.CreateTables();
    }
    catch
    {
        // Tables already created previously, ensure newly added tables exist:
        await EnsureAdditionalTablesExistAsync(dbContext);
    }

    await IdentitySeed.InitializeAsync(scope.ServiceProvider, app.Configuration);
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task EnsureAdditionalTablesExistAsync(ApplicationDbContext db)
{
    // Ensure SiteSettings table exists
    var sqlSiteSettings = @"
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SiteSettings')
    BEGIN
        CREATE TABLE [SiteSettings] (
            [Id] int NOT NULL IDENTITY,
            [CompanyName] nvarchar(100) NOT NULL,
            [CompanyShortName] nvarchar(20) NOT NULL,
            [FooterAboutText] nvarchar(500) NOT NULL,
            [OfficeAddress] nvarchar(200) NOT NULL,
            [ContactPhone] nvarchar(50) NOT NULL,
            [ContactEmail] nvarchar(100) NOT NULL,
            [CareerEmail] nvarchar(100) NOT NULL,
            [OfficeHours] nvarchar(100) NULL,
            [FacebookUrl] nvarchar(200) NULL,
            [LinkedInUrl] nvarchar(200) NULL,
            [GitHubUrl] nvarchar(200) NULL,
            [TwitterUrl] nvarchar(200) NULL,
            [AboutMissionTitle] nvarchar(200) NOT NULL,
            [AboutMissionContent] nvarchar(max) NOT NULL,
            [Pillar1Title] nvarchar(200) NULL,
            [Pillar1Desc] nvarchar(200) NULL,
            [Pillar2Title] nvarchar(200) NULL,
            [Pillar2Desc] nvarchar(200) NULL,
            [Pillar3Title] nvarchar(200) NULL,
            [Pillar3Desc] nvarchar(200) NULL,
            [Pillar4Title] nvarchar(200) NULL,
            [Pillar4Desc] nvarchar(200) NULL,
            CONSTRAINT [PK_SiteSettings] PRIMARY KEY ([Id])
        );
    END";

    // Ensure CompanyLeaders table exists
    var sqlCompanyLeaders = @"
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CompanyLeaders')
    BEGIN
        CREATE TABLE [CompanyLeaders] (
            [Id] int NOT NULL IDENTITY,
            [FullName] nvarchar(100) NOT NULL,
            [Designation] nvarchar(100) NOT NULL,
            [HierarchyRank] int NOT NULL DEFAULT 1,
            [PhotoPath] nvarchar(300) NULL,
            [Speech] nvarchar(1000) NOT NULL,
            [LinkedInUrl] nvarchar(200) NULL,
            [Email] nvarchar(100) NULL,
            [IsActive] bit NOT NULL DEFAULT 1,
            [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
            CONSTRAINT [PK_CompanyLeaders] PRIMARY KEY ([Id])
        );
    END";

    await db.Database.ExecuteSqlRawAsync(sqlSiteSettings);
    await db.Database.ExecuteSqlRawAsync(sqlCompanyLeaders);
}
