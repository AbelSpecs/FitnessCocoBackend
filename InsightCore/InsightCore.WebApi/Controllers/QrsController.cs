using InsightCore.Application.UseCases.Qrs.Commands.CreateQrForCoachCommand;
using InsightCore.Application.UseCases.Qrs.Queries;
using InsightCore.Application.UseCases.Students.Queries.GetStudentQuery;
using InsightCore.Application.UseCases.Students.Queries.GetStudentsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class QrsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QrsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("GenerateQr/{coachId}")]
        public async Task<IActionResult> GenerateQr(int coachId)
        {
            var result = await _mediator.Send(new CreateQrForCoachCommand { CoachId = coachId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet("redirect/{token}")]
        public async Task<IActionResult> RedirectToCoach(string token)
        {
            var result = await _mediator.Send(new RedirectToCoachQuery { Token = token });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

    }
}
