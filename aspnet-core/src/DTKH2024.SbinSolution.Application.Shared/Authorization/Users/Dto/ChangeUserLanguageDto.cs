using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
