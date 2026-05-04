using InsightCore.Application.UseCases.Transactions.Queries.GetTransactionsQuery;
using InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand;
using InsightCore.WebApi.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IMediator _mediator;

        public TransactionsController(IOptions<AppSettings> appSettings, IMediator mediator)
        {
            _appSettings = appSettings.Value;
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpGet("Transaction")]
        public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsQuery command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}
