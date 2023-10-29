using MediatR;

namespace ECommerce.Application.Features.Queries.AppUser.GetUserById
{
	public class GetUserByIdQueryRequest : IRequest<GetUserByIdQueryResponse>
	{
		public string UserId { get; set; }
	}
}
