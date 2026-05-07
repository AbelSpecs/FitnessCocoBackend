using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Gyms.Commands.CreateGymCommand;
using InsightCore.Application.UseCases.Gyms.Commands.UpdateGymCommand;
using InsightCore.Application.UseCases.Gyms.Commands.DeleteGymCommand;
using InsightCore.Application.UseCases.Gyms.Queries.GetGymQuery;
using InsightCore.Application.UseCases.Gyms.Queries.GetGymsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GymsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GymsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGymCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetGymQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGyms()
        {
            var result = await _mediator.Send(new GetGymsQuery());
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GymDto dto)
        {
            var result = await _mediator.Send(new UpdateGymCommand { Id = id, Gym = dto });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGymCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
