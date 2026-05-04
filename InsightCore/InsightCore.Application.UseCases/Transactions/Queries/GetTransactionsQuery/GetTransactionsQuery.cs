using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InsightCore.Application.UseCases.Transactions.Queries.GetTransactionsQuery
{
    public sealed record GetTransactionsQuery : IRequest<Response<List<TransactionDto>>>
    {
        [Required(ErrorMessage = "El userId es obligatorio")]
        public required int userId { get; set; }
    }
}
