namespace Core.Services.AuthService
{
    public class JwtTokenModel
    {
        private JwtTokenModel()
        {
            
        }

        public JwtTokenModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public void SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }
        
        public void SetRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}