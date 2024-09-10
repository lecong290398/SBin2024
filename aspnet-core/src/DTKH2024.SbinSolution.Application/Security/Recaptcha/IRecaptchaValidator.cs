using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}