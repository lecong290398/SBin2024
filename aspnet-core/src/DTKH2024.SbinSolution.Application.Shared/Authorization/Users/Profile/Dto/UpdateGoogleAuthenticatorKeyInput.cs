using System.Collections.Generic;

namespace DTKH2024.SbinSolution.Authorization.Users.Profile.Dto
{
    public class UpdateGoogleAuthenticatorKeyInput
    {
        public string GoogleAuthenticatorKey { get; set; }
        public string AuthenticatorCode { get; set; }
    }
}
