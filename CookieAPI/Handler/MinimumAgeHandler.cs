using CookieAPI.Requirement;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CookieAPI.Handler
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                var birthDate = DateTime.Parse(context.User.FindFirst(ClaimTypes.DateOfBirth).Value);
                int age = DateTime.Now.Year - birthDate.Year;

                if(birthDate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }
                if(age >= requirement.minimumAge)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
