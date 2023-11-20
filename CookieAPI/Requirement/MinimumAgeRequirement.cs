using Microsoft.AspNetCore.Authorization;

namespace CookieAPI.Requirement
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int minimumAge { get; set; }
        public MinimumAgeRequirement(int _minimumAge)
        {
           minimumAge = _minimumAge;
        }
    }
}
