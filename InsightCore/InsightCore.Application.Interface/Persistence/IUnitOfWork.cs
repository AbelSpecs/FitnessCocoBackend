using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository Users { get; }
        ITransactionsRepository Transactions { get; }
        IStudentsRepository Students { get; }
        ICoachStudentsRepository CoachStudents { get; }
        ICoachesRepository Coaches { get; }
        Task<int> Save(CancellationToken cancellationToken);
    }
}
