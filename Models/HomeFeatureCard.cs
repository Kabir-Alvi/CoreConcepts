using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class HomeFeatureCard
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string Description { get; set; } = string.Empty;

    [StringLength(50)]
    public string? IconClass { get; set; } // Bootstrap icon or fontawesome e.g. "bi-laptop", "bi-palette"

    [StringLength(300)]
    public string? ImagePath { get; set; }

    [StringLength(200)]
    public string? LinkUrl { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}
