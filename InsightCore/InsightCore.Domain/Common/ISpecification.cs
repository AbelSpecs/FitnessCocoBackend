using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Domain.Common
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
