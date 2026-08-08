using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PyrosFit.Application.Features.Streaks.Events;
using PyrosFit.Application.Features.Streaks.Queries;
using PyrosFit.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StreaksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StreaksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint para notificar que un alumno completó un entrenamiento
        [AllowAnonymous]
        [HttpPost("workout-completed")]
        public async Task<IActionResult> WorkoutCompleted([FromBody] WorkoutCompletedRequest request)
        {
            if (request == null) return BadRequest();

            var notification = new DailyWorkoutCompletedNotification(request.StudentId, request.ActivityDate);
            await _mediator.Publish(notification);
            return Accepted();
        }

        // Endpoint para obtener el radar de riesgo de los estudiantes de un coach
        [AllowAnonymous]
        [HttpGet("coach/{coachId}/risk-radar")]
        public async Task<IActionResult> GetCoachRiskRadar(int coachId)
        {
            var query = new GetCoachStudentsRiskRadarQuery(coachId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        public class WorkoutCompletedRequest
        {
            public int StudentId { get; set; }
            public System.DateTime ActivityDate { get; set; }
        }
    }
}
