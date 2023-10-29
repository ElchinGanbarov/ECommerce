using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(DateTime exparitionDateTime, AppUser appUser);
        string CreateRefreshToken();
    }
}
