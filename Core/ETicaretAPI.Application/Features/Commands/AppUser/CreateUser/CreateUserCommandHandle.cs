using ETicaretAPI.Application.Abstractions.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandle : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Idenity.AppUser> _user;
        

        public CreateUserCommandHandle(UserManager<Domain.Entities.Idenity.AppUser> user)
        {
            _user = user;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
          IdentityResult result = await  _user.CreateAsync(new()
            {
                Id =Guid.NewGuid().ToString(),
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.NameSurname
            },request.Password);
            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded};
            if (result.Succeeded)
                response.Message = "Kullanıcı Başarıyla Oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description} ";

            return response;
        }
    }
}
