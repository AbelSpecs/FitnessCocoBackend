using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.DailyStudentExercises.Commands.AssignDailyExerciseCommand;
using InsightCore.Application.UseCases.DailyStudentExercises.Commands.CompleteDailyExerciseCommand;
using InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DailyStudentExercisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DailyStudentExercisesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignDailyExerciseCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompleteExerciseDto dto)
        {
            var result = await _mediator.Send(new CompleteDailyExerciseCommand { Id = id, Complete = dto });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var result = await _mediator.Send(new GetDailyExercisesByStudentQuery { StudentId = studentId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet("student/{studentId}/date/{date}")]
        public async Task<IActionResult> GetByStudentAndDate(int studentId, DateTime date)
        {
            var result = await _mediator.Send(new InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentAndDateQuery.GetDailyExercisesByStudentAndDateQuery { StudentId = studentId, Date = date });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
