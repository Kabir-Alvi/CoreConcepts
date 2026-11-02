using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class CompanyLeader
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Designation")]
    public string Designation { get; set; } = string.Empty; // e.g. "Chief Executive Officer (CEO)", "Managing Director (MD)"

    [Display(Name = "Seniority / Hierarchy Rank")]
    public int HierarchyRank { get; set; } = 1; 
    // Higher rank appears first. For example: 
    // 1 = CEO, 2 = Managing Director (MD), 3 = Executive Director, 4 = Director, 5 = Head of Dept, etc.

    [StringLength(300)]
    [Display(Name = "Leader Photo")]
    public string? PhotoPath { get; set; }

    [Required, StringLength(1000)]
    [Display(Name = "Speech / Executive Quote")]
    public string Speech { get; set; } = string.Empty;

    [StringLength(200)]
    public string? LinkedInUrl { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
