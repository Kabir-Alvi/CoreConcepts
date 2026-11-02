using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class HeroBanner
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Subtitle { get; set; }

    [StringLength(300)]
    public string? ImagePath { get; set; }

    [StringLength(100)]
    public string? ButtonText { get; set; }

    [StringLength(200)]
    public string? ButtonUrl { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
