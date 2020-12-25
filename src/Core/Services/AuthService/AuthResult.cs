using Core.Services.AuthService;

namespace Core.Services.AuthService
{
    public class AuthResult
    {
        public void InvalidLogin()
        {
            Message = "Invalid login";
        }

        public void InvalidPassword()
        {
            Message = "Invalid password";
        }

        public void InvalidRefreshToken()
        {
            Message = "invalid refresh token";
        }

        public AuthResult()
        {
            AuthToken = null;
            Message = "Authentication failed";
        }

        public void SuccessAuth(JwtTokenModel authToken)
        {
            AuthToken = authToken;
            Message = "Authentication successful";
        }
        public JwtTokenModel AuthToken { get; private set; }
        public string Message { get; private set; }
    }
}