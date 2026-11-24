using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Exercises.Commands.CreateExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Commands.UpdateExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Commands.DeleteExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Queries.GetExerciseQuery;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleGroupQuery;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByCoachQuery;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleNameQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private int? ResolveRequesterUserId(int? queryUserId)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (int.TryParse(claim, out var claimUserId) && claimUserId > 0)
                return claimUserId;

            return queryUserId.HasValue && queryUserId.Value > 0 ? queryUserId.Value : null;
        }

        [AllowAnonymous]
        [HttpGet("muscle-group/{muscleGroupId}")]
        public async Task<IActionResult> GetByMuscleGroup(int muscleGroupId)
        {
            var result = await _mediator.Send(new GetExercisesByMuscleGroupQuery { MuscleGroupId = muscleGroupId });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("by-muscle-name/{name}")]
        public async Task<IActionResult> GetByMuscleName(string name)
        {
            var result = await _mediator.Send(new GetExercisesByMuscleNameQuery { Name = name });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("coach/{coachId}")]
        public async Task<IActionResult> GetByCoach(int coachId)
        {
            var result = await _mediator.Send(new GetExercisesByCoachQuery { CoachId = coachId });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseCommand command, [FromQuery] int? userId = null)
        {
            command.RequesterUserId ??= ResolveRequesterUserId(userId);
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("No tienes permisos")) return StatusCode(StatusCodes.Status403Forbidden, result);
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
        public async Task<IActionResult> Update(int id, [FromBody] ExerciseUpdateDto dto, [FromQuery] int? userId = null)
        {
            var result = await _mediator.Send(new UpdateExerciseCommand
            {
                Id = id,
                Exercise = dto,
                RequesterUserId = ResolveRequesterUserId(userId)
            });
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("No tienes permisos")) return StatusCode(StatusCodes.Status403Forbidden, result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("Muscle group not found")) return NotFound(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int? userId = null)
        {
            var result = await _mediator.Send(new DeleteExerciseCommand
            {
                Id = id,
                RequesterUserId = ResolveRequesterUserId(userId)
            });
            if (result.IsSuccess) return Ok(result);
            if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("No tienes permisos")) return StatusCode(StatusCodes.Status403Forbidden, result);
            return NotFound(result);
        }
    }
}
