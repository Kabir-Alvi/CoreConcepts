using CoreConcepts.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<HeroBanner> HeroBanners { get; set; }
    public DbSet<HomeFeatureCard> HomeFeatureCards { get; set; }
    public DbSet<CompanyProject> CompanyProjects { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<JobOpening> JobOpenings { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<SiteSettings> SiteSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<JobApplication>()
            .HasOne(ja => ja.JobOpening)
            .WithMany(jo => jo.Applications)
            .HasForeignKey(ja => ja.JobOpeningId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<JobApplication>()
            .HasOne(ja => ja.ApplicantUser)
            .WithMany()
            .HasForeignKey(ja => ja.ApplicantUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
