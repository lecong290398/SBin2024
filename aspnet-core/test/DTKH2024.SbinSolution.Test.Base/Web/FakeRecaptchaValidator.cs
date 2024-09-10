using System.Threading.Tasks;
using DTKH2024.SbinSolution.Security.Recaptcha;

namespace DTKH2024.SbinSolution.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
