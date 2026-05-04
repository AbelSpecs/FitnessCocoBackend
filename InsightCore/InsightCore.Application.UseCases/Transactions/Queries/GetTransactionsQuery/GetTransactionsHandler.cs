using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Transactions.Queries.GetTransactionsQuery
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, Response<List<TransactionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTransactionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<List<TransactionDto>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<List<TransactionDto>>();

                var userTransaction = await _unitOfWork.Transactions.GetTransactionsByUserIdAsync(request.userId);

                if (userTransaction is null)
                {
                    response.IsSuccess = true;
                    response.Message = "El usuario no existe.";
                    return response;
                }

                var transactionEntity = _mapper.Map<List<TransactionDto>>(userTransaction);

                // 5. Retornar respuesta exitosa
                return new Response<List<TransactionDto>>
                {
                    Data = transactionEntity,
                    IsSuccess = true,
                    Message = "Usuario existente."
                };
     
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<List<TransactionDto>> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }

        }

    }
}
