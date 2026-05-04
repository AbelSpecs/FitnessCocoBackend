using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public record TokenResponse(string Token, DateTime Expiration);
}
