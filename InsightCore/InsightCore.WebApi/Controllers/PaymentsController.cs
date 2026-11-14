using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Payments.Commands.CreatePaymentCommand;
using InsightCore.Application.UseCases.Payments.Commands.UpdatePaymentCommand;
using InsightCore.Application.UseCases.Payments.Queries.GetPaymentByIdQuery;
using InsightCore.Application.UseCases.Payments.Queries.GetPaymentsQuery;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    /// <summary>
    /// Gestión de Pagos — soporta múltiples monedas (VES, USD, USDT)
    /// y métodos (Transferencia, Pago Móvil, Zelle, Binance Pay, Efectivo, etc.)
    /// con trazabilidad de referencias bancarias y hashes de transacción.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payments")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registra un nuevo pago en el sistema.
        /// El estado inicial siempre es <c>Pendiente</c> hasta que un coach
        /// o administrador lo apruebe mediante el endpoint PUT.
        /// </summary>
        /// <remarks>
        /// Monedas: 1=VES (Bolívares), 2=USD, 3=USDT
        ///
        /// Métodos: 1=Transferencia, 2=PagoMovil, 3=Zelle, 4=BinancePay, 5=Efectivo, 6=PayPal, 99=Otro
        ///
        /// Estado:  1=Pendiente (default), 2=Aprobado, 3=Rechazado
        /// </remarks>
        /// <param name="command">Datos del pago: monto, moneda, método y referencia.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El pago creado con su Id asignado.</returns>
        /// <response code="200">Pago registrado correctamente.</response>
        /// <response code="400">Datos inválidos (monto ≤ 0, referencia faltante, incoherencia moneda/método).</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Actualiza los datos o el estado de un pago existente.
        /// Caso de uso principal: cambiar estado de <c>Pendiente</c> → <c>Aprobado</c> o <c>Rechazado</c>.
        /// </summary>
        /// <param name="id">Id del pago a modificar.</param>
        /// <param name="command">Nuevos datos del pago.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El pago actualizado.</returns>
        /// <response code="200">Pago actualizado correctamente.</response>
        /// <response code="400">Datos inválidos o Id no coincide.</response>
        /// <response code="404">Pago no encontrado.</response>
        [AllowAnonymous]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new Response<PaymentDto>
                {
                    IsSuccess = false,
                    Message = "El Id de la ruta no coincide con el Id del cuerpo de la petición."
                });
            }

            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);

            if (result.Message?.Contains("No se encontró") == true)
                return NotFound(result);

            return BadRequest(result);
        }

        /// <summary>
        /// Lista pagos con paginación y filtros opcionales.
        /// </summary>
        /// <remarks>
        /// Filtra por: userId, studentId, moneda ("VES"|"USD"|"USDT"),
        /// estado ("Pendiente"|"Aprobado"|"Rechazado") y rango de fechas.
        /// </remarks>
        /// <param name="userId">Filtrar por UserId (opcional).</param>
        /// <param name="studentId">Filtrar por StudentId (opcional).</param>
        /// <param name="currency">Filtrar por moneda: "VES", "USD" o "USDT" (opcional).</param>
        /// <param name="status">Filtrar por estado: "Pendiente", "Aprobado" o "Rechazado" (opcional).</param>
        /// <param name="from">Fecha de inicio UTC (opcional).</param>
        /// <param name="to">Fecha de fin UTC (opcional).</param>
        /// <param name="page">Número de página base-1 (por defecto: 1).</param>
        /// <param name="pageSize">Tamaño de página, máximo 100 (por defecto: 20).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista paginada de pagos.</returns>
        /// <response code="200">Lista de pagos.</response>
        /// <response code="400">Parámetros inválidos.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(Response<PagedResult<PaymentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PagedResult<PaymentDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? userId,
            [FromQuery] int? studentId,
            [FromQuery] string? currency,
            [FromQuery] string? status,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPaymentsQuery
            {
                UserId    = userId,
                StudentId = studentId,
                Currency  = currency,
                Status    = status,
                From      = from,
                To        = to,
                Page      = page,
                PageSize  = pageSize
            };

            var result = await _mediator.Send(query, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Obtiene el detalle completo de un pago por su Id.
        /// </summary>
        /// <param name="id">Id del pago.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Detalle del pago.</returns>
        /// <response code="200">Detalle del pago.</response>
        /// <response code="404">Pago no encontrado.</response>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PaymentDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery { Id = id }, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
        }
    }
}
