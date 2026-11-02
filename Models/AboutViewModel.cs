using CoreConcepts.Models;

namespace CoreConcepts.Models;

public class AboutViewModel
{
    public SiteSettings Settings { get; set; } = new();
    public List<CompanyLeader> Leaders { get; set; } = [];
}
