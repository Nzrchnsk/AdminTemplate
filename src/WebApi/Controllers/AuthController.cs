using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Services.AuthService;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.AuthService;
using Newtonsoft.Json.Linq;
using SharedDto;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Constants;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
          
        }


        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [Produces( typeof(AuthModel) )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<JwtTokenModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseModel<NoContentResponse>))]
        public async Task<IActionResult> Login([FromBody] AuthModel authModel)
        {
            var authResult = await _authService.Login(authModel);
            return authResult.Status switch
            {
                (int)Core.Constants.ActionStatuses.Fail => Unauthorized(new ResponseModel<NoContentResponse>(authResult.Result.Message, null)),
                // (int)Core.Constants.ActionStatuses.Success => Ok(new ResponseModel<AuthResult>("", authResult.Result))
                (int)Core.Constants.ActionStatuses.Success => Ok(authResult.Result)
            };
        }
        
        
        /// <summary>
        /// refresh by access и refresh tokens
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokens([FromBody]JwtTokenModel authToken)
        {
            var refreshResult = await _authService.Refresh(authToken);
            return refreshResult.Status switch
            {
                (int)Core.Constants.ActionStatuses.Fail => Unauthorized(new ResponseModel<NoContentResponse>(refreshResult.Result.Message, null)),
                // (int)Core.Constants.ActionStatuses.Success => Ok(new ResponseModel<AuthResult>("", refreshResult.Result))
                (int)Core.Constants.ActionStatuses.Success => Ok(refreshResult.Result)
            };
            
        }
        
        [HttpGet]
        [Route("checkAdminRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserRole()
        {
            var a = User;
            var b =a.FindAll(c => c.Type=="Role" ).ToList();
            var isInRole = a.IsInRole("Administrators");
            return Ok(isInRole);
        }
    }
}