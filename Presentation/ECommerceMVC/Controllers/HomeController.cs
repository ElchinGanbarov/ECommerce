using ECommerce.Application.Const;
using ECommerceMVC.Controllers.Base;
using ECommerceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace ECommerceMVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        [Route("/{lang:lang}", Order = -1)]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        //[Authorize]
        public IActionResult Privacy()
        {
            if (User.Identity.IsAuthenticated)
            {

            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}