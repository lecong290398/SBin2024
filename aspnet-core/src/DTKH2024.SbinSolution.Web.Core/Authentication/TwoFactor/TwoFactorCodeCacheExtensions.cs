using Abp.Runtime.Caching;
using DTKH2024.SbinSolution.Authentication.TwoFactor;

namespace DTKH2024.SbinSolution.Web.Authentication.TwoFactor
{
    public static class TwoFactorCodeCacheExtensions
    {
        public static ITypedCache<string, TwoFactorCodeCacheItem> GetTwoFactorCodeCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, TwoFactorCodeCacheItem>(TwoFactorCodeCacheItem.CacheName);
        }
    }
}
