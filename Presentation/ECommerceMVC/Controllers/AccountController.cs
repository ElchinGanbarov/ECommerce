﻿using ECommerce.Application;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.CustomAttributes;
using ECommerce.Application.Features.Commands.AppUser.AssignRoleToUser;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using ECommerce.Application.Features.Commands.CreateUser;
using ECommerce.Application.Features.Commands.UpdatePassword;
using ECommerce.Application.Features.Queries.AppUser.GetAllUsers;
using ECommerce.Application.Features.Queries.AppUser.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ECommerce.Application.Features.Commands.AppUser.GoogleLogin;

namespace ECommerceMVC.Controllers
{
    public class AccountController : Controller
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;

        private readonly SignInManager<ECommerce.Domain.Entities.Identity.AppUser> _signInManager;

        public AccountController(IMediator mediator,
                               IMailService mailService,
                               SignInManager<ECommerce.Domain.Entities.Identity.AppUser> signInManager)
        {
            _mediator = mediator;
            _mailService = mailService;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            await _mediator.Send(loginUserCommandRequest);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ExternalLogin(string returnUrl = "/")
        {
            // Generate a random nonce and store it in the session
            var nonce = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("GoogleLoginNonce", nonce);

            // Specify the Google login provider and the return URL
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", new { returnUrl }),
                Items = { { "nonce", nonce } },
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = "/home/index")
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded || string.IsNullOrEmpty(result.Principal?.Identity?.Name))
            {
                // Handle the login failure (e.g., return to login page or display an error)
                return RedirectToAction("Login");
            }

            // Verify the nonce to protect against replay attacks
            if (!result.Properties.Items.TryGetValue("nonce", out var nonce) ||
                !HttpContext.Session.TryGetValue("GoogleLoginNonce", out var storedNonce) ||
                nonce != Encoding.UTF8.GetString(storedNonce))
            {
                // Nonce doesn't match, handle the error (e.g., return to login page)
                return RedirectToAction("Login");
            }

            // Clear the nonce from the session
            HttpContext.Session.Remove("GoogleLoginNonce");

            // Successful login; you can access the user's information here
            var userEmail = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;



            GoogleLoginCommandRequest googleLoginCommandRequest = new GoogleLoginCommandRequest
            {
                FirstName = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                LastName = result.Principal.FindFirst(ClaimTypes.Surname)?.Value,
                Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,
                Name = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                Provider = "Google",
                IdToken = result.Principal.FindFirst(ClaimTypes.NameIdentifier).Value

        };
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);

            // Add your logic for user registration or any other operations here

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Principal);

            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = "Users")]
        public async Task<IActionResult> Index([FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            GetAllUsersQueryResponse response = await _mediator.Send(getAllUsersQueryRequest);
            return View(response);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To Users", Menu = "Users")]
        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest getRolesToUserQueryRequest)
        {
            GetRolesToUserQueryResponse response = await _mediator.Send(getRolesToUserQueryRequest);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Assign Role To User", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }
    }
}
