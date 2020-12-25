using System.Numerics;
using System.Threading.Tasks;
using Core.ActionResults;
using Core.Interfaces;
using Core.Services.AuthService;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private JwtTokenHelper _tokenHelper;

        private AuthService()
        {
            
        }
        
        public AuthService(UserManager<ApplicationUser> userManager, JwtTokenHelper tokenHelper)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
        }
        
        public async Task<ApplicationActionResult<AuthResult>> Login(AuthModel authModel)
        {
            AuthResult authResult = new AuthResult();
            var user = await _userManager.FindByNameAsync(authModel.Login);
            if (user == null)
            {
                authResult.InvalidLogin();
                return new ApplicationActionResult<AuthResult>(authResult,(int) Core.Constants.ActionStatuses.Fail, "");
            }

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, authModel.Password);
            if (!checkPasswordResult)
            {
                authResult.InvalidPassword();
                return new ApplicationActionResult<AuthResult>(authResult,(int) Core.Constants.ActionStatuses.Fail, "");
            }
            
            JwtTokenModel token = await _tokenHelper.CreateJwtTokens(user);
            authResult.SuccessAuth(token);
            return new ApplicationActionResult<AuthResult>(authResult,(int) Core.Constants.ActionStatuses.Success, "");
        }


        public async Task<ApplicationActionResult<AuthResult>> Refresh(JwtTokenModel authToken)
        {
            AuthResult refreshResult = new AuthResult();
            string userLogin = _tokenHelper.GetUserNameFromExpiredToken(authToken.AccessToken);
            ApplicationUser user = await _userManager.FindByNameAsync(userLogin);
            //Validate refresh token
            if (user.RefreshToken != authToken.RefreshToken)
            {
                refreshResult.InvalidRefreshToken();
                return new ApplicationActionResult<AuthResult>(refreshResult,(int) Core.Constants.ActionStatuses.Fail, "");
            }
            //create access and refresh token
            var token = await _tokenHelper.CreateJwtTokens(user);
            //save user refresh token in database
            user.SetRefreshToken(token.RefreshToken);
            await _userManager.UpdateAsync(user);
            refreshResult.SuccessAuth(token);
            return new ApplicationActionResult<AuthResult>(refreshResult,(int) Core.Constants.ActionStatuses.Success, "");
        }
        
        public BaseActionResult Logout(JwtTokenModel tokenModel)
        {
            throw new System.NotImplementedException();
        }
    }
}