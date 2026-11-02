using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreConcepts.Models;

public class JobOpening
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Job Category")]
    public int JobCategoryId { get; set; }

    [ForeignKey(nameof(JobCategoryId))]
    public JobCategory? JobCategory { get; set; }

    [Required, StringLength(50)]
    public string Type { get; set; } = "Internship"; // Internship, Full-Time, Part-Time, Remote

    [Required, StringLength(100)]
    public string Location { get; set; } = "Dhaka, Bangladesh (Hybrid)";

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }

    [StringLength(100)]
    public string? SalaryOrStipend { get; set; } // e.g. "Negotiable" or "15,000 - 20,000 BDT"

    [DataType(DataType.Date)]
    public DateTime? Deadline { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<JobApplication> Applications { get; set; } = [];
}
