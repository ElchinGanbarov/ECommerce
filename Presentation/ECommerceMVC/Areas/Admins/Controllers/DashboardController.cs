using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerce.Domain.Entities.Identity;
using ECommerceMVC.Areas.Admins.Models;
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
    [Authorize(Roles = "Admin")]
    [Route("[area]/[controller]/[action]")]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            viewModel.PackageCount = 17;
            return View(viewModel);
        }
    }
}
