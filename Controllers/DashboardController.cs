using CoreConcepts.Data;
using CoreConcepts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

[Authorize]
public class DashboardController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    // ==========================================
    // USER DASHBOARD
    // ==========================================
    [Authorize(Roles = IdentitySeed.UserRole)]
    [ActionName("User")]
    public async Task<IActionResult> UserDashboard()
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        var applications = await context.JobApplications
            .Include(a => a.JobOpening)
                .ThenInclude(j => j!.JobCategory)
            .Where(a => a.ApplicantUserId == currentUser.Id)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

        ViewBag.User = currentUser;
        return View(applications);
    }

    // ==========================================
    // ADMIN DASHBOARD OVERVIEW
    // ==========================================
    [Authorize(Roles = IdentitySeed.AdminRole)]
    public async Task<IActionResult> Admin()
    {
        ViewBag.TotalBanners = await context.HeroBanners.CountAsync();
        ViewBag.TotalProjects = await context.CompanyProjects.CountAsync();
        ViewBag.TotalJobs = await context.JobOpenings.CountAsync();
        ViewBag.TotalApplications = await context.JobApplications.CountAsync();

        var recentApplications = await context.JobApplications
            .Include(a => a.JobOpening)
            .OrderByDescending(a => a.AppliedAt)
            .Take(5)
            .ToListAsync();

        return View(recentApplications);
    }
}
