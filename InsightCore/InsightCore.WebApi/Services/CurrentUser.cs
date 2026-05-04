using InsightCore.Application.Interface.Presentation;
using InsightCore.Application.UseCases.Common.Constants;
using System.Security.Claims;

namespace InsightCore.WebApi.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("Id") ?? GlobalConstant.DefaultUserId;

        public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("UserName") ?? GlobalConstant.DefaultUserName;
    }
}
