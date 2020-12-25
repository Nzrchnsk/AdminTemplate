using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Services.AuthService;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.AuthService
{
    public class JwtTokenHelper
    {
        private string _issuer;
        private string _audience;
        private string _secureKey;
        private string _lifeTime;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenHelper(string issuer, string audience, string secureKey,
            UserManager<ApplicationUser> userManager, string lifeTime)
        {
            _issuer = issuer;
            _audience = audience;
            _secureKey = secureKey;
            _lifeTime = lifeTime;
            _userManager = userManager;
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = _issuer,
                ValidIssuer = _audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secureKey)),
                ValidateLifetime = false
            };
        }

        private readonly TokenValidationParameters _tokenValidationParameters;

        /// <summary>
        /// создание и возврат Jwt токенов
        /// </summary>
        /// <param name="user">почта пользователя</param>
        /// <returns>токены доступа</returns>
        public async Task<JwtTokenModel> CreateJwtTokens(ApplicationUser user)
        {
            //создание клеймов по почте пользователя
            var claims = await CreateAuthClime(user);
            //генерация токенов
            var tokens = new JwtTokenModel(GenerateAccessToken(claims), GenerateRefreshToken());
            if (user.Name != "guest")
            {
                user.SetRefreshToken(tokens.RefreshToken);
                await _userManager.UpdateAsync(user);
            }

            return tokens;
        }


        /// <summary>
        /// создание клеймов для генерации токенов пользователя
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns>набор клеймов</returns>
        private async Task<Claim[]> CreateAuthClime(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            return new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        /// <summary>
        /// генерация токена доступа
        /// </summary>
        /// <param name="claims">набор клеймов</param>
        /// <returns>токен доступа</returns>
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(Convert.ToDouble(_lifeTime))),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secureKey)),
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// генерация токенов обновления
        /// </summary>
        /// <returns>токен обновления</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// получение почты пользователя из истекшего токена доступа
        /// </summary>
        /// <param name="token">токен доступа</param>
        /// <returns>почта</returns>
        /// <exception cref="SecurityTokenException">неверный токен</exception>
        public string GetUserNameFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal.Identity.Name;
        }

        public JwtSecurityToken AccessToSecurity(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            return jwtSecurityToken;
        }
    }
}