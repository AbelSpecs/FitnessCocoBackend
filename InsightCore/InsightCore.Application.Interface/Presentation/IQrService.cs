using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Presentation
{
    public interface IQrService
    {
        string GenerateQrBase64(string content);
    }
}
