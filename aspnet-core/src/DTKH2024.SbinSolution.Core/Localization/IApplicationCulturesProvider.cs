using System.Globalization;

namespace DTKH2024.SbinSolution.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}