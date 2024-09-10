using Abp.Dependency;

namespace DTKH2024.SbinSolution.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}