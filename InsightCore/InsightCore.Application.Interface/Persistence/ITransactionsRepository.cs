using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Persistence
{
    public interface ITransactionsRepository
    {
        Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    }
}
