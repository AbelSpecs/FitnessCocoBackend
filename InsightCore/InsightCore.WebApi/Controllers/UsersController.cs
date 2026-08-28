
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Students.Queries.GetStudentQuery;
using InsightCore.Application.UseCases.Users.Commands.CreateUserTokenCommand;
using InsightCore.Application.UseCases.Users.Commands.LoginUserCommand;
using InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand;
using InsightCore.Application.UseCases.Users.Commands.ConfirmEmailCommand;
using InsightCore.Application.UseCases.Users.Commands.ForgotPasswordCommand;
using InsightCore.Application.UseCases.Users.Commands.ResetPasswordCommand;
using InsightCore.Application.UseCases.Users.Queries.GetUserQuery;
using InsightCore.Application.UseCases.Users.Queries.GetUserRolesDetailQuery;
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
        private readonly InsightCore.Application.Interface.Presentation.ICurrentUser _currentUser;
        public UsersController(IOptions<AppSettings> appSettings, IMediator mediator, InsightCore.Application.Interface.Presentation.ICurrentUser currentUser)
        {
            _appSettings = appSettings.Value;
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
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

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetUserQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet("{userId}/details")]
        public async Task<IActionResult> GetUserDetail(int userId)
        {
            var result = await _mediator.Send(new GetUserDetailQuery { UserId = userId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [HttpPut("{userId}/profilePictures")]
        public async Task<IActionResult> UpdateProfilePictures(int userId, [FromBody] InsightCore.Application.UseCases.Users.Commands.UpdateProfilePictureCommand.UpdateProfilePictureCommand command)
        {
            if (command == null) return BadRequest();
            // Validar que el usuario autenticado corresponde al userId solicitado
            if (!int.TryParse(_currentUser.UserId, out var currentUserId) || currentUserId != userId)
            {
                return Forbid();
            }

            command.UserId = userId;
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
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
