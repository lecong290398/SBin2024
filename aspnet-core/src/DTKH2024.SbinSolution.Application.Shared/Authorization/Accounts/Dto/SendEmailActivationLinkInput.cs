using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}