using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DTKH2024.SbinSolution.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly SbinSolutionAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new SbinSolutionAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
