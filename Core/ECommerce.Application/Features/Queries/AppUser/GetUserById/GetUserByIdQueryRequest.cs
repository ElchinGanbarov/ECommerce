using MediatR;

namespace ECommerce.Application.Features.Queries.AppUser.GetUserById
{
	public record GetUserByIdQueryRequest(string UserId) : IRequest<GetUserByIdQueryResponse>;
}
