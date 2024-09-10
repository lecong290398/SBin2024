using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}