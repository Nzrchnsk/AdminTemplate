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
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        
    }
}