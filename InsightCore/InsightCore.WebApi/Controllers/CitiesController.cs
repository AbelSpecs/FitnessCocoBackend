using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Cities.Commands.CreateCityCommand;
using InsightCore.Application.UseCases.Cities.Commands.DeleteCityCommand;
using InsightCore.Application.UseCases.Cities.Commands.UpdateCityCommand;
using InsightCore.Application.UseCases.Cities.Queries.GetCitiesByCountryQuery;
using InsightCore.Application.UseCases.Cities.Queries.GetCitiesQuery;
using InsightCore.Application.UseCases.Cities.Queries.GetCityQuery;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.DeleteCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
using InsightCore.Application.UseCases.Countries.Queries.GetCountriesQuery;
using InsightCore.Application.UseCases.Countries.Queries.GetCountryQuery;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCitiesQuery());
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{countryId}")]
        public async Task<IActionResult> Get(int countryId)
        {
            var result = await _mediator.Send(new GetCityQuery { CountryId = countryId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCityCommand command)
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
            var result = await _mediator.Send(new DeleteCityCommand { Id = id });
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("country/{countryId}")]
        public async Task<IActionResult> GetCitiesByCountry(int countryId)
        {
            var result = await _mediator.Send(new GetCitiesByCountryQuery { CountryId = countryId });
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}

