using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Coaches.Commands.CreateCoachCommand;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachByUserIdQuery;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoachesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(new { message = "Coach registrado exitosamente." });
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoach(int id)
        {
            var result = await _mediator.Send(new GetCoachQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCoachByUserId(int userId)
        {
            var result = await _mediator.Send(new GetCoachByUserIdQuery { UserId = userId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
