namespace ECommerce.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthentication
    {
        Task<DTOs.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
        Task<DTOs.Token> GoogleLoginAsync(string email, string name, string IdToken);
    }
}
