using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Net.Emailing
{
    public interface IEmailSettingsChecker
    {
        bool EmailSettingsValid();

        Task<bool> EmailSettingsValidAsync();
    }
}