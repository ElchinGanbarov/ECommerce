﻿namespace ECommerce.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(string usernameOrEmail, string password, DateTime exparitionDateTime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
