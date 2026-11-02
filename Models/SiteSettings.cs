using System.ComponentModel.DataAnnotations;

namespace CoreConcepts.Models;

public class SiteSettings
{
    public int Id { get; set; } = 1;

    // Company Branding & Summary (Footer & Top Bar)
    [Required, StringLength(100)]
    public string CompanyName { get; set; } = "CoreConcepts";

    [Required, StringLength(20)]
    public string CompanyShortName { get; set; } = "CC";

    [Required, StringLength(500)]
    public string FooterAboutText { get; set; } = "CoreConcepts (CC) is a forward-thinking software development and innovation agency. We deliver enterprise solutions, modern UI/UX design, and empower future talent through real-industry internship programs.";

    // Headquarters Contact Info
    [Required, StringLength(200)]
    public string OfficeAddress { get; set; } = "Banani C/A, Road 11, Dhaka 1213, Bangladesh";

    [Required, StringLength(50)]
    public string ContactPhone { get; set; } = "+880 1700-000000";

    [Required, EmailAddress, StringLength(100)]
    public string ContactEmail { get; set; } = "contact@coreconcepts.com";

    [Required, EmailAddress, StringLength(100)]
    public string CareerEmail { get; set; } = "career@coreconcepts.com";

    [StringLength(100)]
    public string OfficeHours { get; set; } = "Sun - Thu: 9:00 AM - 6:00 PM";

    // Social Links
    [StringLength(200)]
    public string? FacebookUrl { get; set; } = "https://facebook.com";

    [StringLength(200)]
    public string? LinkedInUrl { get; set; } = "https://linkedin.com";

    [StringLength(200)]
    public string? GitHubUrl { get; set; } = "https://github.com";

    [StringLength(200)]
    public string? TwitterUrl { get; set; } = "https://x.com";

    // About Us Content (Dynamic)
    [Required, StringLength(200)]
    public string AboutMissionTitle { get; set; } = "Empowering Innovation Through Code & Creativity";

    [Required]
    public string AboutMissionContent { get; set; } = "CoreConcepts (CC) is an engineering and design collective. We engineer robust, enterprise-grade software platforms, sleek digital identities, and high-impact digital experiences.\n\nBeyond production software, CoreConcepts is deeply committed to nurturing the next generation of engineers and designers through our structured internship programs in software development, graphics design, and quality assurance.";

    [StringLength(200)]
    public string Pillar1Title { get; set; } = "Software";
    [StringLength(200)]
    public string Pillar1Desc { get; set; } = "High-performance scalable web and cloud systems";

    [StringLength(200)]
    public string Pillar2Title { get; set; } = "Creative";
    [StringLength(200)]
    public string Pillar2Desc { get; set; } = "User interfaces, design systems, and visual branding";

    [StringLength(200)]
    public string Pillar3Title { get; set; } = "Internships";
    [StringLength(200)]
    public string Pillar3Desc { get; set; } = "Real client exposure and senior dev mentorship";

    [StringLength(200)]
    public string Pillar4Title { get; set; } = "Quality";
    [StringLength(200)]
    public string Pillar4Desc { get; set; } = "Automated testing, stability, and high performance";
}
