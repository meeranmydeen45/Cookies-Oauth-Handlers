using Microsoft.AspNetCore.Authorization;

namespace CookieAPI.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MinimumAgeAuthorizeAttribute : AuthorizeAttribute 
    { 
        const string POLICY_PREFIX = "MinimumAge";

        public MinimumAgeAuthorizeAttribute(int age)
        {
            Age = age;
        }

        public int Age
        {
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}
