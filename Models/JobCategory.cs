using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class JobCategory
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty; // e.g. "Software Engineering", "Graphics Design", "UI/UX", "Quality Assurance"

    [StringLength(300)]
    public string? Description { get; set; }

    public ICollection<JobOpening> JobOpenings { get; set; } = [];
}
