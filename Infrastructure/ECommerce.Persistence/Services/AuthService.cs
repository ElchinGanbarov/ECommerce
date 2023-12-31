using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Facebook;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using ECommerce.Application.Helpers;
using ECommerce.Application.Results;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class AuthService : IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public AuthService(HttpClient httpClient,
                           IConfiguration configuration,
                           UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IUserService userService,
                           ITokenHandler tokenHandler,
                           IMailService mailService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _tokenHandler = tokenHandler;
            _mailService = mailService;
        }


        async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

                Token token = _tokenHandler.CreateAccessToken(DateTime.UtcNow.AddDays(accessTokenLifeTime), user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                await _signInManager.SignInAsync(user, true);
                
                return token;
            }
            throw new Exception("Invalid external authentication.");
        }
        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifeTime);
            }
            throw new Exception("Invalid external authentication.");
        }
        public async Task<Token> GoogleLoginAsync(string email, string name, string IdToken)
        {
            var info = new UserLoginInfo("GOOGLE", IdToken, "GOOGLE");

            Domain.Entities.Identity.AppUser user = await _userManager.FindByEmailAsync(email);

            return await CreateUserExternalAsync(user, email, name, info, 50);
        }

        public async Task<IDataResult<Token>> LoginAsync(string usernameOrEmail, string password, DateTime exparitonDateTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            
            if (user == null)
                return new ErrorDataResult<Token>("User not Found");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded) //Authentication başarılı!
            {
                Token token = _tokenHandler.CreateAccessToken(exparitonDateTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                await _signInManager.PasswordSignInAsync(usernameOrEmail, password, false, lockoutOnFailure: false);


                return new SuccessDataResult<Token>(token);
            }
            return new ErrorDataResult<Token>("Password Invalid");
        }

        public async Task<IResult> LogoutAsync()
        {
          await _signInManager.SignOutAsync();
          return new SuccessResult("logout user");
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(30), user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();
        }

        public async Task PasswordResetAsnyc(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
                resetToken = resetToken.UrlEncode();

                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }
    }
}
