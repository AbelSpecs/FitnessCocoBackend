
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Users.Commands.CreateUserTokenCommand;
using InsightCore.Application.UseCases.Users.Commands.LoginUserCommand;
using InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand;
using InsightCore.Transversal.Common;
using InsightCore.WebApi.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IMediator _mediator;
        public UsersController(IOptions<AppSettings> appSettings, IMediator mediator)
        {
            _appSettings = appSettings.Value;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(new { message = "Usuario registrado. Por favor, revisa tu correo." });
            }

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                // El resultado debe contener el Token JWT y datos del usuario
                return Ok(result);
            }

            // Si las credenciales son inválidas (401) o hay error de validación (400)
            return Unauthorized(result);
        }

        private string BuildToken(Response<UserDto> usersDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usersDto.Data.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
