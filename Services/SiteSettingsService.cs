using CoreConcepts.Data;
using CoreConcepts.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreConcepts.Services;

public class SiteSettingsService(ApplicationDbContext context)
{
    public async Task<SiteSettings> GetSettingsAsync()
    {
        var settings = await context.SiteSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new SiteSettings
            {
                Id = 0 // Ensures EF Core treats it as a new entity for IDENTITY insert
            };
            context.SiteSettings.Add(settings);
            await context.SaveChangesAsync();
        }
        return settings;
    }
}
