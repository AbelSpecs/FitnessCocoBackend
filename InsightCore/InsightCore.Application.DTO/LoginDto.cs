using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public record LoginDto(
     string Token,
     DateTime Expiration,
     string UserName,
     string Email,
     int id
 );
}
