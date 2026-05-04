using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand;
using InsightCore.Application.UseCases.CoachStudents.Queries.GetCoachStudentQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CoachStudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoachStudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AssignStudent([FromBody] AssignStudentCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{coachId}/{studentId}")]
        public async Task<IActionResult> GetCoachStudent(int coachId, int studentId)
        {
            var result = await _mediator.Send(new GetCoachStudentQuery { CoachId = coachId, StudentId = studentId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
