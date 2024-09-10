using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
