using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Coaches.Commands.CreateCoachCommand;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachByUserIdQuery;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery;
using InsightCore.Application.UseCases.Coaches.Queries.GetStudentsListByCoachIdQuery;
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

        [AllowAnonymous]
        [HttpGet("profile/{coachId}")]
        public async Task<IActionResult> GetCoachProfile(int coachId)
        {
            var result = await _mediator.Send(new InsightCore.Application.UseCases.Coaches.Queries.GetCoachProfileQuery.GetCoachProfileQuery { CoachId = coachId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPost("{coachId}/rate")]
        public async Task<IActionResult> RateCoach(int coachId, [FromBody] InsightCore.Application.UseCases.Coaches.Commands.RateCoachCommand.RateCoachCommand command)
        {
            if (command == null) return BadRequest();
            // Ensure coachId path matches body
            command.CoachId = coachId;
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoach(int id, [FromBody] InsightCore.Application.UseCases.Coaches.Commands.UpdateCoachCommand.UpdateCoachCommand command)
        {
            if (command == null) return BadRequest();
            // Ensure id path matches body
            command.Id = id;
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("studentsList/{coachId}")]
        public async Task<IActionResult> GetStudentsListByCoachId(int coachId)
        {
            var result = await _mediator.Send(new GetStudentsListByCoachIdQuery { CoachId = coachId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
