using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}