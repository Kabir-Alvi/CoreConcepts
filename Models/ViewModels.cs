using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class CareerApplyViewModel
{
    public int JobOpeningId { get; set; }

    public string? JobTitle { get; set; }
    public string? CategoryName { get; set; }
    public string? JobType { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Your Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "LinkedIn, GitHub or Portfolio Link / Short Note")]
    public string? CoverNote { get; set; }

    [Required]
    [Display(Name = "Upload CV / Resume (PDF, DOC, DOCX - Max 5MB)")]
    public IFormFile? CvFile { get; set; }
}

public class HomeIndexViewModel
{
    public List<HeroBanner> Banners { get; set; } = [];
    public List<HomeFeatureCard> FeatureCards { get; set; } = [];
    public List<CompanyProject> FeaturedProjects { get; set; } = [];
    public List<JobOpening> LatestJobs { get; set; } = [];
}
