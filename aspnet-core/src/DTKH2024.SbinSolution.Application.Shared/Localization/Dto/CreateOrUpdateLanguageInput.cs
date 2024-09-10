using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}