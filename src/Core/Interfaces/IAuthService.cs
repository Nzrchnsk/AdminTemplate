using System.Threading.Tasks;
using Core.ActionResults;
using Core.Services.AuthService;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        public Task<ApplicationActionResult<AuthResult>> Login(AuthModel authModel);
        public Task<ApplicationActionResult<AuthResult>> Refresh(JwtTokenModel authToken);

        public BaseActionResult Logout(JwtTokenModel tokenModel);
    }
}