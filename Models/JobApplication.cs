using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreConcepts.Models;

public class JobApplication
{
    public int Id { get; set; }

    [Required]
    public int JobOpeningId { get; set; }

    [ForeignKey(nameof(JobOpeningId))]
    public JobOpening? JobOpening { get; set; }

    [Required]
    public string ApplicantUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicantUserId))]
    public ApplicationUser? ApplicantUser { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Cover Note / Portfolio / LinkedIn Link")]
    public string? CoverNote { get; set; }

    [Required, StringLength(300)]
    [Display(Name = "CV / Resume File")]
    public string CvFilePath { get; set; } = string.Empty;

    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Reviewed, Shortlisted, Rejected

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
