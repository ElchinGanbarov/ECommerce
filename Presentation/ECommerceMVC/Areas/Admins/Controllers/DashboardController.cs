using ECommerceMVC.Areas.Admins.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Admins.Controllers
{
    [Area("Admins")]
    //[Authorize(AuthenticationSchemes = "Admin")]
    //[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            viewModel.PackageCount = 17;
            return View(viewModel);
        }
    }
}
