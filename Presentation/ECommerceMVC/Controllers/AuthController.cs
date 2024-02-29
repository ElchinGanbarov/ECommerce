using ECommerce.Application;
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
using ECommerce.Application.Features.Commands.AppUser.PasswordReset;
using ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerce.Application.Features.Queries.AppUser.GetUserById;
using Microsoft.AspNetCore.Http;
using ECommerce.Application.Features.Commands.AppUser.LogoutUser;

namespace ECommerceMVC.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;
        private readonly ILogger<HomeController> _logger;

        private readonly SignInManager<ECommerce.Domain.Entities.Identity.AppUser> _signInManager;

        public AuthController(IMediator mediator,
                               IMailService mailService,
                               SignInManager<ECommerce.Domain.Entities.Identity.AppUser> signInManager,
                               ILogger<HomeController> logger)
        {
            _mediator = mediator;
            _mailService = mailService;
            _signInManager = signInManager;
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            if (!ModelState.IsValid) return View();
            var res = await _mediator.Send(loginUserCommandRequest);
            if (!res.Success)
            {
                ModelState.AddModelError("Password", res.Message);
                return View(loginUserCommandRequest);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(PasswordResetCommandRequest passwordResetCommandRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            PasswordResetCommandResponse response = await _mediator.Send(passwordResetCommandRequest);
            return Ok(response);
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
            var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticationResult.Succeeded || string.IsNullOrEmpty(authenticationResult.Principal?.Identity?.Name))
            {
                // Handle the login failure (e.g., return to login page or display an error)
                return RedirectToAction("Login");
            }

            // Verify the nonce to protect against replay attacks
            if (!authenticationResult.Properties.Items.TryGetValue("nonce", out var nonce) ||
                !HttpContext.Session.TryGetValue("GoogleLoginNonce", out var storedNonce) ||
                nonce != Encoding.UTF8.GetString(storedNonce))
            {
                // Nonce doesn't match, handle the error (e.g., return to login page)
                return RedirectToAction("Login");
            }

            // Clear the nonce from the session
            HttpContext.Session.Remove("GoogleLoginNonce");

            // Successful login; you can access the user's information here

            var googleLoginCommandRequest = new GoogleLoginCommandRequest
            {
                FirstName = GetValueFromClaim(authenticationResult.Principal, ClaimTypes.Name),
                LastName = GetValueFromClaim(authenticationResult.Principal, ClaimTypes.Surname),
                Email = GetValueFromClaim(authenticationResult.Principal, ClaimTypes.Email),
                Name = GetValueFromClaim(authenticationResult.Principal, ClaimTypes.Name),
                Provider = "Google",
                IdToken = authenticationResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };

            await _mediator.Send(googleLoginCommandRequest);

            // Add your logic for user registration or any other operations here

            return Redirect(returnUrl);
        }

        private string GetValueFromClaim(ClaimsPrincipal principal, string claimType)
        {
            return principal.FindFirst(claimType)?.Value;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]CreateUserCommandRequest createUserCommandRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            if (!response.Success)
            {
                ModelState.AddModelError("Password", response.Message);
                return View(createUserCommandRequest);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("auth/updatepassword/{userId}")]
        public async Task<IActionResult> UpdatePassword([FromRoute] string userId)
        {
            GetUserByIdQueryRequest getUserByIdQueryRequest = new GetUserByIdQueryRequest { UserId = userId };
            GetUserByIdQueryResponse getUserByIdQueryHandler = await _mediator.Send(getUserByIdQueryRequest);
            if (!getUserByIdQueryHandler.Result) { return NotFound("User not Found"); }

            UpdatePasswordCommandRequest updatePasswordCommandRequest = new UpdatePasswordCommandRequest { UserId = userId };

            return View(updatePasswordCommandRequest);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            if (response is null) return BadRequest(ModelState);
            return View(nameof(Login));
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

        //[HttpPost("assign-role-to-user")]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Assign Role To User", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }

        public async Task<IActionResult> Logout()
        {
            var res = await _mediator.Send(new LogoutUserCommandRequest());

            if (res is null) return BadRequest(res?.Message);

            return RedirectToAction("Index", "Home");
        }
    }
}
