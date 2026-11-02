using CoreConcepts.Data;
using CoreConcepts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Controllers;

public class CareerController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IWebHostEnvironment environment) : Controller
{
    // GET: /Career
    public async Task<IActionResult> Index(int? categoryId, string? type, string? search)
    {
        var query = context.JobOpenings
            .Include(j => j.JobCategory)
            .Where(j => j.IsActive)
            .AsQueryable();

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(j => j.JobCategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(type) && type != "All")
        {
            query = query.Where(j => j.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(j => j.Title.Contains(search) || j.Description.Contains(search));
        }

        var jobs = await query.OrderByDescending(j => j.CreatedAt).ToListAsync();

        ViewBag.Categories = await context.JobCategories.ToListAsync();
        ViewBag.SelectedCategoryId = categoryId ?? 0;
        ViewBag.SelectedType = type ?? "All";
        ViewBag.Search = search;

        return View(jobs);
    }

    // GET: /Career/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var job = await context.JobOpenings
            .Include(j => j.JobCategory)
            .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

        if (job == null) return NotFound();

        return View(job);
    }

    // GET: /Career/Apply/5 (Requires Authentication)
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Apply(int id)
    {
        var job = await context.JobOpenings
            .Include(j => j.JobCategory)
            .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

        if (job == null) return NotFound();

        var currentUser = await userManager.GetUserAsync(User);

        // Check if user already applied
        var alreadyApplied = await context.JobApplications
            .AnyAsync(a => a.JobOpeningId == id && a.ApplicantUserId == currentUser!.Id);

        if (alreadyApplied)
        {
            TempData["InfoMessage"] = "You have already submitted an application for this position! You can view its status in your dashboard.";
            return RedirectToAction("User", "Dashboard");
        }

        var vm = new CareerApplyViewModel
        {
            JobOpeningId = job.Id,
            JobTitle = job.Title,
            CategoryName = job.JobCategory?.Name,
            JobType = job.Type,
            FullName = currentUser?.FullName ?? "",
            Email = currentUser?.Email ?? "",
            Phone = currentUser?.ContactInfo ?? ""
        };

        return View(vm);
    }

    // POST: /Career/Apply
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(CareerApplyViewModel model)
    {
        var job = await context.JobOpenings
            .Include(j => j.JobCategory)
            .FirstOrDefaultAsync(j => j.Id == model.JobOpeningId && j.IsActive);

        if (job == null) return NotFound();

        model.JobTitle = job.Title;
        model.CategoryName = job.JobCategory?.Name;
        model.JobType = job.Type;

        if (model.CvFile == null || model.CvFile.Length == 0)
        {
            ModelState.AddModelError("CvFile", "Please upload your CV / Resume file.");
        }
        else
        {
            var ext = Path.GetExtension(model.CvFile.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            if (!allowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("CvFile", "Allowed file formats are .pdf, .doc, and .docx only.");
            }

            if (model.CvFile.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                ModelState.AddModelError("CvFile", "CV file size cannot exceed 5MB.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        // Save CV File
        var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "resumes");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(model.CvFile!.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await model.CvFile.CopyToAsync(stream);
        }

        var application = new JobApplication
        {
            JobOpeningId = model.JobOpeningId,
            ApplicantUserId = currentUser.Id,
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            CoverNote = model.CoverNote,
            CvFilePath = $"/uploads/resumes/{uniqueFileName}",
            Status = "Pending",
            AppliedAt = DateTime.UtcNow
        };

        context.JobApplications.Add(application);
        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Your application for '{job.Title}' has been submitted successfully! The HR team will review your profile.";
        return RedirectToAction("User", "Dashboard");
    }
}
