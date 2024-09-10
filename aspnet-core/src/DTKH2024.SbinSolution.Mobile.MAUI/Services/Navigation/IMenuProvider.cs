using DTKH2024.SbinSolution.Models.NavigationMenu;

namespace DTKH2024.SbinSolution.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}