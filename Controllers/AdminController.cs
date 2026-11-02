using CoreConcepts.Data;
using CoreConcepts.Models;
using CoreConcepts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

[Authorize(Roles = IdentitySeed.AdminRole)]
public class AdminController(
    ApplicationDbContext context,
    SiteSettingsService settingsService,
    IWebHostEnvironment environment) : Controller
{
    // ==========================================
    // SITE & FOOTER SETTINGS CMS
    // ==========================================
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var settings = await settingsService.GetSettingsAsync();
        return View(settings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(SiteSettings model)
    {
        if (ModelState.IsValid)
        {
            var settings = await settingsService.GetSettingsAsync();
            settings.CompanyName = model.CompanyName;
            settings.CompanyShortName = model.CompanyShortName;
            settings.FooterAboutText = model.FooterAboutText;
            settings.OfficeAddress = model.OfficeAddress;
            settings.ContactPhone = model.ContactPhone;
            settings.ContactEmail = model.ContactEmail;
            settings.CareerEmail = model.CareerEmail;
            settings.OfficeHours = model.OfficeHours;
            settings.FacebookUrl = model.FacebookUrl;
            settings.LinkedInUrl = model.LinkedInUrl;
            settings.GitHubUrl = model.GitHubUrl;
            settings.TwitterUrl = model.TwitterUrl;

            await context.SaveChangesAsync();
            TempData["Success"] = "Footer & Site Settings updated successfully!";
            return RedirectToAction(nameof(Settings));
        }
        return View(model);
    }

    // ==========================================
    // COMPANY LEADERS / DIRECTORS / CEO MANAGEMENT
    // ==========================================
    public async Task<IActionResult> Leaders()
    {
        var leaders = await context.CompanyLeaders
            .OrderBy(l => l.HierarchyRank)
            .ThenBy(l => l.CreatedAt)
            .ToListAsync();
        return View(leaders);
    }

    [HttpGet]
    public IActionResult CreateLeader() => View(new CompanyLeader());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLeader(CompanyLeader leader, IFormFile? photoFile)
    {
        if (ModelState.IsValid)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                leader.PhotoPath = await SaveUploadedFileAsync(photoFile, "leaders");
            }

            context.CompanyLeaders.Add(leader);
            await context.SaveChangesAsync();
            TempData["Success"] = $"Leader '{leader.FullName}' ({leader.Designation}) added successfully!";
            return RedirectToAction("About", "Home");
        }
        return View(leader);
    }

    [HttpGet]
    public async Task<IActionResult> EditLeader(int id)
    {
        var leader = await context.CompanyLeaders.FindAsync(id);
        if (leader == null) return NotFound();
        return View(leader);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLeader(CompanyLeader leader, IFormFile? photoFile)
    {
        if (ModelState.IsValid)
        {
            var existing = await context.CompanyLeaders.FindAsync(leader.Id);
            if (existing == null) return NotFound();

            if (photoFile != null && photoFile.Length > 0)
            {
                existing.PhotoPath = await SaveUploadedFileAsync(photoFile, "leaders");
            }

            existing.FullName = leader.FullName;
            existing.Designation = leader.Designation;
            existing.HierarchyRank = leader.HierarchyRank;
            existing.Speech = leader.Speech;
            existing.LinkedInUrl = leader.LinkedInUrl;
            existing.Email = leader.Email;
            existing.IsActive = leader.IsActive;

            await context.SaveChangesAsync();
            TempData["Success"] = $"Updated details for {leader.FullName}!";
            return RedirectToAction("About", "Home");
        }
        return View(leader);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLeader(int id)
    {
        var leader = await context.CompanyLeaders.FindAsync(id);
        if (leader != null)
        {
            context.CompanyLeaders.Remove(leader);
            await context.SaveChangesAsync();
            TempData["Success"] = "Leader removed!";
        }
        return RedirectToAction("About", "Home");
    }

    // ==========================================
    // HERO BANNERS MANAGEMENT
    // ==========================================
    public async Task<IActionResult> Banners()
    {
        var banners = await context.HeroBanners.OrderBy(b => b.DisplayOrder).ToListAsync();
        return View(banners);
    }

    [HttpGet]
    public IActionResult CreateBanner() => View(new HeroBanner());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBanner(HeroBanner banner, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                banner.ImagePath = await SaveUploadedFileAsync(imageFile, "banners");
            }

            context.HeroBanners.Add(banner);
            await context.SaveChangesAsync();
            TempData["Success"] = "Banner created successfully!";
            return RedirectToAction(nameof(Banners));
        }
        return View(banner);
    }

    [HttpGet]
    public async Task<IActionResult> EditBanner(int id)
    {
        var banner = await context.HeroBanners.FindAsync(id);
        if (banner == null) return NotFound();
        return View(banner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBanner(HeroBanner banner, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            var existing = await context.HeroBanners.FindAsync(banner.Id);
            if (existing == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                existing.ImagePath = await SaveUploadedFileAsync(imageFile, "banners");
            }

            existing.Title = banner.Title;
            existing.Subtitle = banner.Subtitle;
            existing.ButtonText = banner.ButtonText;
            existing.ButtonUrl = banner.ButtonUrl;
            existing.DisplayOrder = banner.DisplayOrder;
            existing.IsActive = banner.IsActive;

            await context.SaveChangesAsync();
            TempData["Success"] = "Banner updated successfully!";
            return RedirectToAction(nameof(Banners));
        }
        return View(banner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        var banner = await context.HeroBanners.FindAsync(id);
        if (banner != null)
        {
            context.HeroBanners.Remove(banner);
            await context.SaveChangesAsync();
            TempData["Success"] = "Banner deleted successfully!";
        }
        return RedirectToAction(nameof(Banners));
    }

    // ==========================================
    // HOME FEATURE CARDS MANAGEMENT
    // ==========================================
    public async Task<IActionResult> FeatureCards()
    {
        var cards = await context.HomeFeatureCards.OrderBy(c => c.DisplayOrder).ToListAsync();
        return View(cards);
    }

    [HttpGet]
    public IActionResult CreateFeatureCard() => View(new HomeFeatureCard());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFeatureCard(HomeFeatureCard card, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                card.ImagePath = await SaveUploadedFileAsync(imageFile, "cards");
            }
            context.HomeFeatureCards.Add(card);
            await context.SaveChangesAsync();
            TempData["Success"] = "Feature card created successfully!";
            return RedirectToAction(nameof(FeatureCards));
        }
        return View(card);
    }

    [HttpGet]
    public async Task<IActionResult> EditFeatureCard(int id)
    {
        var card = await context.HomeFeatureCards.FindAsync(id);
        if (card == null) return NotFound();
        return View(card);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFeatureCard(HomeFeatureCard card, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            var existing = await context.HomeFeatureCards.FindAsync(card.Id);
            if (existing == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                existing.ImagePath = await SaveUploadedFileAsync(imageFile, "cards");
            }

            existing.Title = card.Title;
            existing.Description = card.Description;
            existing.IconClass = card.IconClass;
            existing.LinkUrl = card.LinkUrl;
            existing.DisplayOrder = card.DisplayOrder;
            existing.IsActive = card.IsActive;

            await context.SaveChangesAsync();
            TempData["Success"] = "Feature card updated successfully!";
            return RedirectToAction(nameof(FeatureCards));
        }
        return View(card);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFeatureCard(int id)
    {
        var card = await context.HomeFeatureCards.FindAsync(id);
        if (card != null)
        {
            context.HomeFeatureCards.Remove(card);
            await context.SaveChangesAsync();
            TempData["Success"] = "Feature card deleted!";
        }
        return RedirectToAction(nameof(FeatureCards));
    }

    // ==========================================
    // PROJECTS MANAGEMENT
    // ==========================================
    public async Task<IActionResult> Projects()
    {
        var projects = await context.CompanyProjects.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return View(projects);
    }

    [HttpGet]
    public IActionResult CreateProject() => View(new CompanyProject());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProject(CompanyProject project, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                project.ImagePath = await SaveUploadedFileAsync(imageFile, "projects");
            }

            context.CompanyProjects.Add(project);
            await context.SaveChangesAsync();
            TempData["Success"] = "Project created successfully!";
            return RedirectToAction(nameof(Projects));
        }
        return View(project);
    }

    [HttpGet]
    public async Task<IActionResult> EditProject(int id)
    {
        var project = await context.CompanyProjects.FindAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProject(CompanyProject project, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            var existing = await context.CompanyProjects.FindAsync(project.Id);
            if (existing == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                existing.ImagePath = await SaveUploadedFileAsync(imageFile, "projects");
            }

            existing.Title = project.Title;
            existing.Category = project.Category;
            existing.Description = project.Description;
            existing.Technologies = project.Technologies;
            existing.ProjectUrl = project.ProjectUrl;
            existing.Status = project.Status;
            existing.IsFeatured = project.IsFeatured;

            await context.SaveChangesAsync();
            TempData["Success"] = "Project updated successfully!";
            return RedirectToAction(nameof(Projects));
        }
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await context.CompanyProjects.FindAsync(id);
        if (project != null)
        {
            context.CompanyProjects.Remove(project);
            await context.SaveChangesAsync();
            TempData["Success"] = "Project deleted!";
        }
        return RedirectToAction(nameof(Projects));
    }

    // ==========================================
    // JOB OPENINGS & INTERNSHIPS MANAGEMENT
    // ==========================================
    public async Task<IActionResult> Jobs()
    {
        var jobs = await context.JobOpenings
            .Include(j => j.JobCategory)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
        return View(jobs);
    }

    [HttpGet]
    public async Task<IActionResult> CreateJob()
    {
        ViewBag.Categories = new SelectList(await context.JobCategories.ToListAsync(), "Id", "Name");
        return View(new JobOpening());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJob(JobOpening job)
    {
        if (ModelState.IsValid)
        {
            context.JobOpenings.Add(job);
            await context.SaveChangesAsync();
            TempData["Success"] = "Job / Internship posted successfully!";
            return RedirectToAction(nameof(Jobs));
        }
        ViewBag.Categories = new SelectList(await context.JobCategories.ToListAsync(), "Id", "Name", job.JobCategoryId);
        return View(job);
    }

    [HttpGet]
    public async Task<IActionResult> EditJob(int id)
    {
        var job = await context.JobOpenings.FindAsync(id);
        if (job == null) return NotFound();

        ViewBag.Categories = new SelectList(await context.JobCategories.ToListAsync(), "Id", "Name", job.JobCategoryId);
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditJob(JobOpening job)
    {
        if (ModelState.IsValid)
        {
            context.Update(job);
            await context.SaveChangesAsync();
            TempData["Success"] = "Job opening updated successfully!";
            return RedirectToAction(nameof(Jobs));
        }
        ViewBag.Categories = new SelectList(await context.JobCategories.ToListAsync(), "Id", "Name", job.JobCategoryId);
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var job = await context.JobOpenings.FindAsync(id);
        if (job != null)
        {
            context.JobOpenings.Remove(job);
            await context.SaveChangesAsync();
            TempData["Success"] = "Job post deleted!";
        }
        return RedirectToAction(nameof(Jobs));
    }

    // ==========================================
    // JOB CATEGORIES (Software, Graphics, QA, etc.)
    // ==========================================
    public async Task<IActionResult> Categories()
    {
        var categories = await context.JobCategories
            .Include(c => c.JobOpenings)
            .ToListAsync();
        return View(categories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(JobCategory category)
    {
        if (!string.IsNullOrWhiteSpace(category.Name))
        {
            context.JobCategories.Add(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "Category added successfully!";
        }
        return RedirectToAction(nameof(Categories));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await context.JobCategories.Include(c => c.JobOpenings).FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            if (category.JobOpenings.Any())
            {
                TempData["Error"] = "Cannot delete category with associated job postings.";
            }
            else
            {
                context.JobCategories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "Category deleted!";
            }
        }
        return RedirectToAction(nameof(Categories));
    }

    // ==========================================
    // RECEIVED APPLICATIONS & RESUMES
    // ==========================================
    public async Task<IActionResult> Applications(int? jobId, string? status)
    {
        var query = context.JobApplications
            .Include(a => a.JobOpening)
                .ThenInclude(j => j!.JobCategory)
            .AsQueryable();

        if (jobId.HasValue && jobId.Value > 0)
        {
            query = query.Where(a => a.JobOpeningId == jobId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(a => a.Status == status);
        }

        var applications = await query.OrderByDescending(a => a.AppliedAt).ToListAsync();

        ViewBag.Jobs = await context.JobOpenings.ToListAsync();
        ViewBag.SelectedJobId = jobId ?? 0;
        ViewBag.SelectedStatus = status ?? "All";

        return View(applications);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateApplicationStatus(int id, string status)
    {
        var application = await context.JobApplications.FindAsync(id);
        if (application != null)
        {
            application.Status = status;
            await context.SaveChangesAsync();
            TempData["Success"] = $"Status updated to '{status}' for applicant {application.FullName}!";
        }
        return RedirectToAction(nameof(Applications));
    }

    // Helper method for file uploads
    private async Task<string> SaveUploadedFileAsync(IFormFile file, string subFolder)
    {
        var uploadsPath = Path.Combine(environment.WebRootPath, "uploads", subFolder);
        Directory.CreateDirectory(uploadsPath);

        var uniqueFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{subFolder}/{uniqueFileName}";
    }
}
