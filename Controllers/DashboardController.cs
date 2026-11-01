using CoreConcepts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreConcepts.Controllers;

[Authorize]
public class DashboardController : Controller
{
    [Authorize(Roles = IdentitySeed.AdminRole)]
    public IActionResult Admin() => View();

    [Authorize(Roles = IdentitySeed.UserRole)]
    [ActionName("User")]
    public IActionResult UserDashboard() => View();
}
