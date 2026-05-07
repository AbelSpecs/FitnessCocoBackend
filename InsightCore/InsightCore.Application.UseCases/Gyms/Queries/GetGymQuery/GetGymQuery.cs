using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Queries.GetGymQuery
{
    public class GetGymQuery : IRequest<Response<GymDto>>
    {
        public int Id { get; set; }
    }
}
