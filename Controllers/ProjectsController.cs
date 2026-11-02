using CoreConcepts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

public class ProjectsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? category, string? status, string? search)
    {
        var query = context.CompanyProjects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category) && category != "All")
        {
            query = query.Where(p => p.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(p => p.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Title.Contains(search) || p.Description.Contains(search) || (p.Technologies != null && p.Technologies.Contains(search)));
        }

        var projects = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

        ViewBag.SelectedCategory = category ?? "All";
        ViewBag.SelectedStatus = status ?? "All";
        ViewBag.Search = search;
        
        // Dynamic categories from JobCategories + existing project categories
        var deptNames = await context.JobCategories.Select(c => c.Name).ToListAsync();
        var projCategories = await context.CompanyProjects.Select(p => p.Category).Distinct().ToListAsync();
        ViewBag.Categories = deptNames.Union(projCategories).Distinct().ToList();

        return View(projects);
    }

    public async Task<IActionResult> Details(int id)
    {
        var project = await context.CompanyProjects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return NotFound();

        return View(project);
    }
}
