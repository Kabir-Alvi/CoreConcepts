using CoreConcepts.Data;
using CoreConcepts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
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

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
