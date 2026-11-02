using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class CompanyProject
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Category { get; set; } = "Software"; // Software, Web App, Mobile App, UI/UX, Graphics

    [StringLength(300)]
    public string? ImagePath { get; set; }

    [StringLength(200)]
    public string? ProjectUrl { get; set; }

    [StringLength(100)]
    public string Status { get; set; } = "Running"; // Running, Completed, Upcoming

    [StringLength(300)]
    public string? Technologies { get; set; } // e.g. ".NET 10, React, MySQL"

    public bool IsFeatured { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
