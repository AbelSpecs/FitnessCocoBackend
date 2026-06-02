using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Exercises.Commands.CreateExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Commands.UpdateExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Commands.DeleteExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Queries.GetExerciseQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExercisesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("Muscle group not found")) return NotFound(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetExerciseQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExerciseUpdateDto dto)
        {
            var result = await _mediator.Send(new UpdateExerciseCommand { Id = id, Exercise = dto });
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("Muscle group not found")) return NotFound(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteExerciseCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
