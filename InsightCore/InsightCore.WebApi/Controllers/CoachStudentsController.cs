using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoachStudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoachStudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public async Task<IActionResult> AssignStudent([FromBody] AssignStudentCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    if (result.IsSuccess) return Ok(result);
        //    return BadRequest(result);
        //}

        //[HttpGet("{coachId}/{studentId}")]
        //public async Task<IActionResult> GetCoachStudent(int coachId, int studentId)
        //{
        //    var result = await _mediator.Send(new GetCoachStudentQuery { CoachId = coachId, StudentId = studentId });
        //    if (result.IsSuccess) return Ok(result);
        //    return NotFound(result);
        //}
    }
}
