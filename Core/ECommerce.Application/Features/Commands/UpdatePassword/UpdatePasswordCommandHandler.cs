using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Commands.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
                throw new PasswordChangeFailedException("Lütfen şifreyi birebir doğrulayınız.");

            await _userService.UpdatePasswordAsync(request.UserId, request.Password);
            return new();
        }
    }
}
