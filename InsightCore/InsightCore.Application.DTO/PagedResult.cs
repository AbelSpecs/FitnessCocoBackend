using System.Collections.Generic;

namespace InsightCore.Application.DTO
{
    public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);
}
