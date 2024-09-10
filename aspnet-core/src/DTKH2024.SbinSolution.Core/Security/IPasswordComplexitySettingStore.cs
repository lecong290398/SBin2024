using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
