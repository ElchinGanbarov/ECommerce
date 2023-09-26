using ECommerce.Application.DTOs.User;

namespace ECommerce.Application.Features.Queries.AppUser.GetAllUsers
{
    public class GetAllUsersQueryResponse
    {
        public List<ListUser> Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}