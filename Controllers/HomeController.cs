using CoreConcepts.Data;
using CoreConcepts.Models;
using CoreConcepts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

public class HomeController(
    ApplicationDbContext context,
    SiteSettingsService settingsService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var banners = await context.HeroBanners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        var cards = await context.HomeFeatureCards
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var projects = await context.CompanyProjects
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.CreatedAt)
            .Take(6)
            .ToListAsync();

        var jobs = await context.JobOpenings
            .Include(j => j.JobCategory)
            .Where(j => j.IsActive)
            .OrderByDescending(j => j.CreatedAt)
            .Take(4)
            .ToListAsync();

        var vm = new HomeIndexViewModel
        {
            Banners = banners,
            FeatureCards = cards,
            FeaturedProjects = projects,
            LatestJobs = jobs
        };

        return View(vm);
    }

    public async Task<IActionResult> About()
    {
        var settings = await settingsService.GetSettingsAsync();

        // Sort automatically by Seniority / Hierarchy Rank: Lower number = Higher rank (CEO #1, MD #2, etc.)
        var leaders = await context.CompanyLeaders
            .Where(l => l.IsActive)
            .OrderBy(l => l.HierarchyRank)
            .ThenBy(l => l.CreatedAt)
            .ToListAsync();

        var vm = new AboutViewModel
        {
            Settings = settings,
            Leaders = leaders
        };

        return View(vm);
    }

    [Authorize(Roles = IdentitySeed.AdminRole)]
    [HttpGet]
    public async Task<IActionResult> EditAbout()
    {
        var settings = await settingsService.GetSettingsAsync();
        return View(settings);
    }

    [Authorize(Roles = IdentitySeed.AdminRole)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAbout(SiteSettings model)
    {
        var settings = await settingsService.GetSettingsAsync();
        settings.AboutMissionTitle = model.AboutMissionTitle;
        settings.AboutMissionContent = model.AboutMissionContent;
        settings.Pillar1Title = model.Pillar1Title;
        settings.Pillar1Desc = model.Pillar1Desc;
        settings.Pillar2Title = model.Pillar2Title;
        settings.Pillar2Desc = model.Pillar2Desc;
        settings.Pillar3Title = model.Pillar3Title;
        settings.Pillar3Desc = model.Pillar3Desc;
        settings.Pillar4Title = model.Pillar4Title;
        settings.Pillar4Desc = model.Pillar4Desc;

        await context.SaveChangesAsync();
        TempData["Success"] = "About Us page content updated successfully!";
        return RedirectToAction(nameof(About));
    }

    public async Task<IActionResult> Contact()
    {
        var settings = await settingsService.GetSettingsAsync();
        return View(settings);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
