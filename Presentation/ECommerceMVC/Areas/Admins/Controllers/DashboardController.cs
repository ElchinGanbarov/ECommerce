using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerce.Domain.Entities.Identity;
using ECommerceMVC.Areas.Admins.Models;
using ECommerceMVC.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ECommerceMVC.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = "ADMIN")]
    [Route("[area]/[controller]/[action]")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(ILogger<DashboardController> logger) : base(logger)
        {
                
        }
        [HttpGet]
        public IActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            viewModel.PackageCount = 17;
            return View(viewModel);
        }
    }
}
