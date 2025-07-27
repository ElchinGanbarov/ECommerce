
// example aded for learning
global using ECommerce.Application;
global using ECommerce.Application.Abstractions.Services;
global using ECommerce.Application.CustomAttributes;
global using ECommerce.Application.Features.Commands.AppUser.AssignRoleToUser;
global using ECommerce.Application.Features.Commands.AppUser.LoginUser;
global using ECommerce.Application.Features.Commands.CreateUser;
global using ECommerce.Application.Features.Commands.UpdatePassword;
global using ECommerce.Application.Features.Queries.AppUser.GetAllUsers;
global using ECommerce.Application.Features.Queries.AppUser.GetRolesToUser;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication.Google;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using System.Security.Claims;
global using System.Text;
global using Microsoft.AspNetCore.Identity;
global using ECommerce.Application.Features.Commands.AppUser.GoogleLogin;
global using ECommerce.Application.Features.Commands.AppUser.PasswordReset;
global using ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
global using ECommerce.Application.Features.Queries.AppUser.GetUserById;
global using Microsoft.AspNetCore.Http;
global using ECommerce.Application.Features.Commands.AppUser.LogoutUser;