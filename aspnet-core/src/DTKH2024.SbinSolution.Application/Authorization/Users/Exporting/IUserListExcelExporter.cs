using System.Collections.Generic;
using DTKH2024.SbinSolution.Authorization.Users.Dto;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}