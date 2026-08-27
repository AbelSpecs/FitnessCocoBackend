using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PyrosFit.Application.DTOs;
using PyrosFit.Application.Features.Streaks.Commands;
using PyrosFit.Application.Features.Streaks.Events;
using PyrosFit.Application.Features.Streaks.Queries;
using System;
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

        /// <summary>
        /// Obtiene el estado actual de la racha de un estudiante.
        /// </summary>
        /// <param name="studentId">Identificador del estudiante.</param>
        [AllowAnonymous]
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentStreak(int studentId)
        {
            var result = await _mediator.Send(new GetStudentStreakQuery(studentId));
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Obtiene el historial de logs y eventos de racha de un estudiante.
        /// </summary>
        /// <param name="studentId">Identificador del estudiante.</param>
        /// <param name="limit">Cantidad máxima de registros a retornar (por defecto 30).</param>
        [AllowAnonymous]
        [HttpGet("student/{studentId}/history")]
        public async Task<IActionResult> GetStudentStreakHistory(int studentId, [FromQuery] int limit = 30)
        {
            var result = await _mediator.Send(new GetStudentStreakLogsQuery(studentId, limit));
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Registra la finalización de un entrenamiento y actualiza la racha del alumno.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("workout-completed")]
        public async Task<IActionResult> WorkoutCompleted([FromBody] WorkoutCompletedRequest request)
        {
            if (request == null) return BadRequest(new Response<StudentStreakDto> { IsSuccess = false, Message = "El payload no puede estar vacío." });

            var command = new RecordWorkoutStreakCommand(request.StudentId, request.ActivityDate);
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Utiliza un escudo de congelación para proteger la racha del estudiante ante inactividad.
        /// </summary>
        /// <param name="studentId">Identificador del estudiante.</param>
        /// <param name="request">Opcional: Fecha específica para la que se aplica el escudo.</param>
        [AllowAnonymous]
        [HttpPost("student/{studentId}/use-freeze-shield")]
        public async Task<IActionResult> UseFreezeShield(int studentId, [FromBody] UseFreezeShieldRequest? request = null)
        {
            var command = new UseFreezeShieldCommand(studentId, request?.ShieldDate);
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Permite ajustar manualmente la racha y escudos de un alumno (uso por Coach o Administrador).
        /// </summary>
        /// <param name="studentId">Identificador del estudiante.</param>
        /// <param name="dto">Valores a ajustar.</param>
        [AllowAnonymous]
        [HttpPost("student/{studentId}/adjust")]
        public async Task<IActionResult> AdjustStreak(int studentId, [FromBody] AdjustStreakDto dto)
        {
            if (dto == null) return BadRequest(new Response<StudentStreakDto> { IsSuccess = false, Message = "El payload no puede estar vacío." });

            var command = new AdjustStudentStreakCommand(studentId, dto.CurrentStreak, dto.LongestStreak, dto.FreezeShields, dto.Reason);
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Obtiene el radar de riesgo de abandono de los alumnos asignados a un coach.
        /// </summary>
        /// <param name="coachId">Identificador del coach.</param>
        [AllowAnonymous]
        [HttpGet("coach/{coachId}/risk-radar")]
        public async Task<IActionResult> GetCoachRiskRadar(int coachId)
        {
            var query = new GetCoachStudentsRiskRadarQuery(coachId);
            var result = await _mediator.Send(query);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Obtiene el ranking (leaderboard) de rachas de los alumnos de un coach.
        /// </summary>
        /// <param name="coachId">Identificador del coach.</param>
        /// <param name="limit">Cantidad máxima de alumnos en el ranking (por defecto 10).</param>
        [AllowAnonymous]
        [HttpGet("coach/{coachId}/leaderboard")]
        public async Task<IActionResult> GetCoachLeaderboard(int coachId, [FromQuery] int limit = 10)
        {
            var query = new GetCoachStreaksLeaderboardQuery(coachId, limit);
            var result = await _mediator.Send(query);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Obtiene el ranking global de rachas de la plataforma.
        /// </summary>
        /// <param name="limit">Cantidad máxima de alumnos en el ranking (por defecto 20).</param>
        [AllowAnonymous]
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetGlobalLeaderboard([FromQuery] int limit = 20)
        {
            var query = new GetGlobalStreaksLeaderboardQuery(limit);
            var result = await _mediator.Send(query);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Envía un correo motivacional al alumno desde el Radar de Riesgo (gatillado por WhatsApp o acción de seguimiento).
        /// </summary>
        [AllowAnonymous]
        [HttpPost("student/{studentId}/send-motivation")]
        public async Task<IActionResult> SendMotivation(int studentId, [FromBody] SendMotivationEmailRequest request)
        {
            if (request == null) return BadRequest(new Response<bool> { IsSuccess = false, Message = "El payload no puede estar vacío." });

            var command = new InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand.SendMotivationEmailCommand
            {
                StudentId = studentId,
                StudentEmail = request.StudentEmail,
                Message = request.Message,
                CoachName = request.CoachName
            };

            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Envía un correo motivacional con payload completo.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("send-motivation")]
        public async Task<IActionResult> SendMotivation([FromBody] InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand.SendMotivationEmailCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        public class WorkoutCompletedRequest
        {
            public int StudentId { get; set; }
            public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
        }

        public class UseFreezeShieldRequest
        {
            public DateTime? ShieldDate { get; set; }
        }

        public class SendMotivationEmailRequest
        {
            public string? StudentEmail { get; set; }
            public string Message { get; set; } = string.Empty;
            public string? CoachName { get; set; }
        }
    }
}
