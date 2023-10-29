using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Queries.AppUser.GetUserById
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
	{
		public readonly IUserService _userService;

		public GetUserByIdQueryHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _userService.GetUserById(request.UserId);
			return new GetUserByIdQueryResponse { Result = response };

		}
	}
}
