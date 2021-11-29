using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Extensions
{
    public class CustomAuthorization
    {
        public static bool IsValidUserClaim(HttpContext context, string claimName, string claimValue)
        {
            var roleUsuario = context.User.GetUserRole();
            return context.User.Identity.IsAuthenticated && claimName.Contains(roleUsuario);
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string role) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(role, role) };
        }

        public ClaimsAuthorizeAttribute(params string[] roles) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(String.Join(',', roles), String.Join(',', roles)) };
        }

    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
            }

            if (!CustomAuthorization.IsValidUserClaim(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
