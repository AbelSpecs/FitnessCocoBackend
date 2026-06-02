using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.DailyExerciseSets.Commands.CreateDailyExerciseSetCommand;
using InsightCore.Application.UseCases.DailyExerciseSets.Commands.UpdateDailyExerciseSetCommand;
using InsightCore.Application.UseCases.DailyExerciseSets.Commands.DeleteDailyExerciseSetCommand;
using InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetsQuery;
using InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DailyExerciseSetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DailyExerciseSetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetDailyExerciseSetsQuery());
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetDailyExerciseSetQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDailyExerciseSetCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("Daily student exercise not found")) return NotFound(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DailyExerciseSetDto dto)
        {
            var result = await _mediator.Send(new UpdateDailyExerciseSetCommand { Id = id, Set = dto });
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("Daily student exercise not found")) return NotFound(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDailyExerciseSetCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
