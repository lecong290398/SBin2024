using System.Text.RegularExpressions;

namespace DTKH2024.SbinSolution.Web.Xss
{
    
    public class DefaultHtmlSanitizer : IHtmlSanitizer
    {
        public string Sanitize(string html)
        {
            return Regex.Replace(html, "<.*?>|&.*?;", string.Empty);
        }
    }
}