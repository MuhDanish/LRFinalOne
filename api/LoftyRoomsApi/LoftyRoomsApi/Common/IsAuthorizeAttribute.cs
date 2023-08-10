using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoftyRoomsApi.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        string claim = "";
        public IsAuthorizeAttribute(string _claim) {
            this.claim = _claim;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isUserAuthorized = context.HttpContext.User.HasClaim(x => x.Type == this.claim);

            if (!isUserAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
