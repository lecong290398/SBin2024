using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace DTKH2024.SbinSolution.Organizations.Dto
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; } 
    }
}