namespace Core.Services.AuthService
{
    public class AuthModel
    {
        private AuthModel()
        {
        }

        public AuthModel(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}