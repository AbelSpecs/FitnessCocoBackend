using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.MuscleGroups.Commands.CreateMuscleGroupCommand;
using InsightCore.Application.UseCases.MuscleGroups.Commands.UpdateMuscleGroupCommand;
using InsightCore.Application.UseCases.MuscleGroups.Commands.DeleteMuscleGroupCommand;
using InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupsQuery;
using InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MuscleGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MuscleGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetMuscleGroupsQuery());
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetMuscleGroupQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuscleGroupCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMuscleGroupCommand command)
        {
            if (id != command.Id) return BadRequest(new Transversal.Common.Response<object> { IsSuccess = false, Message = "Id mismatch." });
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteMuscleGroupCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
