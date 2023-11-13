using ECommerce.Application.Results;

namespace ECommerce.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<IDataResult<DTOs.Token>> LoginAsync(string usernameOrEmail, string password, DateTime exparitionDateTime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
        Task<IResult> LogoutAsync();

    }
}
