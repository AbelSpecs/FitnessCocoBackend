using InsightCore.Application.DTO.Tickets;
using InsightCore.Application.UseCases.Tickets.Commands.CreateTicketCommand;
using InsightCore.Application.UseCases.Tickets.Commands.UpdateTicketStatusCommand;
using InsightCore.Application.UseCases.Tickets.Queries.GetAllTicketsQuery;
using InsightCore.Application.UseCases.Tickets.Queries.GetMyTicketsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private bool TryGetUserId(out int userId)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            return int.TryParse(claim, out userId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var userId = TryGetUserId(out var claimUserId) ? claimUserId : (dto.UserId ?? 0);
            if (userId <= 0)
            {
                return BadRequest(new { isSuccess = false, message = "El UserId del usuario es requerido." });
            }

            if (string.IsNullOrWhiteSpace(dto.BugName) || string.IsNullOrWhiteSpace(dto.Description))
            {
                return BadRequest(new { isSuccess = false, message = "El título del bug y la descripción son requeridos." });
            }

            var response = await _mediator.Send(new CreateTicketCommand(
                userId,
                dto.BugName,
                dto.Description,
                dto.Platform,
                dto.AppVersion,
                dto.DeviceModel));

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketsByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { isSuccess = false, message = "El userId es requerido." });
            }

            var response = await _mediator.Send(new GetMyTicketsQuery(userId));
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllTickets([FromQuery] bool? isAddressed)
        {
            var response = await _mediator.Send(new GetAllTicketsQuery(isAddressed));
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateTicketStatusDto dto)
        {
            var response = await _mediator.Send(new UpdateTicketStatusCommand(id, dto.IsAddressed, dto.ResolutionNotes));
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
