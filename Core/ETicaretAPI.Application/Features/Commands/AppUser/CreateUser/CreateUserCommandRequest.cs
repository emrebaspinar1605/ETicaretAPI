using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandRequest :IRequest<CreateUserCommandResponse>
    {
        public string NameSurname { get; set; }
        public string Username { get; set; }
        public string Email{ get; set; }
        public string Password{ get; set; }
        public string PasswordConfirm { get; set; }
    }
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
    public class CreateUserCommandResponse 
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
