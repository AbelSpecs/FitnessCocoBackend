using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Presentation
{
    public interface ICurrentUser
    {
        string? UserId { get; }
        string? UserName { get; }
    }
}
