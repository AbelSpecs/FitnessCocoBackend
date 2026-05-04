using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.UseCases
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
