using Microsoft.EntityFrameworkCore;
using DTKH2024.SbinSolution.OpenIddict.Applications;
using DTKH2024.SbinSolution.OpenIddict.Authorizations;
using DTKH2024.SbinSolution.OpenIddict.Scopes;
using DTKH2024.SbinSolution.OpenIddict.Tokens;

namespace DTKH2024.SbinSolution.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}