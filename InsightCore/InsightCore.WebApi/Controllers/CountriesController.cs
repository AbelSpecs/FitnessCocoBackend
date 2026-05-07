using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.DeleteCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
using InsightCore.Application.UseCases.Countries.Queries.GetCountryQuery;
using InsightCore.Application.UseCases.Countries.Queries.GetCountriesQuery;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCountriesQuery());
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetCountryQuery { Id = id });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCountryCommand command)
        {
            if (id != command.Id) return BadRequest(new Response<CountryDto> { IsSuccess = false, Message = "Id mismatch." });
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCountryCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
