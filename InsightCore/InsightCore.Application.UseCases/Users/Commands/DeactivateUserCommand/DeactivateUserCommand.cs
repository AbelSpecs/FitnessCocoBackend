using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Users.Commands.DeactivateUserCommand
{
    public class DeactivateUserCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
