using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Coaches.Commands.CreateCoachCommand;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
    }
}
