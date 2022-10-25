using ETicaretAPI.Application.Abstractions.Tokens;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandle : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Idenity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Idenity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandle(SignInManager<Domain.Entities.Idenity.AppUser> signInManager, UserManager<Domain.Entities.Idenity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Kullanıcı vey şifre hatalı...");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded) //Authentication başarılı!
            {
                var token = _tokenHandler.CreateAccessToken(15);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            };

            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı Adı veya Şifre Hatalı..."
            //};
            throw new AuthenticationErrorException();
    }
    }
}
