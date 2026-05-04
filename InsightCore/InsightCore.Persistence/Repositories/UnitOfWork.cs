using InsightCore.Application.Interface.Persistence;
using InsightCore.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public IUsersRepository Users { get; }
        public ITransactionsRepository Transactions { get; }
        public IStudentsRepository Students { get; }
        public ICoachStudentsRepository CoachStudents { get; }
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(IUsersRepository users,ITransactionsRepository transactions, ApplicationDbContext applicationDbContext)
        {
            Users = users;
            Transactions = transactions;
            Students = new StudentsRepository(applicationDbContext);
            CoachStudents = new CoachStudentsRepository(applicationDbContext);
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}
