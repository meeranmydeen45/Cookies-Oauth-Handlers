using CookieAPI.Requirement;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CookieAPI.PolicyProvider
{
    public class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "MinimumAge";
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

        public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
        {
             BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder("Cookie").RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
           int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var age))
            {
                var policy = new AuthorizationPolicyBuilder("Cookie");
                policy.AddRequirements(new MinimumAgeRequirement(age));
                return await Task.FromResult(policy.Build());
            }

            return await BackupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
