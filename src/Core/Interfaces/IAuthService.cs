using Core.ActionResults;
using Core.Services.AuthService;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        public ApplicationActionResult<JwtTokenModel> Login(AuthModel authModel);
        public BaseActionResult Logout(JwtTokenModel tokenModel);
    }
}