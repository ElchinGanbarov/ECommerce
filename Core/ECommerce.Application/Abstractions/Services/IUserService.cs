using ECommerce.Application.DTOs.User;
using ECommerce.Application.Results;
using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<IResult> CreateAsync(CreateUser model);
		Task<bool> GetUserById(string userId);

		Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string newPassword);
        Task<List<ListUser>> GetAllUsersAsync(int page, int size);
        int TotalUsersCount { get; }
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
